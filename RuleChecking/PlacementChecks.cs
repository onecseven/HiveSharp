namespace Hive
{
    public partial class Hive
    {
        bool placementLegalityCheck(Cell destination, Players player, Pieces piece)
        {
            bool checkIfPlayerHasPieceInInventory(Players player, Pieces piece) => players[player].hasPiece(piece);
            List<Cell> surroundingPieces = board.getOccupiedNeighbors(destination);
            List<Piece> actualPieces = surroundingPieces.Select(cel => board.tiles[cel].activePiece).ToList();
            bool piecesBelongToMover = actualPieces.All(pieces => player == pieces.owner);
            bool playerHasPieceInHand = checkIfPlayerHasPieceInInventory(player, piece);
            if (actualPieces.Count > 0 && piecesBelongToMover && playerHasPieceInHand) return true;
            return false;
        }
        bool initialPlacementCheck(INITIAL_PLACE move)
        {
            if (moves.Count == 0) return true;
            else if (moves.Count == 1)
            {
                bool isCorrectUser = moves[0].player != move.player;
                bool isAdjacentToFirstPiece = board.AreCellsAdjacent(((INITIAL_PLACE)moves[0]).destination, move.destination);
                if (isCorrectUser && isAdjacentToFirstPiece) return true;
            }
            return false;
        }
    }
}