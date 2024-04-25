using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Hive
{
    namespace Notation
    {
        internal static class Shared
        {
            public static Regex rawMoveTemplate = new Regex(@"([\/\\-])?([wb]{1}){1}([SAQBMLGPsaqbmlgp]{1})([123]{1})?([\/\\-])?|([\.])");
            public static Regex moveListPattern = new Regex(@"(([\/\\-])?([wb]{1}){1}([SAQBMLGP]{1})([123]{1})?([\/\\-])?|[\.])");
            public static Regex positionalParticle = new Regex(@"([\/\\-])");
            public static Regex playerMark = new Regex(@"([wb]{1}){1}");
            public static Regex pieceParticle = new Regex(@"([SAQBMLGP]{1})");
            public static Regex numberSpecifierParticle = new Regex(@"(?:[SAQBMLGP]{1})([123]{1})");

            internal static Dictionary<string, Pieces> pieceDict = new Dictionary<string, Pieces>()
            {
                ["S"] = Pieces.SPIDER,
                ["A"] = Pieces.ANT,
                ["Q"] = Pieces.BEE,
                ["B"] = Pieces.BEETLE,
                ["M"] = Pieces.MOSQUITO,
                ["L"] = Pieces.LADYBUG,
                ["G"] = Pieces.GRASSHOPPER,
            };
            internal static Cell getCellFromDirection(Cell origin, FTHexCorner dir)
            {
                return new Cell(origin.x + HexUtils.corners[dir].x, origin.y + HexUtils.corners[dir].y, origin.z + HexUtils.corners[dir].z);
            }

            internal static Dictionary<Pieces, string> reversedPieceDict = pieceDict.ToDictionary(x => x.Value, x => x.Key);
            internal static Pieces[] multiples = {
            Pieces.ANT,
            Pieces.SPIDER,
            Pieces.BEETLE,
            Pieces.GRASSHOPPER,
        };
            internal static Dictionary<Pieces, int> reference = new Dictionary<Pieces, int>()
            {
                [Pieces.BEE] = 1,
                [Pieces.MOSQUITO] = 1,
                [Pieces.LADYBUG] = 1,
                [Pieces.SPIDER] = 2,
                [Pieces.BEETLE] = 2,
                [Pieces.ANT] = 3,
                [Pieces.GRASSHOPPER] = 3
            };
            internal class Subject
            {
                public Players playerMarker;
                public Pieces pieceMarker;
                public int numMarker = 0;
                override public string ToString() => $"playerMarker: {playerMarker}\npieceMarker: {pieceMarker}\nnumMarker: {numMarker}";
                public string ToNotation()
                {
                    string result = "";
                    if (playerMarker == Players.BLACK) result += "b";
                    else result += "w";
                    result += pieceDict.Where<KeyValuePair<string, Pieces>>(kvp => kvp.Value == pieceMarker).First().Key;
                    if (numMarker != 0) result += numMarker.ToString();
                    return result;
                }
            }
            internal class Objet : Subject
            {
                public FTHexCorner positionalMarker;
                public bool onTop = false;
                override public string ToString() => $"positionalMarker: {positionalMarker}\nplayerMarker: {playerMarker}\npieceMarker: {pieceMarker}\nnumMarker: {numMarker}";
                public new string ToNotation()
                {
                    Subject basse = new Subject()
                    {
                        playerMarker = this.playerMarker,
                        pieceMarker = this.pieceMarker,
                        numMarker = this.numMarker
                    };
                    string basseStr = basse.ToNotation();
                    switch (positionalMarker)
                    {
                        case FTHexCorner.Right:
                            return basseStr + "-";
                        case FTHexCorner.UpRight:
                            return basseStr + "/";
                        case FTHexCorner.UpLeft:
                            return "/" + basseStr;
                        case FTHexCorner.Left:
                            return "-" + basseStr;
                        case FTHexCorner.DownLeft:
                            return "\\" + basseStr;
                        case FTHexCorner.DownRight:
                            return basseStr + "\\";
                    }
                    throw new Exception("wtf kind of notation request was that");
                }
            }
            internal class NullObjet : Objet
            {
                override public string ToString() => $"null";
                public new string ToNotation() => ".";
            }

        }
    }
}
