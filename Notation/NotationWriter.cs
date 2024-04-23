using static Hive.Notation.Tokenizer;
using static Hive.Notation.Parser;
using static Hive.Notation.Shared;

namespace Hive
{
    namespace Notation {
    public static class Writer
    {
        private static string toNotationSubject(Pieces piece, Players player, int id = 0) => $"{(player == Players.WHITE ? 'w' : 'b')}{reversedPieceDict[piece]}{(id == 0 ? "" : id)}";
        private static string toNotationObject(Cell location, Tile reference, bool isBeetleMove)
        {
            string prelim = toNotationSubject(reference.activePiece.type, reference.activePiece.owner, reference.activePiece.id);
            if (isBeetleMove) return prelim;
            foreach (FTHexCorner hexCorner in HexUtils.corners.Keys)
            {
                Cell toCheck = getCellFromDirection(location, hexCorner);
                if (reference.cell == toCheck)
                {
                    switch (hexCorner)
                    {
                        case FTHexCorner.Right:
                            //it's to the left of the other guy
                            return $"-{prelim}";
                        case FTHexCorner.Left:
                            //fixed
                            return $"{prelim}-";
                        case FTHexCorner.UpRight:
                            //fixed
                            return $"/{prelim}";
                        case FTHexCorner.DownRight:
                            //fixed
                            return $"\\{prelim}";
                        case FTHexCorner.DownLeft:
                            //fixed
                            return $"{prelim}/";
                        case FTHexCorner.UpLeft:
                            //fixed
                            return $"{prelim}\\";
                    }
                }
            }
            throw new Exception("weird happening");
        }
        public static List<string> moveListToNotation(List<Move> moves) 
        {
                #region declarations

                Dictionary<Pieces, int> wInventory = new Dictionary<Pieces, int>()
            {
                [Pieces.ANT] = 3,
                [Pieces.SPIDER] = 2,
                [Pieces.BEETLE] = 2,
                [Pieces.GRASSHOPPER] = 3,
            };
            Dictionary<Pieces, int> bInventory = new Dictionary<Pieces, int>()
            {
                [Pieces.ANT] = 3,
                [Pieces.SPIDER] = 2,
                [Pieces.BEETLE] = 2,
                [Pieces.GRASSHOPPER] = 3,
            };
            Dictionary<Players, Dictionary<Pieces, int>> inventories = new Dictionary<Players, Dictionary<Pieces, int>>()
            {
                [Players.BLACK] = bInventory,
                [Players.WHITE] = wInventory,
            };
            Dictionary<Cell, List<string>> map = new Dictionary<Cell, List<string>>() { };
            List<string> converted = new List<string>();

                #endregion

                #region helper functions
                // helper functions
                string Sujeto(Pieces piece, Players player) => $"{(player == Players.WHITE ? 'w' : 'b')}{reversedPieceDict[piece]}{(inventories[player].ContainsKey(piece) ? reference[piece] - inventories[player][piece] : "")}";
            string placeSubject(Pieces piece, Players player, Cell destination)
            {
                if (piece != Pieces.BEE && piece != Pieces.MOSQUITO && piece != Pieces.LADYBUG)
                {
                    inventories[player][piece]--;
                }
                string subject = Sujeto(piece, player);
                map.Add(destination, new List<string>() { subject });
                return subject;
            }
            string Objeto(Cell destination)
            {
                foreach (FTHexCorner hexCorner in HexUtils.corners.Keys)
                {
                    Cell toCheck = getCellFromDirection(destination, hexCorner);
                    if (map.ContainsKey(toCheck))
                    {
                        string prelim = map[toCheck][0];
                        switch (hexCorner)
                        {
                            case FTHexCorner.Right:
                                //it's to the left of the other guy
                                return $"-{prelim}";
                            case FTHexCorner.Left:
                                //fixed
                                return $"{prelim}-";
                            case FTHexCorner.UpRight:
                                //fixed
                                return $"/{prelim}";
                            case FTHexCorner.DownRight:
                                //fixed
                                return $"\\{prelim}";
                            case FTHexCorner.DownLeft:
                                //fixed
                                return $"{prelim}/";
                            case FTHexCorner.UpLeft:
                                //fixed
                                return $"{prelim}\\";
                        }
                    }
                }
                throw new Exception("Couldn't find appropiate reference for the Objet part.");
            }

            string initialToNotation(INITIAL_PLACE move)
            {
                switch (move.player)
                {
                    case Players.WHITE:
                        return $"{placeSubject(move.piece, move.player, move.destination)} .";
                    case Players.BLACK:
                        return $"{placeSubject(move.piece, move.player, move.destination)} {Objeto(move.destination)}";
                    default:
                        throw new Exception("shut up type guy");
                }

            }
            string placeToNotation(PLACE move) => $"{placeSubject(move.piece, move.player, move.destination)} {Objeto(move.destination)}";
            string moveToNotation(MOVE_PIECE move)
            {
                string subj = map[move.origin][0];
                if (map[move.origin].Count > 1)
                {
                    map[move.origin].RemoveAt(0);
                }
                else
                {
                    map.Remove(move.origin);
                }
                string prelim = $"{subj} {Objeto(move.destination)}";
                if (move.piece == Pieces.BEETLE && map.ContainsKey(move.destination))
                {
                    prelim = $"{subj} {map[move.destination].Last()}";
                }
                if (map.ContainsKey(move.destination))
                {
                    map[move.destination].Insert(0, subj);
                }
                else
                {
                    map.Add(move.destination, new List<string>() { subj });
                }
                return prelim;
            }
                #endregion

                //mainloop
                foreach (Move move in moves)
            {
                switch (move.type)
                {
                    case MoveType.INITIAL_PLACE:
                        converted.Add(initialToNotation((INITIAL_PLACE)move));
                        break;
                    case MoveType.PLACE:
                        converted.Add(placeToNotation((PLACE)move));
                        break;
                    case MoveType.MOVE_PIECE:
                        converted.Add(moveToNotation((MOVE_PIECE)move));
                        break;
                    case MoveType.AUTOPASS:
                        converted.Add("pass");
                        break;
                    default:
                        break;
                }
            }
            return converted;
        }
    }
    }
}
