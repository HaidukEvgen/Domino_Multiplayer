using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DominoServer
{
    public class DominoGame
    {
        public const int MAX_AMOUNT = 28;
        public const int MAX_NUM = 6;
        public const int INITIAL_AMOUNT = 7;
        public Domino[] tilesArr = new Domino[MAX_AMOUNT];
        public Domino[] bazar;
        public DominoPlayer[] players;
        private int playersAmount;
        public int LeftNum { get; set; }
        public int RightNum { get; set; }

        public int Round { get; set; } = 1;
        public bool isFirstTurn = true;
        public void FindFirstTurn()
        { }

        public DominoGame(int _playersAmount)
        {
            this.playersAmount = _playersAmount;
            this.players = new DominoPlayer[_playersAmount];
            this.bazar = _playersAmount == 4 ? null : new Domino[MAX_AMOUNT - _playersAmount * INITIAL_AMOUNT];
            InitGame();
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
                Random random = new Random();
                index = random.Next(0, MAX_AMOUNT);
                Domino temp = tilesArr[i];
                tilesArr[i] = tilesArr[index];
                tilesArr[index] = temp;
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
            List<string> response = new List<string>();
            response.Add(RP.YOUR_DOMINOES);
            DominoPlayer player = players[playerOrder - 1];
            int tilesAmount = player.CurTilesAmount;
            response.Add(tilesAmount.ToString());
            for (int i = 0; i < tilesAmount; i++)
                response.Add(player.tilesArr[i].FirstNum + ":" + player.tilesArr[i].SecondNum);
            Player.isTurnDone = false;
            return response;
        }

        public List<string> GetGameNums()
        {
            List<string> response = new List<string>();
            response.Add(RP.GAME_NUMS);
            response.Add(LeftNum + ":" + RightNum);
            return response;
        }

        public List<string> PlaceDominoDifferent(int playerOrder, string message)
        {
            List<string> response = new List<string>();
            DominoPlayer player = players[playerOrder - 1];
            Domino attempt = new Domino(message[message.IndexOf(":") - 1] - '0',
                                        message[message.IndexOf(":") + 1] - '0');
            int index = 0;
            if (HasThisDomino(attempt, player, ref index) && message[message.Length - 1] == '1')
                LeftNum = attempt.FirstNum == LeftNum ? attempt.SecondNum : attempt.FirstNum;
            else
                RightNum = attempt.FirstNum == RightNum ? attempt.SecondNum : attempt.FirstNum;
            player.DeleteTile(index);
            Player.isTurnDone = true;
            return response;
        }

        internal void GetBazarDomino(int playerOrder)
        {
            Domino bazarDomino = bazar[bazar.Length - 1];
            bazar = bazar.SkipLast(1).ToArray();
            players[playerOrder - 1].AddTile(bazarDomino);
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
            List<string> response = new List<string>();
            DominoPlayer player = players[playerOrder - 1];
            Domino attempt = new Domino(message[message.IndexOf(":") - 1] - '0',
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
            if (CanBePlacedLeft(attempt))
                LeftNum = attempt.FirstNum == LeftNum ? attempt.SecondNum : attempt.FirstNum;
            else
                RightNum = attempt.FirstNum == RightNum ? attempt.SecondNum : attempt.FirstNum;
            player.DeleteTile(index);
            Player.isTurnDone = true;
            return response;
        }

        private bool CanBePlaced(Domino attempt)
        {
            return CanBePlacedLeft(attempt) || CanBePlacedRight(attempt);
        }

        internal void PlaceFirstDomino(int playerOrder, string message)
        {
            Domino attempt = new Domino(message[message.IndexOf(":") - 1] - '0',
                                        message[message.IndexOf(":") + 1] - '0');
            LeftNum = attempt.FirstNum;
            RightNum = attempt.SecondNum;
            int index = 0;
            var player = players[playerOrder - 1];
            HasThisDomino(attempt, player, ref index);
            player.DeleteTile(index);
            Player.isTurnDone = true;
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
        private bool HasThisDomino(Domino attempt, DominoPlayer player, ref int index)
        {
            bool hasDomino = false;
            for (int i = 0; i < player.CurTilesAmount; i++)
            {
                if (player.tilesArr[i].Equals(attempt))
                {
                    index = i;
                    hasDomino = true;
                }
            }
            if (!hasDomino)
                return false;
            return true;
        }

        public bool IsRoundOver(int playersAmount)
        {
            int count = 0;
            for (int i = 0; i < players.Length; i++)
            {
                var player = players[i];
                if (player.CurTilesAmount == 0)
                    return true;
                if (!CanMakeTurn(i + 1))
                    count++;
            }
            return count == playersAmount && bazar.Length == 0;
        }
    }
}