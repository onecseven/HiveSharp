using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hive
{
    public class Bee : Piece
    {
        public Bee(Players p, Cell l) : base(Pieces.BEE, p, l, 0) { }
        public static List<Path> _getLegalMoves(Board board, Cell origin) => board.adjacentLegalCells(origin).Select(cell => new Path(cell, Pieces.BEE)).ToList();
    }
}
