using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static Hive.Notation.Tokenizer;
using static Hive.Notation.Parser;
using static Hive.Notation.Shared;
namespace Hive
{
    namespace Notation
    {
        public static class Parser
        {
            private static FTHexCorner parseDirection(bool isLeft, string line)
            {
                switch (line)
                {
                    case "-":
                        if (isLeft) return FTHexCorner.Left;
                        else return FTHexCorner.Right;
                    case "\\":
                        if (isLeft) return FTHexCorner.UpLeft;
                        else return FTHexCorner.DownRight;
                    case "/":
                        if (isLeft) return FTHexCorner.DownLeft;
                        else return FTHexCorner.UpRight;
                    default:
                        throw new Exception("POSITION FUCKED UP");
                }
            }
            private static Objet ParseObjet(string obj)
            {
                if (obj == ".") return new NullObjet();
                Subject basse = ParseSubject(obj);
                Objet objet = new Objet()
                {
                    numMarker = basse.numMarker,
                    pieceMarker = basse.pieceMarker,
                    playerMarker = basse.playerMarker,
                };
                string first = obj.First().ToString();
                string last = obj.Last().ToString();
                FTHexCorner position;
                if (positionalParticle.IsMatch(first))
                {
                    objet.positionalMarker = parseDirection(true, first);
                }
                else if (positionalParticle.IsMatch(last))
                {
                    objet.positionalMarker = parseDirection(false, last);
                }
                else
                {
                    objet.onTop = true;
                }
                return objet;
            }
            private static Subject ParseSubject(string subj)
            {
                var subject = new Subject()
                {
                    numMarker = 0,
                };

                foreach (var token in subj)
                {
                    if (playerMark.IsMatch(token.ToString())) { subject.playerMarker = token == 'w' ? Players.WHITE : Players.BLACK; }
                    else if (pieceParticle.IsMatch(token.ToString())) { subject.pieceMarker = pieceDict[token.ToString().ToUpper()]; }
                }
                if (numberSpecifierParticle.IsMatch(subj))
                {
                    subject.numMarker = int.Parse(numberSpecifierParticle.Match(subj).Value[1].ToString());
                };
                return subject;
            }
            private static (Subject, Objet) ParseMove(List<string> tokenized)
            {
                return (ParseSubject(tokenized[0]), ParseObjet(tokenized[1]));
            }
            internal static List<(Subject, Objet)> Parse(List<List<string>> moves) => moves.Select(ParseMove).ToList();
            internal static Subject parseUnformatted(string lowercase, bool isSubject)
            {
                System.Text.RegularExpressions.Match item = rawMoveTemplate.Match(lowercase);
                if (item.Success)
                {
                    var values = item.Groups.Values.ToList();
                    bool hasDir = values[1].Success || values[5].Success;
                    bool isDot = values[6].Success;
                    if (isDot) return new NullObjet();
                    bool isLeft = values[1].Success;
                    string leftMarker = values[1].Value;
                    string _player = values[2].Value;
                    string _piece = values[3].Value;
                    bool hasNum = values[4].Success;
                    string _num = values[4].Value;
                    string rightMarker = values[5].Value;
                    Players player = _player == "w" ? Players.WHITE : Players.BLACK;
                    Pieces piece = pieceDict[_piece.ToUpper()];
                    int num = hasNum ? int.Parse(_num) : 0;
                    if (!isSubject && hasDir)
                    {
                        return new Objet()
                        {
                            numMarker = num,
                            playerMarker = player,
                            pieceMarker = piece,
                            positionalMarker = parseDirection(isLeft, isLeft ? leftMarker : rightMarker)
                        };
                    }
                    else if (!isSubject && !hasDir)
                    {
                        return new Objet()
                        {
                            numMarker = num,
                            playerMarker = player,
                            pieceMarker = piece,
                            onTop = true
                        };
                    }
                    else if (isSubject)
                    {
                        return new Subject()
                        {
                            numMarker = num,
                            playerMarker = player,
                            pieceMarker = piece,
                        };
                    }
                }
                throw new Exception("Unformatted parsing error");
            }
        }
    }
}
