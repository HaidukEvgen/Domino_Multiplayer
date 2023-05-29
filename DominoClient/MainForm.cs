using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;

namespace DominoClient
{
    public partial class MainForm : Form
    {
        public MainForm(TcpClient tcpClient, NetworkStream stream, BinaryWriter writer, BinaryReader reader, string playerName)
        {
            this.tcpClient = tcpClient;
            this.playerName = playerName;
            this.stream = stream;
            this.writer = writer;
            this.reader = reader;
            InitializeComponent();
        }

        private Thread connectionThread;
        private TcpClient tcpClient;
        private NetworkStream stream;
        private BinaryWriter writer;
        private BinaryReader reader;
        private bool gameIsGoing = true;
        private readonly string playerName;
        private Game game;
        private const int PICTURE_WIDTH = 35;
        private const int PICTURE_HEIGHT = 67;
        private Image emptyImageVertical;
        private Image emptyImageHorizontal;
        List<PlayerStats> stats;
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

        enum MessageType
        {
            Text,
            Image
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            bazarPanel.Location = new Point(ClientSize.Width / 2 - bazarPanel.Size.Width / 2, ClientSize.Height / 2 - bazarPanel.Size.Height / 2);
            LeftArrowPB.Location = new Point(ClientSize.Width / 2 - (LeftArrowPB.Width + clockPB.Width / 2), ClientSize.Height - 250);
            RightArrowPB.Location = new Point(LeftArrowPB.Right + clockPB.Width, LeftArrowPB.Top);
            clockPB.Location = new Point(LeftArrowPB.Right, LeftArrowPB.Top);
            crossPB.Location = new Point(ClientSize.Width - crossPB.Width - 5, 5);
            hidePB.Location = new Point(crossPB.Left - hidePB.Width, 5);
            connectionThread = new Thread(new ThreadStart(Run));
            connectionThread.Start();
        }


        public void Run()
        {
            reader.ReadInt32();
            int orderNumber = Int32.Parse(reader.ReadString());
            reader.ReadInt32();
            int playersAmount = Int32.Parse(reader.ReadString());
            game = new Game(playersAmount, orderNumber);
            try
            {
                while (gameIsGoing)
                {
                    MessageType messageType = (MessageType)reader.ReadInt32();
                    switch (messageType)
                    {
                        case (MessageType.Text):
                            {
                                string message = reader.ReadString();
                                if (message != null)
                                {
                                    ProcessMessage(message);
                                }
                                break;
                            }
                        case (MessageType.Image):
                            {
                                RecieveImage();
                                break;
                            }
                        default:
                            {
                                MessageBox.Show("Unknown message type");
                                break;
                            }
                    }
                }
            }
            catch (SystemException)
            {
                if (gameIsGoing)
                    MessageBox.Show("Потеряно соединение с сервером. Игра закончена.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                    MessageBox.Show("Игра закончена.", "Завершение игры", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            try
            {
                BeginInvoke(new Action(() => { Close(); }));
            }
            catch (Exception) { }
        }

        private Image RecieveImage()
        {
            int imageSize = reader.ReadInt32();
            byte[] imageBytes = reader.ReadBytes(imageSize);
            MemoryStream ms = new(imageBytes);
            Image image = Image.FromStream(ms);
            return image;
        }

        private void SendMessage(string message)
        {
            writer.Write(message);
        }

        #region Изменение компонентов безопасным способом

        private delegate void DisplayDelegate(string message);

        internal void DisplayMessage(string message)
        {
            if (infoTextBox.InvokeRequired)
            {
                Invoke(new DisplayDelegate(DisplayMessage),
                new object[] { (message + "\n\r") });
            }
            else
                infoTextBox.AppendText(message + "\n");
        }


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
                Invoke(new controlDelegate(ChangeControlEnabled), new object[] { enabled, control });
            }
            else
                control.Enabled = enabled;
        }
        private void ChangeControlVisible(bool enabled, Control control)
        {
            if (control.InvokeRequired)
            {
                Invoke(new controlDelegate(ChangeControlVisible), new object[] { enabled, control });
            }
            else
                control.Visible = enabled;
        }

