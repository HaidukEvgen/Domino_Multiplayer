using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DominoClient
{
    public partial class RegisterForm : Form
    {
        public RegisterForm()
        {
            InitializeComponent();
        }

        private void RegisterForm_Load(object sender, EventArgs e)
        {
            hostNameTB.Text = Dns.GetHostName();
        }

        TcpClient tcpClient = null;
        NetworkStream stream;
        BinaryWriter writer;
        BinaryReader reader;

        private enum RegistrationResult
        {
            Registered = 0,
            Failed = 1,
            LoggedIn = 2
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            Task task = Task.Run(() => Register(registringNewUser: false));
        }

        private bool ConnectToServer()
        {
            const int PORT = 50000;
            string serverName = Dns.GetHostAddresses(hostNameTB.Text)[0].ToString();
            try
            {
                tcpClient = new TcpClient(serverName, PORT);
                stream = tcpClient.GetStream();
                writer = new BinaryWriter(stream);
                reader = new BinaryReader(stream);
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка подключения. Сервер не найден или не запущен. Попробуйте снова", "Ошибка подключения", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private void Register(bool registringNewUser)
        {
            if (tcpClient == null || !tcpClient.Connected)
                if (!ConnectToServer())
                    return;
            string userName = usernameTextBox.Text;
            string passwordHash = CalculateHash(passwordTextBox.Text);
            if (registringNewUser)
            {
                writer.Write((int)RegistrationResult.Registered);
                writer.Write(userName);
                writer.Write(passwordHash);
                if ((RegistrationResult)reader.ReadInt32() == RegistrationResult.Failed)
                {
                    MessageBox.Show("Ошибка регистрации. Пользователь с таким логином и паролем уже существует. Попробуйте снова", "Не удалось зарегистрировать аккаунт", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else
            {
                writer.Write((int)RegistrationResult.LoggedIn);
                writer.Write(userName);
                writer.Write(passwordHash);
                if ((RegistrationResult)reader.ReadInt32() == RegistrationResult.Failed)
                {
                    MessageBox.Show("Ошибка авторизации. Пользователь с таким логином или паролем не обнаружен в базе данных или уже авторизовался. Попробуйте снова или зарегестрируйте новый аккаунт по кнопке внизу", "Не удалось войти в аккаунт", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            MainForm mainForm = new MainForm(tcpClient, stream, writer, reader, userName);
            BeginInvoke(new Action(() =>
            {
                Hide();
                mainForm.ShowDialog();
                tcpClient.Close();
                Show();
            }));

        }

        private static string CalculateHash(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = sha256.ComputeHash(bytes);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }

        private void RegisterButton_Click(object sender, EventArgs e)
        {
            Task.Run(() => Register(registringNewUser: true));
        }
    }
}
