namespace Hive
{
    public partial class Hive
    {
        public bool wincon_check()
        {
            List<Piece> bees = board.tiles.Where(kvp => kvp.Value.isOccupied && kvp.Value.pieces.Any(pie => pie.type == Pieces.BEE)).Select(kvp => kvp.Value.activePiece).ToList();
            if (bees.Any(piece => board.getOccupiedNeighbors(piece.location).Count == 6)) return true;
            return false;
        }
    }
}
