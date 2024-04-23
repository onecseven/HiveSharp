using System.Text.RegularExpressions;
using Hive;
using static Hive.Notation.Tokenizer;
using static Hive.Notation.Parser;
using static Hive.Notation.Shared;

namespace Hive
{
    namespace Notation
    {
        public partial class Reader
        {
            internal static List<Move> Translator(List<(Subject, Objet)> moves)
            {
                if (moves.Count == 0) return null;
                //HashSet<string> seen = new HashSet<string>();
                //seen.Add(first.subject.ToNotation());
                //seen.Add(second.subject.ToNotation());

                List<Move> processed = new List<Move>();
                Cell center = new Cell(4, 5, -9);

                Dictionary<string, Cell> pieceTracker = new Dictionary<string, Cell>();
                List<(Subject, Objet)> iterateMoves = moves.Skip(2).ToList();

                //initial moves

                (Subject subject, Objet objet) first = moves[0];
                (Subject subject, Objet objet) second = moves[1];
                Cell secondPieceLoc = getCellFromDirection(center, second.objet.positionalMarker);
                pieceTracker.Add(first.subject.ToNotation(), center);
                pieceTracker.Add(second.subject.ToNotation(), secondPieceLoc);
                processed.Add(new INITIAL_PLACE(first.subject.playerMarker, first.subject.pieceMarker, center));
                processed.Add(new INITIAL_PLACE(second.subject.playerMarker, second.subject.pieceMarker, secondPieceLoc));

                //rest of moves
                foreach ((Subject, Objet) move in iterateMoves)
                {
                    Subject subj = move.Item1;
                    Objet objet = move.Item2;
                    bool alreadyExists = pieceTracker.ContainsKey(subj.ToNotation());
                    // Gotta use ((Subject)objet).ToNotation() because if we use the Objet version of ToNotation() we also add the positional marker
                    // objet.onTop marks that the notation has no positional marker and is therefore on top of the specified piece
                    // if it's true, we skip the call to getCellFromDirection
                    Cell dest = objet.onTop ? pieceTracker[((Subject)objet).ToNotation()] : getCellFromDirection(pieceTracker[((Subject)objet).ToNotation()], objet.positionalMarker);
                    switch (alreadyExists)
                    {
                        case true:
                            processed.Add(new MOVE_PIECE(subj.playerMarker, subj.pieceMarker, pieceTracker[subj.ToNotation()], dest));
                            pieceTracker[subj.ToNotation()] = dest;
                            break;
                        case false:
                            processed.Add(new PLACE(subj.playerMarker, subj.pieceMarker, dest));
                            pieceTracker.Add(subj.ToNotation(), dest);
                            break;
                    }
                }
                return processed;
            }
            public static List<Move> formattedListToMoves(string moveList)
            {
                if (Validator.IsValidFormattedMoveList(moveList)) return Translator(Parser.Parse(Tokenizer.TokenizeFormatted(moveList)));
                throw new Exception("Invalid movelist.");
            }
            public static Move translateUnformattedMove(string move, Board context)
            {
                bool samePiece(Subject a, Piece b) => a.playerMarker == b.owner && a.pieceMarker == b.type && a.numMarker == b.id;
                var tokenized = TokenizeMoveRaw(move);
                var parsed = (parseUnformatted(tokenized[0], true), parseUnformatted(tokenized[1], false));
                Subject subj = parsed.Item1;
                Objet objet = (Objet)parsed.Item2;
                if (objet is NullObjet)
                {
                    return new INITIAL_PLACE(subj.playerMarker, subj.pieceMarker, new Cell(4, 5, -9));
                }
                List<Tile> pieces = context.filteredPiecesInPlay
                    .Select(cell => context.tiles[cell]).ToList();
                bool alreadyExists = pieces
                    .Any(tile => samePiece(subj, tile.activePiece));
                Tile destination = pieces.Find(tile => samePiece(objet, tile.activePiece))!;
                if (destination != null)
                {
                    Cell shifted = getCellFromDirection(destination.cell, objet.positionalMarker);
                    if (context.filteredPiecesInPlay.Count == 1)
                    {
                        return new INITIAL_PLACE(subj.playerMarker, subj.pieceMarker, shifted);
                    }
                    switch (alreadyExists)
                    {
                        case true:
                            Tile origin = pieces.Find(tile => samePiece(subj, tile.activePiece))!;
                            return new MOVE_PIECE(subj.playerMarker, subj.pieceMarker, origin.cell, objet.onTop ? destination.cell : shifted);
                        case false:
                            return new PLACE(subj.playerMarker, subj.pieceMarker, shifted);
                    }
                }
                throw new Exception($"your move was bad: {move}");
            }
        }
    }
}
