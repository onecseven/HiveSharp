namespace Hive
{
    public class Beetle : Piece
    {
        public static List<Path> _getLegalMoves(Board board, Cell origin)
        {
            Tile piece = board.tiles[origin];
            bool isAbove = piece.hasBlockedPiece && piece.activePiece.type is Pieces.BEETLE;
            List<Cell> result = new List<Cell>();
            //FIXME need to account for times when you need to move down. skip origin, cell when cell is empty and beetle is above another piece
            result.AddRange(board.adjacentLegalCells(origin).Where(cell => {
                if (isAbove) return true;
                else return board.CanMoveBetween(origin, cell);
            }));
            result.AddRange(board.getOccupiedNeighbors(origin));
            return result.ConvertAll<Path>(cell => new Path(cell, Pieces.BEE));
        }
        public Beetle(Players p, Cell l, int id) : base(Pieces.BEETLE, p, l, id) { }
    }
}
