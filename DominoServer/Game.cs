using System.Threading;

namespace DominoServer
{
    public class Game
    {
        private Player[] players;
        private Thread[] playerThreads;
        public DominoGame DominoGame { get; }
        
        public Game(int playersAmount, int pointsAim)
        {
            PlayersAmount = playersAmount;
            PointsAim = pointsAim;
            players = new Player[PlayersAmount];
            playerThreads = new Thread[PlayersAmount];
            DominoGame = new DominoGame(playersAmount, pointsAim);
        }
        public Thread[] PlayerThreads
        {
            get { return playerThreads; }
            set { playerThreads = value; }
        }
        public Player[] Players { get { return players; } }
        public int CurPlayersAmount { get; set; } = 0;
        public int RegisteredPlayers { get; set; } = 0;
        public int PlayersAmount { get; set; }
        public int PointsAim { get; set; }
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
    }
}
