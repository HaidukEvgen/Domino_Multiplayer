using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DominoServer
{
    public class Game
    {
        private Player[] players;
        private Thread[] playerThreads;
        public Game(int playersAmount)
        {
            PlayersAmount = playersAmount;
            players = new Player[PlayersAmount];
            playerThreads = new Thread[PlayersAmount];
        }
        public Thread[] PlayerThreads
        {
            get { return playerThreads; } 
            set { playerThreads = value; }
        }
        public Player[] Players { get { return players; } }
        public int CurPlayersAmount { get; set; } = 0;
        public int PlayersAmount { get; set; }
        public int CurPlayerOrder { get; set; } = 1;
        public bool IsGoing { get; set; } = true;
        public void AddPlayerThread(int i)
        {
            Players[i].Thread = new Thread(new ThreadStart(Players[i].Run));
            Players[i].Thread.Name = "Поток игрока " + (i + 1);
            Players[i].Thread.Start();
            PlayerThreads[i] = Players[i].Thread;
        }
        public void GetNextTurn()
        {
            if (CurPlayerOrder == PlayersAmount)
            {
                CurPlayerOrder = 1;
            }
            else
                CurPlayerOrder++;
        }

        public void FindFirstTurn()
        {
        }
    }
}
