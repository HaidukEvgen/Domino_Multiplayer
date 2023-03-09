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
        private TcpListener listener;
        private Thread recieveConnections;
        internal bool disconnected = false;
        private string name;
        private const int PORT = 50000;

        private void startButton_Click(object sender, EventArgs e)
        {
            startButton.Enabled = false;
            int playersAmount = twoRB.Checked ? 2 : threeRB.Checked ? 3 : 4;
            game = new Game(playersAmount);
            recieveConnections = new Thread(new ThreadStart(SetUp));
            recieveConnections.Start();
        }

        private void ServerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (var player in game.Players)
            {
                player.CloseConnections();
            }
            Environment.Exit(Environment.ExitCode);
        }

        private delegate void DisplayDelegate(string message);

        internal void DisplayMessage(string message)
        {
            if (displayTextBox.InvokeRequired)
            {
                Invoke(new DisplayDelegate(DisplayMessage),
                new object[] { (message + "\n\r") });
            }
            else 
                displayTextBox.AppendText(message + "\n");
        }

        public void SetUp()
        {
            DisplayMessage("Ожидание подключения " + game.PlayersAmount + " игроков...");
            name = Dns.GetHostName();
            DisplayMessage("Host-name данного сервера: " + name);
            IPAddress localIP = Dns.GetHostAddresses(name)[0];
            listener = new TcpListener(localIP, PORT);
            listener.Start();
            for (int i = 0; i < game.PlayersAmount; i++)
            {
                game.Players[i] = new Player(listener.AcceptTcpClient(), this, game);
                game.AddPlayerThread(i);
                DisplayMessage(game.CurPlayersAmount + " игрок подключился");
                for (int j = 0; j < i; j++)
                {
                    lock (game.Players[j])
                    {
                        game.Players[j].threadSuspended = false;
                        Monitor.Pulse(game.Players[j]);
                    }
                }
            }
        }
    }
}