        private void ChangeBazarVisibility(string bazarStr)
        {
            List<string> list = new(bazarStr.Split(',').ToList());
            BeginInvoke(new Action(() =>
            {
                for (int i = 1; i <= 14; i++)
                {
                    string name = $"bazarPB{i}";
                    Control[] matches = bazarPanel.Controls.Find(name, true);
                    PictureBox pictureBox = matches[0] as PictureBox;
                    if (list.Contains(i.ToString()))
                        pictureBox.Visible = true;
                    else
                        pictureBox.Visible = false;
                }
            }));
        }

        private delegate void addTileDelegate(PictureBox tile);
        private void AddTileToForm(PictureBox tile)
        {
            if (InvokeRequired)
            {
                Invoke(new addTileDelegate(AddTileToForm), new object[] { tile });
            }
            else
                Controls.Add(tile);
        }

        private delegate void deleteTilesDelegate();
        private void DeleteTilesFromForm()
        {
            if (InvokeRequired)
            {
                Invoke(new deleteTilesDelegate(DeleteTilesFromForm), Array.Empty<object>());
            }
            else
            {
                for (int i = Controls.Count - 1; i >= 0; i--)
                {
                    Control ctrl = Controls[i];
                    if (ctrl is PictureBox && ctrl.Name.StartsWith("tile"))
                    {
                        Controls.Remove(ctrl);
                        ctrl.Dispose();
                    }
                }
            }
        }

        private delegate void redrawTileDelegate(PictureBox picture, int left, int top);

