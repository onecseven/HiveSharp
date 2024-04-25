namespace Hive
{
    public partial class Hive
    {
        private HashSet<Cell> recursiveGetNeighbors(Cell cell, HashSet<Cell> memo, Cell? excludedCell)
        {
            List<Cell> neighbors = board.getOccupiedNeighbors(cell);
            if (excludedCell.HasValue) neighbors.Remove(excludedCell.Value);
            if (neighbors.Count == 0 || neighbors.All(item => memo.Contains(item))) return memo;
            foreach (Cell neighbor in neighbors)
            {
                memo.Add(neighbor);
            }
            foreach (Cell neighbor in neighbors)
            {
                memo = recursiveGetNeighbors(neighbor, memo, excludedCell);
            }
            return memo;
        }
        private bool oneHiveRuleCheck(Cell movingPiece)
        {
            HashSet<Cell> hypoHive = new HashSet<Cell>();
            List<Cell> prelim = board.filteredPiecesInPlay;
            //FIXME account for empty boards
            Tile tile = board.tiles[movingPiece];
            if (!tile.hasBlockedPiece)
            {
                prelim.Remove(movingPiece);
            }
            Cell first = prelim.First();
            int target = prelim.Count;
            HashSet<Cell> computed = recursiveGetNeighbors(first, hypoHive, tile.hasBlockedPiece ? null : movingPiece);
            if (computed.Count != target) return false;
            return true;
        }
    }
}
