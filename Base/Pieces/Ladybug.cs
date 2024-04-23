using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hive
{
    public class Ladybug : Piece
    {
        public static List<Path> _getLegalMoves(Board board, Cell origin)
        {
            List<Cell> pathOrigins = board.getOccupiedNeighbors(origin);
            List<Path> paths = new();
            foreach (Cell firstStep in pathOrigins)
            {

                List<Cell> secondStep = board.getOccupiedNeighbors(firstStep);
                secondStep.Remove(origin);

                foreach (Cell seStep in secondStep)
                {
                    List<Cell> lastStep = board.getEmptyNeighbors(seStep);
                    lastStep.ForEach(last => paths.Add(new Path(new List<Cell>() { firstStep, seStep, last }, Pieces.LADYBUG)));
                }
            }
            return paths;
        }
        public Ladybug(Players p, Cell l) : base(Pieces.LADYBUG, p, l, 0) { }
    }

}
