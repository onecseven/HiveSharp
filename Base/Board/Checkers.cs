namespace Hive
{
    public partial class Board
    {
        public bool AreCellsAdjacent(Cell a, Cell B) => HiveUtils.getNeighbors(a).Contains(B);
        public bool CanMoveAboveHive(Cell a, Cell b)
        {
            List<Cell> adjacents = connectingAdjacents(a, b);
            return !adjacents.All(cell => tileIsOccupied(cell) && tiles[cell].hasBlockedPiece);
        }
        public bool CanMoveBetween(Cell a, Cell b) => !connectingAdjacents(a, b).All(cell => tileIsOccupied(cell));
        public bool tileIsOccupied(Cell cell) => tiles.ContainsKey(cell) && tiles[cell].isOccupied;
    }
}
