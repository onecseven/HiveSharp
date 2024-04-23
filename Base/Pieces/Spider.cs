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

                List<Cell> secondStep = board.hypotheticalAdjacentLegalCells(firstStep, origin);
                secondStep.Remove(origin);

                foreach (Cell seStep in secondStep)
                {
                    List<Cell> lastStep = board.hypotheticalAdjacentLegalCells(seStep, origin);
                    lastStep.Remove(firstStep);
                    lastStep.ForEach(last => paths.Add(new Path(new List<Cell>() { firstStep, seStep, last }, Pieces.SPIDER)));
                }
            }
            return paths;
        }
        public Spider(Players p, Cell l, int id) : base(Pieces.SPIDER, p, l, id) { }
    }
}
