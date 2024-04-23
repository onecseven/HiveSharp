namespace Hive
{
    public class Ant : Piece
    {
        public static HashSet<Cell> findAll(Board board, Cell origin, HashSet<Cell> excluded)
        {
            excluded.Add(origin);
            var adj = board.hypotheticalAdjacentLegalCellsForAnts(origin, excluded.ToList()).Where(item => !excluded.Contains(item)); ;
            if (adj.All(item => excluded.Contains(item))) return excluded;
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
    }
}
