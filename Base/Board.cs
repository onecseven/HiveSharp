namespace Hive
{
    public partial class Board
    {
        public Dictionary<Cell, Tile> tiles = new Dictionary<Cell, Tile>();
        public List<Cell> filteredPiecesInPlay { get { return tiles.Where(kvp => kvp.Value.isOccupied).ToList().Select(KeyValuePair => KeyValuePair.Key).ToList(); } }
        public Board()
        {
            //HACK it occurs to me that it might not be necessary to create all these empty tiles that will never be used, but on the other hand
            //it does save us from the constant tiles.Contains[cell]
            //but it does mean whenever we want to query tiles we basically have to go through 1000~ empty pieces each time...
            //then again, that seems like an infinitesimal amount to any processor from the last two decades
            foreach (Cell cell in HexUtils.HexGen(36, 24, HexOrientation.PointyTopped))
            {
                tiles.Add(cell, new Tile(cell));
            }
        }
        public void movePiece(Cell origin, Piece piece)
        {
            tiles[origin].removePiece();
            tiles[piece.location].addPiece(piece);
        }
        public void placePiece(Piece pieceToPlace) => tiles[pieceToPlace.location].addPiece(pieceToPlace);

    }
}
