namespace Hive
{
    public class Ant : Piece
    {
        public static HashSet<Cell> findAll(Board board, Cell origin, HashSet<Cell> excluded)
        {
            excluded.Add(origin);
            var adj = hypotheticalAdjacentLegalCellsForAnts(board, origin, excluded.ToList()).Where(item => !excluded.Contains(item)); ;
            if (adj.All(excluded.Contains)) return excluded;
            foreach (var p in adj)
            {
                excluded.UnionWith(findAll(board, p, excluded));
            }
            return excluded;
        }
        public static List<Path> _getLegalMoves(Board board, Cell origin)
        {
            return findAll(board, origin, new HashSet<Cell>()).ToList().Select(x => new Path(x, Pieces.ANT)).ToList();
        }
        public Ant(Players p, Cell l, int id) : base(Pieces.ANT, p, l, id) { }

        private static bool hypotheticallCanMoveBetweenForAnts(Board ctx, Cell a, Cell b, List<Cell> exclude)
        {
            List<Cell> adjacents = ctx.connectingAdjacents(a, b);
            if (adjacents.Intersect(exclude).Count() > 0) return true;
            else return !adjacents.All(cell => ctx.tileIsOccupied(cell));
        }
        private static List<Cell> hypotheticalAdjacentLegalCellsForAnts(Board ctx, Cell cell, List<Cell> exclude)
        {
            List<Cell> empty = ctx.getEmptyNeighbors(cell);
            List<Cell> neighbors = ctx.getOccupiedNeighbors(cell);
            foreach (Cell toExclude in exclude)
            {
                neighbors.Remove(toExclude);
            }
            HashSet<Cell> neighbor_adjacent = new HashSet<Cell>();
            foreach (Cell neighbor in neighbors)
            {
                List<Cell> neighborAdjacentEmpties = ctx.getEmptyNeighbors(neighbor);
                neighborAdjacentEmpties.ForEach(temp_tile => neighbor_adjacent.Add(temp_tile));
            }
            var prelim = empty.Intersect(neighbor_adjacent).Where(next => hypotheticallCanMoveBetweenForAnts(ctx, cell, next, exclude)).ToList();
            return prelim.ToList();
        }
    }
}
