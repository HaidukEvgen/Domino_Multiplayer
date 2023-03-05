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
        private Thread outputThread; // поток для полученных от сервера данных
        private TcpClient tcpClient; // клиент для установления соединения
        private NetworkStream stream; // данные из потока
        private StreamWriter writer; // облегчает запись в поток
        private StreamReader reader; // облегчает чтение из потока
        private string id;
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
            //подключиться к серверу и получить связанный сетевой поток 
            try
            {
                //преобразуем dns-имя в ip-адрес
                string address = Dns.GetHostAddresses(hostNameTB.Text)[0].ToString();
                tcpClient = new TcpClient(address, PORT);
                stream = tcpClient.GetStream();
                writer = new StreamWriter(stream) { AutoFlush = true };
                reader = new StreamReader(stream);
                // запустить новый поток для отправки/получения сообщения
                outputThread = new Thread(new ThreadStart(Run));
                outputThread.Start();
                name = nameTextBox.Text == null ? nameTextBox.PlaceholderText : nameTextBox.Text;
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

        // управляющий поток, который позволяет непрерывно обновлять отображение игрового поля
        public void Run()
        {
            // получить id игрока 
            id = reader.ReadLine();
            orderNumber = Int32.Parse(reader.ReadLine());
            // обработка входящих сообщений 
            try
            {
                // получение сообщений, отправленных клиенту
                while (gameIsGoing)
                {
                    string message = reader.ReadLine();
                    if (message != null)
                        ProcessMessage(message);
                }
            }
            catch (IOException)
            {
                MessageBox.Show("Потеряно соединение с сервером. Игра закончена.","Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #region Изменение label безопасным способом

        // делегат, который позволяет вызывать метод по изменению 
        // компонента label в потоке безопасным способом
        private delegate void labelDelegate(string message, Label desiredLabel);

        // метод ChangeLabel устанавливает свойство label
        // потокобезопасным способом
        private void ChangeLabel(string message, Label desiredLabel)
        {
            // если изменение desiredLabel не является потокобезопасным
            if (desiredLabel.InvokeRequired)
            {
                // используем метод Invoke, чтобы
                //выполнить ChangeLabel с помощью делегата
                Invoke(new labelDelegate(ChangeLabel),
                new object[] { message, desiredLabel });
            }
            else
                desiredLabel.Text = message;
        }

        private delegate void buttonDelegate(bool enabled, Button desiredButton);
        private void ChangeButtonEnabled(bool enabled, Button desiredButton)
        {
            if (desiredButton.InvokeRequired)
            {
                Invoke(new buttonDelegate(ChangeButtonEnabled),
                new object[] { enabled, desiredButton });
            }
            else
                desiredButton.Enabled = enabled;
        }
        #endregion

        // обработка сообщений, отправленных клиенту
        public void ProcessMessage(string message)
        {
            switch(message) {
                case ("Ожидаем игрока 2"):
                    {
                        ChangeLabel("Ожидаем игрока 2...", statusLabel);
                        break;
                    }
                case ("Ожидаем игрока 3"):
                    {
                        ChangeLabel("Ожидаем игрока 3...", statusLabel);
                        break;
                    }
                case ("Ожидаем игрока 4"):
                    {
                        ChangeLabel("Ожидаем игрока 4...", statusLabel);
                        break;
                    }
                case ("Игра началась"):
                    {
                        ChangeLabel("Игра началась", statusLabel);
                        break;
                    }
                default:
                    {
                        if(message == "Ходит игрок " + orderNumber)
                        {
                            ChangeLabel("Ваш ход", statusLabel);
                            ChangeButtonEnabled(true, makeTurnButton);
                        } else 
                            ChangeLabel(message, statusLabel);
                        break;
                    }
            }
        }

        private void makeTurnButton_Click(object sender, EventArgs e)
        {
            writer.WriteLine("Я игрок " + orderNumber + " и я походил ");
            ChangeButtonEnabled(false, makeTurnButton);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(Environment.ExitCode);
        }
    }
}
