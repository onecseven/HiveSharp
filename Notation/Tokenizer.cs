using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hive
{
    namespace Notation
    {
        public static class Tokenizer
        {
        public static List<List<string>> TokenizeFormatted(string moveList)
        {
            List<string> lines = moveList.Split("\n").ToList<string>().Where(str => str.Count() > 0).ToList();
            List<List<string>> tokenized = new List<List<string>>();
            foreach (string line in lines)
            {
                List<string> tokenizedMove = TokenizeFormattedMove(line);
                if (tokenizedMove.Count > 0) tokenized.Add(tokenizedMove);
            }
            return tokenized;
        }

        public static List<List<string>> TokenizeRaw(string moveList) => moveList
                                                                        .Split("\n")
                                                                        .ToList<string>()
                                                                        .Where(str => str.Count() > 0)
                                                                        .Select(TokenizeMoveRaw)
                                                                        .ToList();

        
        public static List<string> TokenizeMoveRaw(string move) => move.Split(" ").ToList<string>().Where(str => str.Count() > 0).ToList();
        internal static List<string> TokenizeFormattedMove(string move)
        {
            var basicSplit = move.Split(":").ToList();
            if (basicSplit.Count < 1) throw new Exception("weird move crashed tokenizemove");
            var designator = basicSplit[0];
            if (designator == "URL") return new List<string>();
            else if (int.TryParse(designator, out int id))
            {
                var moves = basicSplit[1].Split(" ").ToList<string>().Where(str => str.Count() > 0).ToList();
                return moves;
            }
            return new List<string>();
        }
        }

    }
}
