using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DominoServer
{
    public class Player
    {
        internal TcpClient tcpClient;
        private NetworkStream socketStream;
        private StreamWriter writer;
        private StreamReader reader;
        private Game game;
        private Thread thread;
        private ServerForm server;
        public static bool isTurnDone = false;
        public Thread Thread
        {
            get { return thread; }
            set { thread = value; }
        }
        private string name;
        internal bool threadSuspended = true;

        public Player(TcpClient tcpClient, ServerForm server, Game game)
        {
            this.tcpClient = tcpClient;
            this.game = game;
            this.server = server;
            this.socketStream = tcpClient.GetStream();
            this.writer = new StreamWriter(socketStream, Encoding.UTF8) { AutoFlush = true };
            this.reader = new StreamReader(socketStream);
            game.CurPlayersAmount += 1;
        }

        private bool ThisThreadTurn()
        {
            return Thread.CurrentThread.ManagedThreadId == game.PlayerThreads[game.CurPlayerOrder - 1].ManagedThreadId;
        }

        private void WaitForPlayers()
        {
            while (game.CurPlayersAmount < game.PlayersAmount)
            {
                int waitingForPlayer = game.CurPlayersAmount + 1;
                threadSuspended = true;
                SendResponses(RP.WAITING_PLAYER + waitingForPlayer);
                lock (this)
                {
                    while (threadSuspended)
                        Monitor.Wait(this);
                }
            }
        }

        public void Run()
        {
            name = reader.ReadLine();
            SendResponses(game.CurPlayersAmount.ToString());
            WaitForPlayers();
            SendResponses(RP.GAME_STARTED, RP.PLAYER_GOES + game.CurPlayerOrder);
            if (ThisThreadTurn())
                server.DisplayMessage(RP.frases[RP.PLAYER_GOES] + game.CurPlayerOrder);
            while (game.IsGoing)
            {
                SkipOldRequests();
                try
                {
                    while (tcpClient.Available == 0)
                    { }
                }
                catch (ObjectDisposedException)
                {
                    System.Windows.Forms.MessageBox.Show("Игрок " + name + " отключился. Продолжение игры невозможно");
                    CloseServer();
                }
                if (ThisThreadTurn())
                    ProcessTurn();
                if (game.DominoGame.IsRoundOver(game.CurPlayersAmount)) {
                    System.Windows.Forms.MessageBox.Show("Раунд окончен");
                    break;
                }
            }
        }

        private void SkipOldRequests()
        {
            while (tcpClient.Available != 0)
            {
                reader.ReadLine();
            }
        }

        private void ProcessTurn()
        {
            //process first turn
            if (game.DominoGame.isFirstTurn)
            {
                string message = "";
                while (true)
                {
                    message = RecieveRequest();
                    if (!message.StartsWith(RQ.MY_TURN))
                        ProcessMessage(message);
                    else
                        break;
                }
                ProcessMessage(RQ.FIRST_TURN + message);
                game.DominoGame.isFirstTurn = false;
                goto EndOfTurn;
            }
            //process bazar until can make move
            while (!game.DominoGame.CanMakeTurn(game.CurPlayerOrder) && game.DominoGame.bazar.Length > 0)
            {
                SendResponses(RP.BAZAR);
                string message = RecieveRequest();
                ProcessMessage(message);
            }
            //process turn
            if (game.DominoGame.CanMakeTurn(game.CurPlayerOrder))
            {
                while (!isTurnDone)
                {
                    string message = RecieveRequest();
                    ProcessMessage(message);
                }
                server.DisplayMessage("Игрок "  + game.CurPlayerOrder + " походил");
            }
            else
            {
                server.DisplayMessage("Игрок " + game.CurPlayerOrder + " пропустил ход");
            }
            EndOfTurn:
            isTurnDone = false;
            game.GetNextTurn();
            server.DisplayMessage(RP.frases[RP.PLAYER_GOES] + game.CurPlayerOrder);
            BroadcastMessage(RP.PLAYER_GOES + game.CurPlayerOrder, game);
        }

        private void ProcessMessage(string message)
        {
            if (message.StartsWith(RQ.MY_DOMINOES))
            {
                server.DisplayMessage("Игрок " + game.CurPlayerOrder + " запросил его массив домино");
                SendListResponse(game.DominoGame.GetTilesArr(game.CurPlayerOrder));
            }
            else
            if (message.StartsWith(RQ.MY_TURN))
            {
                server.DisplayMessage("Игрок " + game.CurPlayerOrder + " запросил положить домино");
                SendListResponse(game.DominoGame.TryToPlaceDomino(game.CurPlayerOrder, message));
            }
            else
            if (message.StartsWith(RQ.GAME_NUMS))
            {
                server.DisplayMessage("Игрок " + game.CurPlayerOrder + " запросил крайние числа");
                SendListResponse(game.DominoGame.GetGameNums());
            }
            else
            if (message.StartsWith(RQ.DIR_TURN))
            {
                server.DisplayMessage("Игрок " + game.CurPlayerOrder + " запросил положить домино");
                SendListResponse(game.DominoGame.PlaceDominoDifferent(game.CurPlayerOrder, message));
            }
            else
            if (message.StartsWith(RQ.GET_BAZAR_DOMINO))
            {
                server.DisplayMessage("Игрок " + game.CurPlayerOrder + " запросил домино с базара");
                game.DominoGame.GetBazarDomino(game.CurPlayerOrder);
            }
            else
            if (message.StartsWith(RQ.FIRST_TURN))
            {
                server.DisplayMessage("Игрок " + game.CurPlayerOrder + " сделал первый ход");
                game.DominoGame.PlaceFirstDomino(game.CurPlayerOrder, message);
            }
        }

        private string RecieveRequest()
        {
            string message = "";
            try
            {
                message = reader.ReadLine();
            }
            catch (IOException)
            {
                System.Windows.Forms.MessageBox.Show("Игрок " + name + " отключился. Продолжение игры невозможно");
                CloseServer();
            }
            return message;
        }

        private void SendResponses(params string[] lines)
        {
            foreach (var line in lines)
                writer.WriteLine(line);
        }

        private void SendListResponse(List<string> response)
        {
            foreach (var line in response)
                writer.WriteLine(line);
        }

        private static void BroadcastMessage(string message, Game game)
        {
            foreach (var player in game.Players)
                player.writer.WriteLine(message);
        }

        public void CloseConnections()
        {
            //writer.WriteLine(RP.GAME_ENDED);
            writer.Close();
            reader.Close();
            socketStream.Close();
            tcpClient.Close();
        }
        private delegate void CloseDelegate();

        private void CloseServer()
        {
            if (server.InvokeRequired)
                server.Invoke(new CloseDelegate(CloseServer));
            else
                server.Close();
        }
    }
}
