using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net.Sockets;
using System.Threading;

namespace DominoServer
{
    public class Player
    {
        internal TcpClient tcpClient;
        private readonly NetworkStream socketStream;
        private readonly BinaryWriter writer;
        private readonly BinaryReader reader;
        private readonly Game game;
        private Thread thread;
        private readonly ServerForm server;
        public static bool isTurnDone = false;
        public static bool isRoundOver = false;
        private string winnerName;
        private int winnerPoints;
        static List<PlayerStats> stats = new List<PlayerStats>();
        private string connectionString = @"Data Source=DESKTOP-GLBQFFP;Initial Catalog=Domino;Integrated Security=True";
        private struct PlayerStats
        {
            public string name;
            public int winsAmount;
            public int lossesAmount;
            public PlayerStats(string name, int winsAmount, int lossesAmount)
            {
                this.name = name;
                this.winsAmount = winsAmount;
                this.lossesAmount = lossesAmount;
            }
        }
        public Thread Thread
        {
            get { return thread; }
            set { thread = value; }
        }
        public string name;
        internal bool threadSuspended = true;
        enum MessageType
        {
            Text,
            Image
        }

        public Player(TcpClient tcpClient, ServerForm server, Game game)
        {
            this.tcpClient = tcpClient;
            this.game = game;
            this.server = server;
            this.socketStream = tcpClient.GetStream();
            this.writer = new BinaryWriter(socketStream/*, Encoding.UTF8*/);
            this.reader = new BinaryReader(socketStream);
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
                    while (threadSuspended || game.RegisteredPlayers < game.PlayersAmount)
                        Monitor.Wait(this);
                }
            }
        }

        public void Run()
        {
            while (true)
            {
                if (LoginPlayer())
                    break;
            }
            for (int i = 0; i < game.CurPlayersAmount; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    lock (game.Players[j])
                    {
                        game.Players[j].threadSuspended = false;
                        Monitor.Pulse(game.Players[j]);
                    }
                }
            }

            SendResponses(game.CurPlayersAmount.ToString(), game.PlayersAmount.ToString());
            WaitForPlayers();
            PrepareRound();
            while (game.IsGoing)
            {
                //SkipOldRequests();
                try
                {
                    while (tcpClient.Available == 0)
                    {
                        if (isRoundOver)
                            PrepareRound();

                    }
                }
                catch (ObjectDisposedException)
                {
                    System.Windows.Forms.MessageBox.Show("Игрок " + name + " отключился. Продолжение игры невозможно");
                    CloseServer();
                }
                if (ThisThreadTurn())
                {
                    bool gameIsGoing = ProcessTurn();
                    if (!gameIsGoing)
                    {
                        UpdateStatistics(winnerName);
                        BroadcastMessage(game, RP.GAME_ENDED, "Игра окончена, победитель - игрок " + winnerName, " набрал " + winnerPoints +" очков");
                        string fileName = Process.GetCurrentProcess().MainModule.FileName;
                        Process.Start(fileName);
                        Environment.Exit(0);
                        break;
                    }
                }
            }
        }

        private bool ProcessTurn()
        {
            //process first turn
            if (game.DominoGame.isFirstTurn)
            {
                string message;
                while (!isTurnDone)
                {
                    message = ReceiveRequest();
                    if (!message.StartsWith(RQ.MY_TURN))
                        ProcessMessage(message);
                    else
                        ProcessMessage(RQ.FIRST_TURN + message);
                }
                BroadcastResponses(RQ.PLAYER_DOMINOES + game.CurPlayerOrder.ToString(), game);
                server.DisplayMessage("Игрок " + game.CurPlayerOrder + " сделал первый ход");
                game.DominoGame.isFirstTurn = false;
                goto EndOfTurn;
            }
            //process bazar until can make move
            while (!game.DominoGame.CanMakeTurn(game.CurPlayerOrder) && game.DominoGame.bazar.Length > 0)
            {
                SendResponses(RP.BAZAR, string.Join(",", game.DominoGame.bazarTilesLeft));
                string message = ReceiveRequest();
                ProcessMessage(message);
            }
            //process turn
            if (game.DominoGame.CanMakeTurn(game.CurPlayerOrder))
            {
                while (!isTurnDone)
                {
                    string message = ReceiveRequest();
                    ProcessMessage(message);
                }
                server.DisplayMessage("Игрок " + game.CurPlayerOrder + " походил");
            }
            else
            {
                ReceiveRequest();
                ReceiveRequest();
                SendResponses(RP.TURN_SKIPPED, game.CurPlayerOrder.ToString());
                server.DisplayMessage("Игрок " + game.CurPlayerOrder + " пропустил ход");
            }
            BroadcastResponses(RQ.PLAYER_DOMINOES + game.CurPlayerOrder.ToString(), game);
        EndOfTurn:
            isTurnDone = false;
            DominoGame.Winner winner = game.DominoGame.IsRoundOver(game.CurPlayersAmount);
            if (winner != DominoGame.Winner.None)
            {
                ProcessMessage(RQ.PLAYER_DOMINOES + game.CurPlayerOrder.ToString());
                int points = game.DominoGame.GetRoundResults(ref winner);
                BroadcastMessage(game, RP.ROUND_ENDED, RP.WINNER + $"|{(int)winner}" + $"|{points}" + $"|{game.Players[(int)winner - 1].name}");
                winnerName = game.Players[(int)winner - 1].name;
                winnerPoints = points;
                if (game.DominoGame.IsGameOver())
                    return false;
                game.DominoGame.InitNewRound();
                isRoundOver = true;
                PrepareRound();
                return true;
            }
            game.GetNextTurn();
            server.DisplayMessage(RP.frases[RP.PLAYER_GOES] + game.CurPlayerOrder);
            BroadcastMessage(game, RP.PLAYER_GOES + game.CurPlayerOrder);
            return true;
        }

        private void PrepareRound()
        {
            game.DominoGame.isFirstTurn = true;
            ProcessMessage(RQ.IMAGES);
            for (int i = 0; i < game.PlayersAmount; i++)
            {
                ProcessMessage(RQ.PLAYER_DOMINOES + (i + 1).ToString());
            }
            SendResponses(RP.GAME_STARTED);
            foreach (var stat in stats)
            {
                SendResponses(stat.name, stat.winsAmount.ToString(), stat.lossesAmount.ToString());
            }
            SendResponses(RP.PLAYER_GOES + game.CurPlayerOrder);
            if (ThisThreadTurn())
                server.DisplayMessage(RP.frases[RP.PLAYER_GOES] + game.CurPlayerOrder);
            isRoundOver = false;
        }

        private void ProcessMessage(string message)
        {
            if (message.StartsWith(RQ.PLAYER_DOMINOES))
            {
                //server.DisplayMessage("Игрок " + game.CurPlayerOrder + " запросил массив домино");
                SendListResponse(game.DominoGame.GetTilesArr(Int32.Parse(message[^1].ToString())));
                return;
            }
            else
            if (message.StartsWith(RQ.MY_TURN))
            {
                //server.DisplayMessage("Игрок " + game.CurPlayerOrder + " запросил положить домино");
                var responses = game.DominoGame.TryToPlaceDomino(game.CurPlayerOrder, message);
                SendTurnInfo(responses);
                return;
            }
            else
            if (message.StartsWith(RQ.GAME_NUMS))
            {
                //server.DisplayMessage("Игрок " + game.CurPlayerOrder + " запросил крайние числа");
                SendListResponse(game.DominoGame.GetGameNums());
                return;
            }
            else
            if (message.StartsWith(RQ.DIR_TURN))
            {
                //server.DisplayMessage("Игрок " + game.CurPlayerOrder + " запросил положить домино");
                var responses = game.DominoGame.PlaceDominoDifferent(game.CurPlayerOrder, message);
                SendTurnInfo(responses);
                return;
            }
            else
            if (message.StartsWith(RQ.GET_BAZAR_DOMINO))
            {
                //server.DisplayMessage("Игрок " + game.CurPlayerOrder + " запросил домино с базара");
                if (!game.DominoGame.bazarTilesLeft.Remove(Int32.Parse(message.Split('|')[1]))) throw new Exception("bad bazar removing");
                SendResponses(RP.BAZAR_DOMINO + game.DominoGame.GetBazarDomino(game.CurPlayerOrder));
                BroadcastResponses(RQ.PLAYER_DOMINOES + game.CurPlayerOrder.ToString(), game);
                return;
            }
            else
            if (message.StartsWith(RQ.FIRST_TURN))
            {
                //server.DisplayMessage("Игрок " + game.CurPlayerOrder + " сделал первый ход");
                var responses = game.DominoGame.PlaceFirstDomino(game.CurPlayerOrder, message);
                SendTurnInfo(responses);
                return;
            }
            else
            if (message.StartsWith(RQ.IMAGES))
            {
                //server.DisplayMessage("Игрок " + game.CurPlayerOrder + " запросил картинки с домино");
                SendResponses(RP.IMAGES);
                SendImages(DominoGame.GetImagesFromFile());
                return;
            }
            else
            {
                server.DisplayMessage("Неизвестное сообщение от игрока");
            }
        }

        private void SendTurnInfo(List<string> responses)
        {
            if (!responses[0].StartsWith(RP.DOMINO_PLACED))
                SendListResponse(responses);
            else
            {
                foreach (var response in responses)
                    BroadcastMessage(game, response);
                var s = responses[1];
                foreach (var player in game.Players)
                {
                    player.SendImage(Image.FromFile(s.Substring(s.IndexOf("|") + 1, s.LastIndexOf("|") - s.IndexOf("|") - 1)));
                }
            }
        }

        private string ReceiveRequest()
        {
            string message = "";
            try
            {
                message = reader.ReadString();
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
            lock (writer)
            {
                foreach (var line in lines)
                {
                    writer.Write((int)MessageType.Text);
                    writer.Write(line);
                }
            }
        }

        private void SendListResponse(List<string> response)
        {
            lock (writer)
            {
                foreach (var line in response)
                {
                    writer.Write((int)MessageType.Text);
                    writer.Write(line);
                }
            }
        }

        private static void BroadcastMessage(Game game, params string[] lines)
        {
            foreach (var player in game.Players)
            {
                foreach (var message in lines)
                {
                    player.writer.Write((int)MessageType.Text);
                    player.writer.Write(message);
                }
            }
        }

        private static void BroadcastResponses(string response, Game game)
        {
            foreach (var player in game.Players)
            {
                player.ProcessMessage(response);
            }
        }

        private void SendImage(Image image)
        {
            lock (writer)
            {
                writer.Write((int)MessageType.Image);
                MemoryStream ms = new();
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] imageBytes = ms.ToArray();
                writer.Write(imageBytes.Length);
                writer.Write(imageBytes);
            }
        }

        private void SendImages(List<Image> images)
        {
            lock (writer)
            {
                foreach (var image in images)
                {
                    SendImage(image);
                }
            }
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

        private void UpdateStatistics(string winnerName)
        {
            string connectionString = @"Data Source=DESKTOP-GLBQFFP;Initial Catalog=Domino;Integrated Security=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "UPDATE Users SET winsAmount = winsAmount + 1 WHERE Username = @Username";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", winnerName);
                    command.ExecuteNonQuery();
                }

                foreach (var player in game.Players)
                {
                    if (player.name != winnerName)
                    {
                        query = "UPDATE Users SET lossesAmount = lossesAmount + 1 WHERE Username = @Username";
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@Username", player.name);
                            command.ExecuteNonQuery();
                        }
                    }
                }
            }
        }

        private enum RegistrationResult
        {
            Registered = 0,
            Failed = 1,
            LoggedIn = 2
        }

        private bool LoginPlayer()
        {
            var registrationType = (RegistrationResult)reader.ReadInt32();
            string username = reader.ReadString();
            string passwordHash = reader.ReadString();
            if (registrationType == RegistrationResult.Registered)
            {
                if (UserExists(username))
                {
                    writer.Write((int)RegistrationResult.Failed);
                    return false;
                }
                else
                {
                    RegisterNewUser(username, passwordHash);
                    writer.Write((int)RegistrationResult.Registered);
                    game.RegisteredPlayers += 1;
                    game.Players[game.CurPlayersAmount - 1].name = username;
                    stats.Add(GetUserStats(username));
                    return true;
                }
            }
            if (UserExists(username))
            {
                if (UserIsInGame(username))
                {
                    writer.Write((int)RegistrationResult.Failed);
                    return false;
                }
                if (IsUserValid(username, passwordHash))
                {
                    writer.Write((int)RegistrationResult.LoggedIn);
                    game.RegisteredPlayers += 1;
                    game.Players[game.CurPlayersAmount - 1].name = username;
                    stats.Add(GetUserStats(username));
                    return true;
                }
                else
                {
                    writer.Write((int)RegistrationResult.Failed);
                    return false;
                }
            }
            else
            {
                writer.Write((int)RegistrationResult.Failed);
                return false;
            }
        }

        private bool UserIsInGame(string username)
        {
            foreach (var player in game.Players)
            {
                if (player != null && player.name == username)
                    return true;
            }
            return false;
        }

        public void RegisterNewUser(string username, string password)
        {
            string query = "INSERT INTO Users (username, password) VALUES (@Username, @Password)";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Password", password);
                    command.ExecuteNonQuery();
                }
            }
        }

        public bool IsUserValid(string username, string password)
        {
            string query = "SELECT COUNT(*) FROM Users WHERE Username = @Username AND Password = @Password";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Password", password);

                    int count = (int)command.ExecuteScalar();

                    return count > 0;
                }
            }
        }

        public bool UserExists(string username)
        {
            string query = "SELECT COUNT(*) FROM Users WHERE Username = @Username";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    int count = (int)command.ExecuteScalar();
                    return count > 0;
                }
            }
        }

        private PlayerStats GetUserStats(string username)
        {
            string query = "SELECT winsAmount, lossesAmount FROM Users WHERE Username = @Username";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        reader.Read();
                        int winsAmount = (int)reader["winsAmount"];
                        int lossesAmount = (int)reader["lossesAmount"];
                        return new PlayerStats(username, winsAmount, lossesAmount);
                    }
                }
            }
        }

    }
}
