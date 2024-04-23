using Hive;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hive
{
    public partial class Hive
    {
        public void autopassCheck()
        {

            if ((playerHasPiecesInPlay(turn) && hasLegalPlacementTarget(turn)) || playerHasPossibleMoves(turn)) return;
            else
            {
                if (moves.Last().type == MoveType.AUTOPASS)
                {
                    throw new Exception("AUTOPASS LOOP FUCK");
                }
                send_move(new AUTOPASS(turn));
            };

        }
        bool hasLegalPlacementTarget(Players player)
        {
            if (moves.Count <= 2) return true;
            List<Piece> playerPieces = board.tiles.Where(kvp => kvp.Value.isOccupied && kvp.Value.activePiece.owner == player).Select(kvp => kvp.Value.activePiece).ToList();
            foreach (Piece item in playerPieces)
            {
                List<Cell> neighbors = board.getEmptyNeighbors(item.location);
                if (neighbors.Any(neigh => placementLegalityCheck(neigh, player, item.type))) return true;
            }
            return false;
        }
        bool playerHasPiecesInPlay(Players player) => board.tiles.Where(kvp => kvp.Value.isOccupied && kvp.Value.activePiece.owner == player).ToList().Count < 13;
        bool playerHasPossibleMoves(Players player)
        {
            List<Piece> playerPieces = board.tiles.Where(kvp => kvp.Value.isOccupied && kvp.Value.activePiece.owner == player).Select(kvp => kvp.Value.activePiece).ToList();
            List<Path> possibleMoves = new List<Path>();
            foreach (Piece piece in playerPieces)
            {
                possibleMoves.AddRange(Piece.getLegalMoves(piece, board));
            }
            if (possibleMoves.Count == 0 || possibleMoves.All(path => path.isNullPath)) return false;
            else return true;
        }
    }
}
