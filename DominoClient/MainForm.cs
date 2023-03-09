using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DominoClient
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }
        private Thread connectionThread;
        private TcpClient tcpClient;
        private NetworkStream stream;
        private StreamWriter writer;
        private StreamReader reader;
        private string name;
        private int orderNumber;
        private const int PORT = 50000;
        private bool gameIsGoing = true;
        private void MainForm_Load(object sender, EventArgs e)
        {
            nameTextBox.PlaceholderText += Dns.GetHostName();
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            try
            {
                string address = Dns.GetHostAddresses(hostNameTB.Text)[0].ToString();
                tcpClient = new TcpClient(address, PORT);
                stream = tcpClient.GetStream();
                writer = new StreamWriter(stream) { AutoFlush = true };
                reader = new StreamReader(stream);
                connectionThread = new Thread(new ThreadStart(Run));
                connectionThread.Start();
                name = nameTextBox.Text == "" ? nameTextBox.PlaceholderText : nameTextBox.Text;
                writer.WriteLine(name);
            }
            catch (SocketException)
            {
                MessageBox.Show("Ошибка подключения. Сервер не найден или не запущен. Попробуйте снова", "Ошибка подключения", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            startButton.Enabled = false;
            connectionDoneLabel.Visible = true;
        }

        public void Run()
        {
            orderNumber = Int32.Parse(reader.ReadLine());
            //try
            //{
                while (gameIsGoing)
                {
                    string message = reader.ReadLine();
                    if (message != null)
                    {
                        ProcessMessage(message);
                    }
                }
            /*}
            catch (Exception)
            {
                MessageBox.Show("Потеряно соединение с сервером. Игра закончена.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }*/
            this.Invoke(new Action(() => { Close(); }));
        }

        #region Изменение компонентов безопасным способом

        private delegate void labelDelegate(string message, Label desiredLabel);

        private void ChangeLabel(string message, Label desiredLabel)
        {
            if (desiredLabel.InvokeRequired)
            {
                Invoke(new labelDelegate(ChangeLabel),
                new object[] { message, desiredLabel });
            }
            else
                desiredLabel.Text = message;
        }

        private delegate void controlDelegate(bool enabled, Control control);
        private void ChangeControlEnabled(bool enabled, Control control)
        {
            if (control.InvokeRequired)
            {
                Invoke(new controlDelegate(ChangeControlEnabled),
                new object[] { enabled, control });
            }
            else
                control.Enabled = enabled;
        }
        private void ChangeControlVisible(bool enabled, Control control)
        {
            if (control.InvokeRequired)
            {
                Invoke(new controlDelegate(ChangeControlVisible),
                new object[] { enabled, control });
            }
            else
                control.Visible = enabled;
        }
        
        #endregion

        public void ProcessMessage(string message)
        {
            switch (message)
            {
                case (RP.GAME_STARTED):
                    {
                        ChangeLabel(RP.frases[RP.GAME_STARTED], statusLabel);
                        break;
                    }
                case (RP.YOUR_DOMINOES):
                    {
                        string line = "";
                        int tilesAmount = Int32.Parse(reader.ReadLine());
                        for (int i = 0; i < tilesAmount; i++)
                        {
                            line += reader.ReadLine() + " ";
                        }
                        ChangeLabel(line, MyDominoesLabel);
                        break;
                    }
                case (RP.NO_SUCH_DOMINO):
                    {
                        MessageBox.Show(RP.frases[RP.NO_SUCH_DOMINO]);
                        break;
                    }
                case (RP.UNSUTABLE_DOMINO):
                    {
                        MessageBox.Show(RP.frases[RP.UNSUTABLE_DOMINO]);
                        break;
                    }
                case (RP.GAME_ENDED):
                    {
                        MessageBox.Show(RP.frases[RP.GAME_ENDED]);
                        gameIsGoing = false;
                        break;
                    }
                case (RP.GAME_NUMS):
                    {
                        ChangeLabel(reader.ReadLine(), numsLabel);
                        break;
                    }
                case (RP.WHICH_DIRECTION):
                    {
                        ChangeControlEnabled(true, leftCheckBox);
                        break;
                    }
                case (RP.BAZAR):
                    {
                        ChangeControlVisible(true, bazarPanel);
                        break;
                    }
                default:
                    {
                        if (message == RP.PLAYER_GOES + orderNumber)
                        {
                            ChangeLabel("", MyDominoesLabel);
                            writer.WriteLine(RQ.MY_DOMINOES);
                            writer.WriteLine(RQ.GAME_NUMS);
                            ChangeLabel(RP.frases[RP.PLAYER_GOES], statusLabel);
                            ChangeControlEnabled(true, gameNumsButton);
                            ChangeControlEnabled(true, SendDominoesButton);
                        }
                        else
                        if (message.StartsWith(RP.WAITING_PLAYER))
                        {
                            ChangeLabel(RP.frases[RP.WAITING_PLAYER] + message[message.Length - 1] + "...", statusLabel);
                        }
                        //ChangeLabel(message, statusLabel);
                        break;
                    }
            }
        }


        private void gameNumsButton_Click(object sender, EventArgs e)
        {
            writer.WriteLine(RQ.GAME_NUMS);
            //ChangeButtonEnabled(false, gameNumsButton);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(Environment.ExitCode);
        }

        private void SendDominoesButton_Click(object sender, EventArgs e)
        {
            MyDominoesLabel.Text = "";
            writer.WriteLine(RQ.MY_DOMINOES);
        }

        private void placeDominoButton_Click(object sender, EventArgs e)
        {
            if (leftCheckBox.Enabled)
            {
                writer.WriteLine(RQ.DIR_TURN + firstDominoNum.Value + ":" + secondDominoNum.Value + (leftCheckBox.Checked ? "1" : "0"));
                ChangeControlEnabled(false, leftCheckBox);
            }
            else
                writer.WriteLine(RQ.MY_TURN + firstDominoNum.Value + ":" + secondDominoNum.Value);
        }

        private void bazarButton_Click(object sender, EventArgs e)
        {
            writer.WriteLine(RQ.GET_BAZAR_DOMINO);
            ChangeControlVisible(false, bazarPanel);
            writer.WriteLine(RQ.MY_DOMINOES);
        }
    }
}
