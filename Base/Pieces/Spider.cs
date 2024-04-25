namespace Hive
{
    public class Spider : Piece
    {
        public static List<Path> _getLegalMoves(Board board, Cell origin)
        {
            List<Cell> pathOrigins = board.adjacentLegalCells(origin);
            List<Path> paths = new();
            List<Cell> endpoints = new();
            foreach (Cell firstStep in pathOrigins)
            {

                List<Cell> secondStep = hypotheticalAdjacentLegalCells(firstStep, origin, board);
                secondStep.Remove(origin);

                foreach (Cell seStep in secondStep)
                {
                    List<Cell> lastStep = hypotheticalAdjacentLegalCells(seStep, origin, board);
                    lastStep.Remove(firstStep);
                    lastStep.ForEach(last => paths.Add(new Path(new List<Cell>() { firstStep, seStep, last }, Pieces.SPIDER)));
                }
            }
            return paths;
        }
        private static List<Cell> hypotheticalAdjacentLegalCells(Cell cell, Cell exclude, Board ctx)
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
            var prelim = empty.Intersect(neighbor_adjacent).Where(next => ctx.hypotheticallCanMoveBetween(cell, next, exclude)).ToList();
            return prelim.ToList();
        }
        public Spider(Players p, Cell l, int id) : base(Pieces.SPIDER, p, l, id) { }
    }
}
