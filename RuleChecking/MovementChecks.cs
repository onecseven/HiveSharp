namespace Hive
{
    public partial class Hive
    {
        bool moveIsLegal(MOVE_PIECE move)
        {
            // check that the piece is in play
            if ((!board.tiles[move.origin].isOccupied && move.piece != Pieces.BEETLE) || board.tiles[move.origin].activePiece.owner != move.player) return false;

            Piece piece = board.tiles[move.origin].activePiece;
            List<Path> paths = Piece.getLegalMoves(piece, board);
            Path playerPath = paths[paths.FindIndex(path => path.last == move.destination)];
            List<Cell> endpoints = paths.Select(path => path.last).ToList();

            if (endpoints.Contains(move.destination) && pathIsLegal(playerPath, piece.type, move)) return true;
            else return false;
        }
        bool pathIsLegal(Path path, Pieces pieceType, MOVE_PIECE move)
        {
            switch (pieceType)
            {
                case Pieces.ANT:
                    return board.getOccupiedNeighbors(path.last).Count < 5;
                case Pieces.SPIDER: //already checked in moveIsLegal by Piece.getLegalMoves()
                    return true;
                case Pieces.BEE:
                    foreach ((Cell first, Cell last) in path.pairs)
                    {
                        if (!board.CanMoveBetween(first, last)) return false;
                    }
                    break;
                case Pieces.LADYBUG:
                    if (path.isNullPath) return false;
                    if (!board.CanMoveAboveHive(path.steps[0], path.steps[1])) return false;
                    break;
                case Pieces.GRASSHOPPER: //already checked in moveIsLegal
                    return true;
                case Pieces.MOSQUITO:
                    return pathIsLegal(path, path.pathType, move);
                case Pieces.BEETLE:
                    var originTile = board.tiles[move.origin];
                    var destTile = board.tiles[path.last];
                    // horizontal movement above hive
                    if ((originTile.zIndex - 1) == destTile.zIndex && originTile.zIndex > 0)
                    {
                        if (!board.CanMoveAboveHive(originTile.cell, destTile.cell)) return false;
                        // horizontal movement on the floor
                    }
                    else if (!originTile.hasBlockedPiece && !destTile.isOccupied)
                    {
                        if (!board.CanMoveBetween(originTile.cell, destTile.cell)) return false;
                    }
                    else if (originTile.zIndex != destTile.zIndex)
                    {
                        return true;
                    }
                    break;
            }
            return true;
        }
    }
}
