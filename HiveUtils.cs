using System.Numerics;

namespace Hive
{
public class HiveUtils
{
    public static bool AreCellsAdjacent(Cell a, Cell B) => HiveUtils.getNeighbors(a).Contains(B);

    public static Dictionary<FTHexCorner, Cell> corners = new Dictionary<FTHexCorner, Cell>() {
        [FTHexCorner.Right] = new Cell(+1, 0, -1),
        [FTHexCorner.UpRight] = new Cell(+1,-1,0),
        [FTHexCorner.DownRight] = new Cell(0, +1, -1),
        [FTHexCorner.Left] = new Cell(-1, 0, +1),
        [FTHexCorner.UpLeft] = new Cell(0, -1, +1),
        [FTHexCorner.DownLeft] = new Cell(-1, +1, 0),
    };

    public static Cell[] directions = new Cell[6] {
            new Cell(+1, 0, -1), new Cell(+1, -1, 0), new Cell(0, -1, +1),
            new Cell(-1, 0, +1), new Cell(-1, +1, 0), new Cell(0, +1, -1),
        };
    public static List<Cell> getNeighbors(Cell cell) => directions.Select(direction => new Cell(cell.x + direction.x, cell.y + direction.y, cell.z + direction.z)).ToList();
    public static Vector2 Vector3ToVector2 (Vector3 vector) => new Vector2(vector.X, vector.Y);

    public static Vector3 cel2vec(Cell cell) => new Vector3(cell.x,cell.y,cell.z);
    public static Cell vec2cel(Vector3 vector) => new Cell(System.Convert.ToInt32(vector.X), System.Convert.ToInt32(vector.Y), System.Convert.ToInt32(vector.Z));
    }
}