using System;
using System.Collections.Generic;
using System.Drawing;

namespace DominoClient
{
    class Player
    {
        public int Points { get; set; } = 0;

        public TablePosition TablePos { get; set; }

        public Domino[] tilesArr;

        public enum TablePosition
        {
            Left,
            Top,
            Right,
            Bottom
        }

        public Player(TablePosition position)
        {
            TablePos = position;
        }

        internal void RefreshTilesArr(string line)
        {
            if (line == null || line.Length == 0)
            {
                tilesArr = Array.Empty<Domino>();
                return;
            }
            string[] tiles = line.Split(" ");
            tilesArr = new Domino[tiles.Length];
            for (int i = 0; i < tiles.Length; i++)
            {
                var tile = tiles[i];
                tilesArr[i] = new Domino(Int32.Parse(tile[0].ToString()), Int32.Parse(tile[^1].ToString()));
            }
        }

        public static Dictionary<TablePosition, Point> coordinates = new()
        {
            { TablePosition.Bottom, new Point(-1, 945) },
            { TablePosition.Left, new Point(10, -1) },
            { TablePosition.Top, new Point(-1, 10) },
            { TablePosition.Right, new Point(10, -1) }
        };
    }
}
