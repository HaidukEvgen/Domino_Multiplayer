using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace DominoServer
{
    public class DominoGame
    {
        public const int MAX_AMOUNT = 28;
        public const int MAX_NUM = 6;
        public const int INITIAL_AMOUNT = 7;
        public Domino[] tilesArr = new Domino[MAX_AMOUNT];
        public Domino[] bazar;
        public List<int> bazarTilesLeft;
        public DominoPlayer[] players;
        private readonly int playersAmount;
        private readonly int pointsAim;
        public int LeftNum { get; set; }
        public int RightNum { get; set; }

        public int Round { get; set; } = 1;
        public bool isFirstTurn = true;
        public void FindFirstTurn()
        { }


        internal void InitNewRound()
        {
            this.players = new DominoPlayer[playersAmount];
            this.bazar = playersAmount == 4 ? Array.Empty<Domino>() : new Domino[MAX_AMOUNT - playersAmount * INITIAL_AMOUNT];
            this.bazarTilesLeft = new List<int>();
            for (int i = 1; i <= bazar.Length; i++)
            {
                bazarTilesLeft.Add(i);
            }
            InitGame();
        }

        public DominoGame(int _playersAmount, int _pointsAim)
        {
            this.playersAmount = _playersAmount;
            this.pointsAim = _pointsAim;
            this.players = new DominoPlayer[_playersAmount];
            this.bazar = _playersAmount == 4 ? Array.Empty<Domino>() : new Domino[MAX_AMOUNT - _playersAmount * INITIAL_AMOUNT];
            this.bazarTilesLeft = new List<int>();
            for (int i = 1; i <= bazar.Length; i++)
            {
                bazarTilesLeft.Add(i);
            }
            InitGame();
        }

        public override string ToString()
        {
            return LeftNum.ToString() + ":" + RightNum.ToString();
        }

        public void InitGame()
        {
            InitTiles();
            GiveTiles();
        }

        private void InitTiles()
        {
            int index = 0;
            for (int i = 0; i <= MAX_NUM; i++)
                for (int j = i; j <= MAX_NUM; j++)
                    tilesArr[index++] = new Domino(i, j);
            for (int i = 0; i < MAX_AMOUNT; i++)
            {
                Random random = new();
                index = random.Next(0, MAX_AMOUNT);
                (tilesArr[index], tilesArr[i]) = (tilesArr[i], tilesArr[index]);
            }
        }

        private void GiveTiles()
        {
            int index = 0;
            for (int i = 0; i < playersAmount; i++)
            {
                players[i] = new DominoPlayer();
                Domino[] temp = new Domino[INITIAL_AMOUNT];
                Array.Copy(tilesArr, index, temp, 0, INITIAL_AMOUNT);
                index += INITIAL_AMOUNT;
                players[i].SetTiles(temp);
            }
            if (bazar == null) return;
            Array.Copy(tilesArr, index, bazar, 0, bazar.Length);
        }

        public List<string> GetTilesArr(int playerOrder)
        {
            List<string> response = new()
            {
                RP.PLAYER_DOMINOES,
                playerOrder.ToString()
            };
            DominoPlayer player = players[playerOrder - 1];
            int tilesAmount = player.CurTilesAmount;
            response.Add(tilesAmount.ToString());
            for (int i = 0; i < tilesAmount; i++)
                response.Add(player.tilesArr[i].ToString());
            Player.isTurnDone = false;
            return response;
        }

        public List<string> GetGameNums()
        {
            List<string> response = new()
            {
                RP.GAME_NUMS,
                ToString()
            };
            return response;
        }

        internal string GetBazarDomino(int playerOrder)
        {
            Domino bazarDomino = bazar[^1];
            bazar = bazar.SkipLast(1).ToArray();
            players[playerOrder - 1].AddTile(bazarDomino);
            return bazarDomino.ToString();
        }

        internal bool CanMakeTurn(int playerOrder)
        {
            for (int i = 0; i < players[playerOrder - 1].CurTilesAmount; i++)
            {
                var domino = players[playerOrder - 1].tilesArr[i];
                if (CanBePlaced(domino))
                {
                    return true;
                }
            }
            return false;
        }

        public List<string> TryToPlaceDomino(int playerOrder, string message)
        {
            List<string> response = new();
            DominoPlayer player = players[playerOrder - 1];
            Domino attempt = new(message[message.IndexOf(":") - 1] - '0',
                                 message[message.IndexOf(":") + 1] - '0');
            int index = 0;
            if (!HasThisDomino(attempt, player, ref index))
            {
                response.Add(RP.NO_SUCH_DOMINO);
                Player.isTurnDone = false;
                return response;
            }
            if (!CanBePlaced(attempt))
            {
                response.Add(RP.UNSUTABLE_DOMINO);
                Player.isTurnDone = false;
                return response;
            }
            if (CanBePlacedDifferent(attempt))
            {
                response.Add(RP.WHICH_DIRECTION);
                Player.isTurnDone = false;
                return response;
            }
            bool placeLeft = CanBePlacedLeft(attempt);
            string path;
            if (placeLeft)
            {
                if (attempt.FirstNum == LeftNum)
                {
                    LeftNum = attempt.SecondNum;
                    path = RIGHT_PATH;
                }
                else
                {
                    LeftNum = attempt.FirstNum;
                    path = LEFT_PATH;
                }
            }
            else
            {
                if (attempt.FirstNum == RightNum)
                {
                    RightNum = attempt.SecondNum;
                    path = LEFT_PATH;
                }
                else
                {
                    RightNum = attempt.FirstNum;
                    path = RIGHT_PATH;
                }
            }
            response.Add(RP.DOMINO_PLACED);
            response.Add(attempt.ToString()
                         + '|'
                         + PathToDomino(attempt.FirstNum, attempt.SecondNum, path)
                         + '|'
                         + (placeLeft ? "1" : "0"));
            player.DeleteTile(index);
            Player.isTurnDone = true;
            return response;
        }

        internal List<string> PlaceFirstDomino(int playerOrder, string message)
        {
            List<string> response = new();
            Domino attempt = new(message[message.IndexOf(":") - 1] - '0',
                                 message[message.IndexOf(":") + 1] - '0');
            LeftNum = attempt.FirstNum;
            RightNum = attempt.SecondNum;
            int index = 0;
            var player = players[playerOrder - 1];
            if (!HasThisDomino(attempt, player, ref index))
            {
                response.Add(RP.NO_SUCH_DOMINO);
                Player.isTurnDone = false;
                return response;
            }
            response.Add(RP.DOMINO_PLACED);
            response.Add(attempt.ToString()
                         + '|'
                         + PathToDomino(LeftNum, RightNum, LEFT_PATH)
                         + '|'
                         + '2');
            player.DeleteTile(index);
            Player.isTurnDone = true;
            return response;
        }

        public List<string> PlaceDominoDifferent(int playerOrder, string message)
        {
            List<string> response = new();
            DominoPlayer player = players[playerOrder - 1];
            Domino attempt = new(message[message.IndexOf(":") - 1] - '0',
                                 message[message.IndexOf(":") + 1] - '0');
            int index = 0;
            string path;
            if (HasThisDomino(attempt, player, ref index) && message[^1] == '1')
            {
                if (attempt.FirstNum == LeftNum)
                {
                    LeftNum = attempt.SecondNum;
                    path = RIGHT_PATH;
                }
                else
                {
                    LeftNum = attempt.FirstNum;
                    path = LEFT_PATH;
                }
            }
            else
            {
                if (attempt.FirstNum == RightNum)
                {
                    RightNum = attempt.SecondNum;
                    path = LEFT_PATH;
                }
                else
                {
                    RightNum = attempt.FirstNum;
                    path = RIGHT_PATH;
                }
            }
            player.DeleteTile(index);
            response.Add(RP.DOMINO_PLACED);
            response.Add(attempt.ToString()
                        + '|'
                        + PathToDomino(attempt.FirstNum, attempt.SecondNum, path)
                        + '|'
                        + (message[^1] == '1' ? "1" : "0"));
            Player.isTurnDone = true;
            return response;
        }

        private bool CanBePlaced(Domino attempt)
        {
            return CanBePlacedLeft(attempt) || CanBePlacedRight(attempt);
        }

        bool CanBePlacedLeft(Domino attempt)
        {
            return (attempt.FirstNum == LeftNum) || (attempt.SecondNum == LeftNum);
        }

        bool CanBePlacedRight(Domino attempt)
        {
            return (attempt.FirstNum == RightNum) || (attempt.SecondNum == RightNum);
        }

        bool CanBePlacedDifferent(Domino attempt)
        {
            return LeftNum == RightNum ||
                (attempt.FirstNum == LeftNum && attempt.SecondNum == RightNum) ||
                (attempt.FirstNum == RightNum && attempt.SecondNum == LeftNum);
        }

        const string UP_PATH = "BlackDominoes/Up/";
        const string DOWN_PATH = "BlackDominoes/Down/";
        const string LEFT_PATH = "BlackDominoes/Left/";
        const string RIGHT_PATH = "BlackDominoes/Right/";

        private static string PathToDomino(int firstNum, int secondNum, string path)
        {
            return path + "Black" + firstNum.ToString() + "-" + secondNum.ToString() + ".png";
        }

        internal static List<Image> GetImagesFromFile()
        {
            var images = new List<Image>();
            for (int i = 0; i <= MAX_NUM; i++)
            {
                for (int j = i; j <= MAX_NUM; j++)
                {
                    images.Add(Image.FromFile(PathToDomino(i, j, UP_PATH)));
                }
            }
            return images;
        }

        private static bool HasThisDomino(Domino attempt, DominoPlayer player, ref int index)
        {
            bool hasDomino = false;
            for (int i = 0; i < player.CurTilesAmount; i++)
            {
                if (player.tilesArr[i].Equals(attempt))
                {
                    index = i;
                    hasDomino = true;
                    break;
                }
            }
            if (!hasDomino)
                return false;
            return true;
        }

        public Winner IsRoundOver(int playersAmount)
        {
            int count = 0;
            for (int i = 0; i < players.Length; i++)
            {
                var player = players[i];
                if (player.CurTilesAmount == 0)
                    return (Winner)Enum.Parse(typeof(Winner), "Player" + (i + 1));
                if (!CanMakeTurn(i + 1))
                    count++;
            }
            if (count == playersAmount && bazar.Length == 0)
                return Winner.Fish;
            return Winner.None;
        }

        public int GetRoundResults(ref Winner winner)
        {
            int curWinner = 1;
            int curWinningSum;
            if (winner == Winner.Fish)
            {
                curWinningSum = players[0].GetTilesSum();
                for (int i = 1; i < players.Length; i++)
                {
                    var player = players[i];
                    var sum = player.GetTilesSum();
                    if (sum > curWinningSum)
                    {
                        curWinner = i + 1;
                        curWinningSum = sum;
                    }
                }
            }
            else
            {
                curWinner = (int)winner;
                curWinningSum = 0;
                foreach (var player in players)
                {
                     curWinningSum += player.GetTilesSum();
                }
            }
            players[curWinner - 1].Points += curWinningSum;
            winner = (Winner)curWinner;
            return curWinningSum;
        }

        internal bool IsGameOver()
        {
            foreach (var player in players)
            {
                if (player.GetTilesSum() >= pointsAim)
                    return true;
            }
            return false;
        }

        public enum Winner
        {
            None = 0,
            Player1 = 1,
            Player2 = 2,
            Player3 = 3,
            Player4 = 4,
            Fish = 5
        }
    }
}