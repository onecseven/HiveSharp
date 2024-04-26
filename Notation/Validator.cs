using static Hive.Notation.Tokenizer;
using static Hive.Notation.Parser;
using static Hive.Notation.Shared;
namespace Hive
{
    namespace Notation
    {
        public static class Validator
        {
            public static bool IsValidFormattedMoveList(string moveList)
            {
                List<string> lines = moveList.Split("\n").ToList<string>().Where(str => str.Count() > 0).ToList();
                foreach (string line in lines)
                {
                    if (!IsValidMoveListToken(line)) return false;
                }
                return true;
            }
            
            public static bool isValidRawMove(string move) => rawMoveTemplate.Match(move).Success;
            public static bool isValidUnformattedMoveList(string moveList) =>  moveList
                                                                            .Split("\n")
                                                                            .ToList<string>()
                                                                            .Where(str => str.Count() > 0)
                                                                            .All(isValidRawMove);

            internal static bool IsValidMoveListToken(string line)
            {
                var a = line.Split(":").ToList();
                if (a.Count < 1) return false;
                var designator = a[0];
                if (designator == "URL") return true;
                else if (int.TryParse(designator, out int _))
                {
                    var moves = a[1].Split(" ").ToList<string>().Where(str => str.Count() > 0).ToList();
                    if (moves.Count != 2) return false;
                    if (!isValidMove(moves)) return false;
                }
                return true;
            }
            internal static bool isValidMove(List<string> moves)
            {
                if (moves.Count != 2) return false; //redundant but you never know
                string subject = moves[0];
                string objet = moves[1];
                if (moveListPattern.Match(subject).Success && (moveListPattern.Match(objet).Success)) return true;
                return false;
            }
        }
    }
}
