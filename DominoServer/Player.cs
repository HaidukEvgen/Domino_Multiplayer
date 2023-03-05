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
        internal TcpClient tcpClient; // сокет для приема соединения
        private NetworkStream socketStream; // данные из потока
        private StreamWriter writer; // облегчает запись в поток
        private StreamReader reader; // облегчает чтение из потока 
        private Game game;
        private Thread thread;
        private ServerForm server;
        public Thread Thread { 
            get { return thread; } set { thread = value; }
        }
        private int number; // номер игрока 
        private string name;
        internal bool threadSuspended = true; // если мы ждем второго игрока 
        internal string Id { get; } = Guid.NewGuid().ToString();

        public Player(TcpClient tcpClient, ServerForm server, Game game, int playerNumber)
        {
            this.tcpClient = tcpClient;
            this.game = game;
            this.server = server;
            this.number = playerNumber;
            this.socketStream = tcpClient.GetStream();
            this.writer = new StreamWriter(socketStream, Encoding.UTF8) { AutoFlush = true };
            this.reader = new StreamReader(socketStream);
            game.CurPlayersAmount += 1;
        }

        private bool ThisThreadTurn()
        {
            return Thread.CurrentThread.ManagedThreadId == game.PlayerThreads[game.CurPlayerOrder - 1].ManagedThreadId;
        }
        
        // позволяет игрокам делать ходы и получать ходы от другого игрока 
        
        private void WaitForPlayers()
        {
            while (game.CurPlayersAmount < game.PlayersAmount)
            {
                int waitingForPlayer = game.CurPlayersAmount + 1;
                threadSuspended = true;
                writer.WriteLine("Ожидаем игрока " + waitingForPlayer);
                // ждем уведомления от сервера, что другой игрок подключился
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
            writer.WriteLine(Id);
            writer.WriteLine(game.CurPlayersAmount);
            WaitForPlayers();
            writer.WriteLine("Игра началась");
            writer.WriteLine("Ходит игрок " + game.CurPlayerOrder);
            if (ThisThreadTurn())
                server.DisplayMessage("Ходит игрок " + game.CurPlayerOrder);
            while (game.IsGoing)
            {
                while (tcpClient.Available == 0)
                { }
                if (ThisThreadTurn())
                {
                    string message = reader.ReadLine();
                    server.DisplayMessage("Игрок " + game.CurPlayerOrder + " походил");
                    game.GetNextTurn();
                    server.DisplayMessage("Ходит игрок " + game.CurPlayerOrder);
                    BroadcastMessage("Ходит игрок " + game.CurPlayerOrder, game);
                }
            }
        #region trash
        /*bool done = false;
        // отобразить на сервере, что соединение было установлено

        //записываем имена игроков в лэйблы
        if (server.readName1() == "name1")
        {
            server.DisplayName1(playerName);
        }
        else
        {
            server.DisplayName2(playerName);
        }

        // отправить клиенту метку текущего игрока
        //writer.Write(mark);

        server.DisplayMessage("Игрок под именем " + playerName + " присоединился и ходит \""
                                         + (number == 0 ? 'X' : 'O') + "\".\r\n");

        // отправить клиенту имя текущего игрока
        writer.Write(playerName);

        // X должен ждать пока подключится другой игрок
        if (mark == 'X')
        {
            writer.Write("Ожидаем второго игрока.");

            // ждем уведомления от сервера, что другой игрок подключился
            lock (this)
            {
                while (threadSuspended)
                    Monitor.Wait(this);
            }
            writer.Write("Второй игрок подключился. Ваш ход.");
        }
        else
        {
            writer.Write("Вы подключились. Ход противника.");
        }


        while (opponentName == "name1" || opponentName == "name2" || opponentName == "...")
        {
            //проверяем какое из двух имен является именем оппонента
            if (server.readName1() == playerName)
            {
                opponentName = server.readName2();
            }
            else
            {
                opponentName = server.readName1();
            }
        }
        //отправляем имя оппонента клиенту
        writer.Write(opponentName);


        // играем
        while (!done)
        {
            // ждём, пока данные станут доступными 
            while (connection.Available == 0)
            {
                Thread.Sleep(1000);

                if (server.disconnected)
                {
                    server.DisplayMessage("Что-то пошло не так... Игра закончена.");
                    return;
                }
            }

            // получаем данные
            int location = reader.ReadInt32();

            // если ход корректный, отобразим его на сервере
            if (server.ValidMove(location, number) && !server.GameOver())
            {
                server.DisplayMessage(playerName + ": ход на ячейку № " + location + ".\r\n");
                writer.Write("Ход был корректным.");
            }
            else if (server.GameOver())
            {
                server.DisplayMessage(playerName + ": ход на ячейку № " + location + ".\r\n");
                writer.Write(condition);
            }
            else // ход некорректный
                writer.Write("Ход был некорректным. Попробуйте снова.");

            // если игра окончена, установливаем для параметра done значение true, чтобы выйти из цикла while 

            if (server.GameOver())
            {
                if (condition == "победитель1" || condition == "победитель2" || condition == "победитель3" ||
                    condition == "победитель4" || condition == "победитель5" || condition == "победитель6" ||
                    condition == "победитель7" || condition == "победитель8")
                {
                    server.DisplayMessage("Игрок " + playerName + " победил.");
                    writer.Write(condition);
                }
                else if (condition == "ничья")
                {
                    server.DisplayMessage("Это ничья");
                    writer.Write(condition);
                }

                lock (this)
                {
                    playAgain = reader.ReadString();
                    while (playAgain == null)
                        Monitor.Wait(this);
                }

                if (playAgain == "Выход")
                {
                    server.DisplayMessage("\r\nИгра окончена.\r\n");
                    done = true;
                }
            }
        }*/
        #endregion
            writer.Close();
            reader.Close();
            socketStream.Close();
            tcpClient.Close();
        }

        private static void BroadcastMessage(string message, Game game)
        {
            foreach (var player in game.Players)
            {
                player.writer.WriteLine(message);
            }
        }
    }
}
