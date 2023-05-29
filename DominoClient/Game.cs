using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DominoClient
{
    class Game
    {
        public const int MAX_AMOUNT = 28;
        public const int MAX_NUM = 6;
        public const int INITIAL_AMOUNT = 7;
        public Player[] players;
        public LinkedList<PictureBox> chain = new();

        public int LeftNum { get; set; }
        public int RightNum { get; set; }

        public string CurTurn { get; set; }

        public int Round { get; set; } = 1;
        public bool isFirstTurn = true;
        public int OrderNumber { get; set; }

        public Game(int _playersAmount, int order)
        {
            players = new Player[_playersAmount];
            if (_playersAmount == 2)
            {
                if (order == 1)
                {
                    players[0] = new Player(Player.TablePosition.Bottom);
                    players[1] = new Player(Player.TablePosition.Top);
                }
                else
                {
                    players[0] = new Player(Player.TablePosition.Top);
                    players[1] = new Player(Player.TablePosition.Bottom);
                }
            }
            else if (_playersAmount == 3)
            {
                if (order == 1)
                {
                    players[0] = new Player(Player.TablePosition.Bottom);
                    players[1] = new Player(Player.TablePosition.Left);
                    players[2] = new Player(Player.TablePosition.Top);
                }
                else if (order == 2)
                {
                    players[0] = new Player(Player.TablePosition.Top);
                    players[1] = new Player(Player.TablePosition.Bottom);
                    players[2] = new Player(Player.TablePosition.Left);
                }
                else
                {
                    players[0] = new Player(Player.TablePosition.Left);
                    players[1] = new Player(Player.TablePosition.Top);
                    players[2] = new Player(Player.TablePosition.Bottom);
                }
            }
            else
            {
                if (order == 1)
                {
                    players[0] = new Player(Player.TablePosition.Bottom);
                    players[1] = new Player(Player.TablePosition.Left);
                    players[2] = new Player(Player.TablePosition.Top);
                    players[3] = new Player(Player.TablePosition.Right);
                }
                else if (order == 2)
                {
                    players[0] = new Player(Player.TablePosition.Right);
                    players[1] = new Player(Player.TablePosition.Bottom);
                    players[2] = new Player(Player.TablePosition.Left);
                    players[3] = new Player(Player.TablePosition.Top);
                }
                else if (order == 3)
                {
                    players[0] = new Player(Player.TablePosition.Top);
                    players[1] = new Player(Player.TablePosition.Right);
                    players[2] = new Player(Player.TablePosition.Bottom);
                    players[3] = new Player(Player.TablePosition.Left);
                }
                else
                {
                    players[0] = new Player(Player.TablePosition.Left);
                    players[1] = new Player(Player.TablePosition.Top);
                    players[2] = new Player(Player.TablePosition.Right);
                    players[3] = new Player(Player.TablePosition.Bottom);
                }
            }
            OrderNumber = order;
        }

        internal void PrepareForNewRound()
        {
            chain = new LinkedList<PictureBox>();
            Round += 1;
            isFirstTurn = true;
        }
    }
}
