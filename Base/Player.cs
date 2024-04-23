using System;
using System.Collections.Generic;

namespace Hive
{
    public enum Players
    {
        BLACK,
        WHITE
    }
    public class Player
    {
        private readonly Dictionary<Pieces, int> reference = new Dictionary<Pieces, int>()
        {
            [Pieces.BEE] = 1,
            [Pieces.MOSQUITO] = 1,
            [Pieces.LADYBUG] = 1,
            [Pieces.SPIDER] = 2,
            [Pieces.BEETLE] = 2,
            [Pieces.ANT] = 3,
            [Pieces.GRASSHOPPER] = 3

        };
        public Dictionary<Pieces, int> inventory = new Dictionary<Pieces, int>()
        {
            [Pieces.BEE] = 1,
            [Pieces.MOSQUITO] = 1,
            [Pieces.LADYBUG] = 1,
            [Pieces.SPIDER] = 2,
            [Pieces.BEETLE] = 2,
            [Pieces.ANT] = 3,
            [Pieces.GRASSHOPPER] = 3

        };
        public Players color;

        public Player(Players _color)
        {
            color = _color;
        }

        public int piecePlaced(Pieces _piece)
        {
            if (hasPiece(_piece)) inventory[_piece]--;
            else throw new ArgumentOutOfRangeException("piece placed called when player is out of pieces");
            return reference[_piece] - inventory[_piece];
        }
        public bool hasPiece(Pieces _piece) => inventory.ContainsKey(_piece) && inventory[_piece] > 0;

    }
}