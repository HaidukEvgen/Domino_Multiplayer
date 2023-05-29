using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DominoServer
{
    public class DominoPlayer
    {
        public int Points { get; set; } = 0;
        public int CurTilesAmount { get; set; }

        public Domino[] tilesArr = new Domino[DominoGame.MAX_AMOUNT];
        
        public void SetTiles(Domino[] tiles)
        {
            CurTilesAmount = 0;
            for (int i = 0; i < DominoGame.INITIAL_AMOUNT; i++)
            {
                AddTile(tiles[i]);
            }
        }

        public void AddTile(Domino curTile) => tilesArr[CurTilesAmount++] = new Domino(curTile);

        public void DeleteTile(int index)
        {
            for (int i = index; i < CurTilesAmount; i++)
            {
                tilesArr[i] = tilesArr[i + 1];
            }
            CurTilesAmount--;
        }

        public int GetTilesSum()
        {
            int sum = 0;
            foreach (var tile in tilesArr)
            {
                if (tile == null) break;
                sum += tile.FirstNum + tile.SecondNum;
            }
            return sum;
        }
    }
}