        private void RedrawTile(PictureBox picture, int left, int top)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new redrawTileDelegate(RedrawTile), new object[] { picture, left, top });
            }
            else
            {
                picture.Left = left; picture.Top = top;
                //picture.Refresh();
            }
        }

        private delegate void flipTileDelegate(PictureBox picture, RotateFlipType rotateType);

        private void FlipTile(PictureBox picture, RotateFlipType rotateType)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new flipTileDelegate(FlipTile), new object[] { picture, rotateType });
            }
            else
            {
                picture.Image.RotateFlip(rotateType);
                switch (rotateType)
                {
                    case (RotateFlipType.Rotate90FlipNone):
                        {
                            picture.Width = PICTURE_HEIGHT;
                            picture.Height = PICTURE_WIDTH;
                            break;
                        }
                    case (RotateFlipType.Rotate270FlipNone):
                        {
                            picture.Width = PICTURE_HEIGHT;
                            picture.Height = PICTURE_WIDTH;
                            break;
                        }
                }
                //picture.Refresh();
            }
        }

        private delegate void changeImageDelegate(PictureBox picture, Image image);

        private void ChangeImage(PictureBox picture, Image image)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new changeImageDelegate(ChangeImage), new object[] { picture, image });
            }
            else
            {
                picture.Image = image;
                //picture.Refresh();
            }
        }


        #endregion

        public void ProcessMessage(string message)
        {
            switch (message)
            {
                case RP.GAME_STARTED:
                    {
                        FillStats();
                        DisplayMessage(RP.frases[RP.GAME_STARTED]);
                        break;
                    }
                case RP.PLAYER_DOMINOES:
                    {
                        string line = "";
                        reader.ReadInt32();
                        int playerOrder = Int32.Parse(reader.ReadString());
                        reader.ReadInt32();
                        int tilesAmount = Int32.Parse(reader.ReadString());
                        for (int i = 0; i < tilesAmount; i++)
                        {
                            reader.ReadInt32();
                            line += reader.ReadString() + " ";
                        }
                        game.players[playerOrder - 1].RefreshTilesArr(line.Trim());
                        DisplayPlayerDominoes(playerOrder);
                        break;
                    }
                case RP.NO_SUCH_DOMINO:
                    {
                        MessageBox.Show(RP.frases[RP.NO_SUCH_DOMINO]);
                        break;
                    }
                case RP.UNSUTABLE_DOMINO:
                    {
                        MessageBox.Show(RP.frases[RP.UNSUTABLE_DOMINO]);
                        break;
                    }
                case RP.GAME_ENDED:
                    {
                        reader.ReadInt32();
                        var res = MessageBox.Show(reader.ReadString(), "Игра окончена", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        gameIsGoing = false;
                        break;
                    }
                case RP.GAME_NUMS:
                    {
                        reader.ReadInt32();
                        ChangeLabel(reader.ReadString(), numsLabel);
                        break;
                    }
                case RP.WHICH_DIRECTION:
                    {
                        ChangeControlVisible(true, LeftArrowPB);
                        ChangeControlVisible(true, RightArrowPB);
                        EnableUserTiles(false);
                        break;
                    }
                case RP.BAZAR:
                    {
                        reader.ReadInt32();
                        string bazarStr = reader.ReadString();
                        ChangeBazarVisibility(bazarStr);
                        ChangeControlVisible(true, bazarPanel);
                        break;
                    }
                case RP.IMAGES:
                    {
                        CreateImages();
                        break;
                    }
                case RP.DOMINO_PLACED:
                    {
                        reader.ReadInt32();
                        string dominoStr = reader.ReadString();
                        var placedDomino = new Domino(dominoStr[dominoStr.IndexOf(":") - 1] - '0', dominoStr[dominoStr.IndexOf(":") + 1] - '0');
                        ChangeControlEnabled(false, GetPictureBox(placedDomino));
                        reader.ReadInt32();
                        Image image = RecieveImage();
                        PictureBox pictureBox = GetPictureBox(placedDomino);
                        if (dominoStr.IndexOf("Left") != -1 || dominoStr.IndexOf("Right") != -1)
                            if (pictureBox.Height > pictureBox.Width)
                                FlipTile(pictureBox, RotateFlipType.Rotate90FlipNone);
                            else
                            if (pictureBox.Height < pictureBox.Width)
                                FlipTile(pictureBox, RotateFlipType.Rotate90FlipNone);
                        ChangeImage(pictureBox, image);
                        PlaceDominoOnBoard(pictureBox, dominoStr[^1].ToString());
                        break;
                    }
                case RP.TURN_SKIPPED:
                    {
                        reader.ReadInt32();
                        int playerOrder = Int32.Parse(reader.ReadString());
                        if (playerOrder == game.OrderNumber)
                        {
                            ChangeControlVisible(false, bazarPanel);
                            EnableUserTiles(false);
                            DisplayMessage("Вы пропустили ход");
                        }
                        else
                        {
                            DisplayMessage($"Игрок {playerOrder} пропустил ход");
                        }
                        break;
                    }
                case RP.ROUND_ENDED:
                    {
                        reader.ReadInt32();
                        string line = reader.ReadString();
                        var parts = line.Split('|');
                        int winner = Int32.Parse(parts[1]);
                        int winningSum = Int32.Parse(parts[2]);
                        string winnerName = parts[3];
                        IncreaseStat(winnerName, winningSum);
                        BeginInvoke(
                            new Action(() =>
                            {
                                MessageBox.Show($"Раунд окончен. Победитель - игрок {winner} получает {winningSum} очков", "Конец раунда", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            })
                         );
                        game.PrepareForNewRound();
                        break;
                    }
                default:
                    {
                        if (message.StartsWith(RP.PLAYER_GOES))
                        {
                            if (message == RP.PLAYER_GOES + game.OrderNumber)
                            {
                                SendMessage(RQ.PLAYER_DOMINOES + game.OrderNumber);
                                SendMessage(RQ.GAME_NUMS);
                                DisplayMessage(RP.frases[RP.PLAYER_GOES]);
                                EnableUserTiles(true);
                                ChangeControlVisible(true, clockPB);
                            }
                            else
                            {
                                DisplayMessage($"Ход игрока {message[^1]}");
                                EnableUserTiles(false);
                                ChangeControlVisible(false, clockPB);
                            }
                        }
                        else
                        if (message.StartsWith(RP.BAZAR_DOMINO))
                        {
                            var bazarDomino = new Domino(message[message.IndexOf(":") - 1] - '0',
                                                         message[message.IndexOf(":") + 1] - '0');
                            if (CanBePlaced(bazarDomino))
                            {
                                ChangeControlEnabled(true, GetPictureBox(bazarDomino));
                                ChangeControlVisible(false, bazarPanel);
                            }
                        }
                        else
                        if (message.StartsWith(RP.WAITING_PLAYER))
                        {
                            DisplayMessage(RP.frases[RP.WAITING_PLAYER] + message[^1] + "...");
                        }
                        break;
                    }
            }
        }

        private void IncreaseStat(string winnerName, int winningSum)
        {
            foreach (DataGridViewRow row in statsGV.Rows)
            {
                if (row.Cells[0].Value != null && row.Cells[0].Value.ToString() == winnerName)
                {
                    if (row.Cells[1].Value != null && int.TryParse(row.Cells[1].Value.ToString(), out int currentValue))
                    {
                        int newValue = currentValue + winningSum;
                        row.Cells[1].Value = newValue.ToString();
                        break;
                    }
                }
            }
        }


        private void FillStats()
        {
            stats = new List<PlayerStats>();
            for (int i = 0; i < game.players.Length; i++)
            {
                PlayerStats temp = new PlayerStats();
                reader.ReadInt32();
                temp.name = reader.ReadString();
                reader.ReadInt32();
                temp.winsAmount = Int32.Parse(reader.ReadString());
                reader.ReadInt32();
                temp.lossesAmount = Int32.Parse(reader.ReadString());
                stats.Add(temp);
            }
            foreach (PlayerStats stats in stats)
            {
                DataGridViewRow row = new DataGridViewRow();
                row.CreateCells(statsGV);

                string username = stats.name;
                if (UsernameExists(username))
                    return;
                int points = 0;
                int winsamount = stats.winsAmount;
                int lossesamount = stats.lossesAmount;

                row.Cells[0].Value = username;
                row.Cells[1].Value = points;
                row.Cells[2].Value = winsamount;
                row.Cells[3].Value = lossesamount;

                statsGV.Rows.Add(row);
            }
            int rowHeight = statsGV.RowTemplate.Height;
            int rowCount = statsGV.Rows.Count;
            int headerHeight = statsGV.ColumnHeadersHeight;
            statsGV.Height = headerHeight + (rowHeight * rowCount);
        }

        private bool UsernameExists(string username)
        {
            foreach (DataGridViewRow row in statsGV.Rows)
            {
                if (row.Cells[0].Value != null && row.Cells[0].Value.ToString() == username)
                {
                    return true;
                }
            }
            return false;
        }


        private bool CanBePlaced(Domino attempt)
        {
            return CanBePlacedLeft(attempt) || CanBePlacedRight(attempt);
        }

        bool CanBePlacedLeft(Domino attempt)
        {
            return (attempt.FirstNum == game.LeftNum) || (attempt.SecondNum == game.LeftNum);
        }

        bool CanBePlacedRight(Domino attempt)
        {
            return (attempt.FirstNum == game.RightNum) || (attempt.SecondNum == game.RightNum);
        }

        private void EnableUserTiles(bool enabled)
        {
            Player player = game.players[game.OrderNumber - 1];
            foreach (var tile in player.tilesArr)
            {
                if (game.chain.Count == 0 && enabled)
                {
                    PictureBox picture = GetPictureBox(tile);
                    ChangeControlEnabled(true, picture);
                }
                else
                if (enabled && CanBePlaced(tile))
                {
                    PictureBox picture = GetPictureBox(tile);
                    ChangeControlEnabled(enabled, picture);
                }
                else
                {
                    PictureBox picture = GetPictureBox(tile);
                    ChangeControlEnabled(false, picture);
                }
            }
        }

        private void PlaceDominoOnBoard(PictureBox dominoPictureBox, string placeLeft)
        {
            int left = 0;
            int top = 0;
            if (game.chain.Count == 0)
            {
                left = (ClientSize.Width - dominoPictureBox.Width) / 2;
                top = (ClientSize.Height - dominoPictureBox.Height) / 2;
                game.chain.AddFirst(dominoPictureBox);
                GetDominoNums(dominoPictureBox, out int firstNum, out int secondNum);
                game.LeftNum = firstNum;
                game.RightNum = secondNum;
            }
            else
            {
                PictureBox firstDomino = game.chain.First.Value;
                PictureBox lastDomino = game.chain.Last.Value;
                if (placeLeft == "1")
                {
                    left = firstDomino.Left - PICTURE_HEIGHT;
                    top = firstDomino.Top;
                    game.chain.AddFirst(dominoPictureBox);
                    GetDominoNums(dominoPictureBox, out int firstNum, out int secondNum);
                    game.LeftNum = game.LeftNum == firstNum ? secondNum : firstNum;
                }
                else if (placeLeft == "0")
                {
                    left = lastDomino.Right;
                    top = lastDomino.Top;
                    game.chain.AddLast(dominoPictureBox);
                    GetDominoNums(dominoPictureBox, out int firstNum, out int secondNum);
                    game.RightNum = game.RightNum == secondNum ? firstNum : secondNum;
                }
            }
            /*if (dominoPictureBox.Tag.ToString() == "double")
            {
                dominoPictureBox.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                (left, top) = (top, left);
            }*/
            //FlipTile(dominoPictureBox, rotateType);
            /*dominoPictureBox.BeginInvoke(new Action(() =>
            {
                switch (rotateType)
                {
                    case (RotateFlipType.Rotate90FlipNone):
                        {
                            dominoPictureBox.Width = PICTURE_HEIGHT;
                            dominoPictureBox.Height = PICTURE_WIDTH;
                            break;
                        }
                    case (RotateFlipType.Rotate270FlipNone):
                        {
                            dominoPictureBox.Width = PICTURE_HEIGHT;
                            dominoPictureBox.Height = PICTURE_WIDTH;
                            break;
                        }
                }
                dominoPictureBox.Image.RotateFlip(rotateType);
                dominoPictureBox.Left = left; dominoPictureBox.Top = top;
                dominoPictureBox.Refresh();
            }));*/
            RedrawTile(dominoPictureBox, left, top);
        }

        private static void GetDominoNums(PictureBox dominoPictureBox, out int firstNum, out int secondNum)
        {
            var tileName = dominoPictureBox.Name;
            firstNum = Int32.Parse(tileName[tileName.LastIndexOf("_") - 1].ToString());
            secondNum = Int32.Parse(tileName[tileName.LastIndexOf("_") + 1].ToString());
        }

        Dictionary<string, Image> images;
        private void CreateImages()
        {
            DeleteTilesFromForm();
            const int MAX_NUM = 7;
            int count = 0;
            images = new Dictionary<string, Image>(Game.MAX_AMOUNT);
            for (int i = 0; i < MAX_NUM; i++)
            {
                for (int j = i; j < MAX_NUM; j++)
                {
                    reader.ReadInt32();
                    Image image = RecieveImage();
                    images.Add(i.ToString() + "_" + j.ToString(), image);
                    if (i == 0 && j == 0)
                    {
                        emptyImageVertical = (Image)image.Clone();
                        image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        emptyImageHorizontal = (Image)image.Clone();
                        image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    }
                    PictureBox tile = new()
                    {
                        Image = image,
                        Tag = (i == j ? "double" : ""),
                        Visible = false,
                        Enabled = false,
                        Name = "tile_" + i.ToString() + "_" + j.ToString(),
                        Size = new Size(PICTURE_WIDTH, PICTURE_HEIGHT),
                        SizeMode = PictureBoxSizeMode.Zoom
                    };
                    tile.Click += Tile_Click;
                    count++;
                    AddTileToForm(tile);
                }
            }
        }

        private void DisplayPlayerDominoes(int orderNumber)
        {
            var player = game.players[orderNumber - 1];
            RedrawPlayerDominoes(player);
        }

        private void RedrawPlayerDominoes(Player player)
        {
            int SPACE = 10;
            int totalWidth = player.tilesArr.Length * (PICTURE_WIDTH + SPACE) - SPACE;
            int left = 0;
            int top = 0;
            var tablePos = player.TablePos;
            Image image = null;
            switch (tablePos)
            {
                case Player.TablePosition.Bottom:
                    left = (Width - totalWidth) / 2;
                    top = Player.coordinates[Player.TablePosition.Bottom].Y;
                    break;
                case Player.TablePosition.Left:
                    left = Player.coordinates[Player.TablePosition.Left].X;
                    top = (Height - totalWidth) / 2;
                    image = (Image)emptyImageHorizontal.Clone();
                    break;
                case Player.TablePosition.Right:
                    left = Width - (PICTURE_HEIGHT + Player.coordinates[Player.TablePosition.Right].X);
                    top = (Height - totalWidth) / 2;
                    image = (Image)emptyImageHorizontal.Clone();
                    break;
                case Player.TablePosition.Top:
                    left = (Width - totalWidth) / 2;
                    top = Player.coordinates[Player.TablePosition.Top].Y;
                    image = (Image)emptyImageVertical.Clone();
                    break;
            }
            for (int i = 0; i < player.tilesArr.Length; i++)
            {
                var domino = player.tilesArr[i];
                PictureBox picture = GetPictureBox(domino);
                RedrawTile(picture, left, top);

                ChangeControlVisible(true, picture);
                if (tablePos == Player.TablePosition.Bottom || tablePos == Player.TablePosition.Top)
                {
                    if (image != null) ChangeImage(picture, image);
                    left += PICTURE_WIDTH + SPACE;
                }
                else
                {
                    FlipTile(picture, RotateFlipType.Rotate90FlipNone);
                    ChangeImage(picture, image);
                    top += PICTURE_WIDTH + SPACE;
                }
            }
        }

        public PictureBox GetPictureBox(Domino domino)
        {
            int l = domino.FirstNum;
            int r = domino.SecondNum;
            string name = $"tile_{l}_{r}";
            Control[] matches = Controls.Find(name, true);
            PictureBox pictureBox = matches[0] as PictureBox;
            return pictureBox;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {

        }
        private void BazarButton_Click(object sender, EventArgs e)
        {
            (sender as PictureBox).Visible = false;
            SendMessage(RQ.GET_BAZAR_DOMINO + '|' + TrimStringToNumber((sender as PictureBox).Name));
        }
        public static string TrimStringToNumber(string str)
        {
            return new string(str.Reverse().TakeWhile(char.IsDigit).Reverse().ToArray());
        }

        private void Tile_Click(object sender, EventArgs e)
        {
            GetDominoNums(sender as PictureBox, out int firstNum, out int secondNum);
            game.CurTurn = firstNum + ":" + secondNum;
            SendMessage(RQ.MY_TURN + firstNum + ":" + secondNum);
        }

        private void LeftArrow_Click(object sender, EventArgs e)
        {
            SendMessage(RQ.DIR_TURN + game.CurTurn + "1");
            ChangeControlVisible(false, LeftArrowPB);
            ChangeControlVisible(false, RightArrowPB);
        }

        private void RightArrow_Click(object sender, EventArgs e)
        {
            SendMessage(RQ.DIR_TURN + game.CurTurn + "0");
            ChangeControlVisible(false, LeftArrowPB);
            ChangeControlVisible(false, RightArrowPB);
        }

        private void statsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            statsGV.Visible = !statsGV.Visible;
        }

        private void crossPB_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void hidePB_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
    }
}
