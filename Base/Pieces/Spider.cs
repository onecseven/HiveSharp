namespace Hive
{
    public class Spider : Piece
    {
        
        public Spider(Players p, Cell l, int id) : base(Pieces.SPIDER, p, l, id) { }
        public static List<Path> _getLegalMoves(Board board, Cell origin)
        {
            List<Cell> pathOrigins = board.adjacentLegalCells(origin);
            List<Path> paths = new();
            List<Cell> endpoints = new();
            foreach (Cell firstStep in pathOrigins)
            {

                List<Cell> secondStep = hypotheticalAdjacentLegalCells(board, firstStep, origin);
                secondStep.Remove(origin);

                foreach (Cell seStep in secondStep)
                {
                    List<Cell> lastStep = hypotheticalAdjacentLegalCells(board, seStep, origin);
                    lastStep.Remove(firstStep);
                    lastStep.ForEach(last => paths.Add(new Path(new List<Cell>() { firstStep, seStep, last }, Pieces.SPIDER)));
                }
            }
            return paths;
        }
        private static List<Cell> hypotheticalAdjacentLegalCells(Board ctx, Cell cell, Cell exclude)
        {
            List<Cell> empty = ctx.getEmptyNeighbors(cell);
            List<Cell> neighbors = ctx.getOccupiedNeighbors(cell);
            neighbors.Remove(exclude);
            HashSet<Cell> neighbor_adjacent = new HashSet<Cell>();
            foreach (Cell neighbor in neighbors)
            {
                List<Cell> neighborAdjacentEmpties = ctx.getEmptyNeighbors(neighbor);
                neighborAdjacentEmpties.ForEach(temp_tile => neighbor_adjacent.Add(temp_tile));
            }
            var prelim = empty.Intersect(neighbor_adjacent).Where(next => hypotheticallCanMoveBetween(ctx, cell, next, exclude)).ToList();
            return prelim.ToList();
        }

        private static bool hypotheticallCanMoveBetween(Board ctx, Cell a, Cell b, Cell exclude)
        {
            List<Cell> adjacents = ctx.connectingAdjacents(a, b);
            if (adjacents.Contains(exclude)) return true;
            else return !adjacents.All(cell => ctx.tileIsOccupied(cell));
        }
    }
}
