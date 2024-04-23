using System.Numerics;

namespace Hive
{
public class HiveUtils
{
    public static Cell[] directions = new Cell[6] {
            new Cell(+1, 0, -1), new Cell(+1, -1, 0), new Cell(0, -1, +1),
            new Cell(-1, 0, +1), new Cell(-1, +1, 0), new Cell(0, +1, -1),
        };
    public static List<Cell> getNeighbors(Cell cell) => directions.Select(direction => new Cell(cell.x + direction.x, cell.y + direction.y, cell.z + direction.z)).ToList();
    //FIXME delete the following?
    public static Dictionary<FTHexCorner, Cell> corners = new Dictionary<FTHexCorner, Cell>() {
        [FTHexCorner.Right] = new Cell(+1, 0, -1),
        [FTHexCorner.UpRight] = new Cell(+1,-1,0),
        [FTHexCorner.DownRight] = new Cell(0, +1, -1),
        [FTHexCorner.Left] = new Cell(-1, 0, +1),
        [FTHexCorner.UpLeft] = new Cell(0, -1, +1),
        [FTHexCorner.DownLeft] = new Cell(-1, +1, 0),
    };
    }
}