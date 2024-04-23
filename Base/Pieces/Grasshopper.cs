using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hive
{
    public class Grasshopper : Piece
    {
        private static Path traverseDirection(Cell startPoint, Cell direction, Board board)
        {
            List<Cell> result = new List<Cell>() { startPoint };
            Cell current = startPoint;
            bool toContinue = true;
            while (toContinue == true)
            {
                var newOne = new Cell(current.x + direction.x, current.y + direction.y, current.z + direction.z);
                if (board.tileIsOccupied(newOne))
                {
                    current = newOne;
                    result.Add(newOne);
                    continue;
                }
                else
                {
                    result.Add(newOne);
                    return new Path(result, Pieces.GRASSHOPPER);
                }
            }
            throw new Exception("huh");
        }
        public static List<Path> _getLegalMoves(Board board, Cell origin)
        {
            List<Path> result = new List<Path>();
            foreach (Cell direction in HexUtils.directions)
            {
                Cell current = new Cell(origin.x + direction.x, origin.y + direction.y, origin.z + direction.z);
                if (board.tileIsOccupied(current)) result.Add(traverseDirection(current, direction, board));
            }
            return result;
        }
        public Grasshopper(Players p, Cell l, int id) : base(Pieces.GRASSHOPPER, p, l, id) { }
    }

}
