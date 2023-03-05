using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DominoServer
{
    public partial class ServerForm : Form
    {
        public ServerForm()
        {
            InitializeComponent();
        }

        Game game;
        private TcpListener listener; // создаём объект класса TcpListener, чтобы слушать соединение с клиентом
        private Thread recieveConnections; // поток для получения клиентских подключений
        internal bool disconnected = false;
        private string name;
        private const int PORT = 50000;

        private void startButton_Click(object sender, EventArgs e)
        {
            startButton.Enabled = false;
            int playersAmount = twoRB.Checked ? 2 : threeRB.Checked ? 3 : 4;
            game = new Game(playersAmount);
            // принимаем соединения в другом потоке 
            recieveConnections = new Thread(new ThreadStart(SetUp));
            recieveConnections.Start();
        }

        private void ServerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(Environment.ExitCode);
        }

        // делегат, который позволяет вызывать метод DisplayMessage в потоке, который создает и поддерживает графический интерфейс 
        private delegate void DisplayDelegate(string message);

        // Метод DisplayMessage устанавливает свойство Text displayTextBox потокобезопасным способом
        internal void DisplayMessage(string message)
        {
            // если изменение displayTextBox не является потокобезопасным
            if (displayTextBox.InvokeRequired)
            {
                // использовать унаследованный метод Invoke для выполнения DisplayMessage
                // через делегата 
                Invoke(new DisplayDelegate(DisplayMessage),
                new object[] { (message + "\n\r") });
            }
            else // чтобы изменить displayTextBox в текущем потоке
                displayTextBox.AppendText(message + "\n");
        }

        #region манипуляции для чтения имен противников

        private delegate void DisplayName1Delegate(string nameText);

        internal void DisplayName1(string nameText)
        {
            if (name1.InvokeRequired)
            {
                Invoke(new DisplayName1Delegate(DisplayName1),
                new object[] { nameText });
            }
            else
                name1.Text = nameText;
        }

        private delegate void DisplayName2Delegate(string nameText);

        internal void DisplayName2(string nameText)
        {
            if (name2.InvokeRequired)
            {
                Invoke(new DisplayName2Delegate(DisplayName2),
                new object[] { nameText });
            }
            else
                name2.Text = nameText;
        }

        internal string readName1()
        {
            try
            {
                return name1.Text;
            }
            catch (InvalidOperationException)
            {
                return "Ошибка";
            }

        }

        internal string readName2()
        {
            try
            {
                return name2.Text;
            }
            catch (InvalidOperationException)
            {
                return "Ошибка";
            }
        }
        #endregion

        // принимаем соединения от игроков
        public void SetUp()
        {
            DisplayMessage("Ожидание подключения " + game.PlayersAmount + " игроков...");
            name = Dns.GetHostName();
            DisplayMessage("Host-name данного сервера: " + name);
            IPAddress localIP = Dns.GetHostAddresses(name)[0];
            // создаем экземпляр класса TcpListener (сервер ждёт запроса по своему же адресу)
            listener = new TcpListener(localIP, PORT);
            listener.Start();
            //принять каждого игрока и запустить каждому поток
            for (int i = 0; i < game.PlayersAmount; i++)
            {
                game.Players[i] = new Player(listener.AcceptTcpClient(), this, game, i + 1);
                game.AddPlayerThread(i);
                DisplayMessage(game.CurPlayersAmount + " игрок подключился");
                for (int j = 0; j < i; j++)
                {
                    lock (game.Players[j])
                    {
                        game.Players[j].threadSuspended = false;
                        Monitor.Pulse(game.Players[j]); //разрешить действовать другому игроку?
                    }
                }
            }
        }

        /*public bool ValidMove(int location, int player)
        {
            // не даем другому потоку осуществить ход 
            lock (this)
            {
                // ждем, пока не настанет очередь текущего игрока 
                while (player != currentPlayer)
                    Monitor.Wait(this);
                Monitor.Pulse(this);
                    return true;
            }
        }*/
    }
}
