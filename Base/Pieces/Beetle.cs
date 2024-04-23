namespace Hive
{
    public class Beetle : Piece
    {
        Piece blockedPiece = null;
        public static List<Path> _getLegalMoves(Board board, Cell origin)
        {
            List<Cell> result = new List<Cell>();
            result.AddRange(board.adjacentLegalCells(origin));
            result.AddRange(board.getOccupiedNeighbors(origin));
            return result.ConvertAll<Path>(cell => new Path(cell, Pieces.BEE));
        }
        public Beetle(Players p, Cell l, int id) : base(Pieces.BEETLE, p, l, id) { }
    }
}
