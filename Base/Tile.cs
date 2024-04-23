using Hive;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hive
{
    public class Tile
    {

        public Cell cell;
        public List<Piece> pieces = new List<Piece>();
        public Piece activePiece
        {
            get
            {
                if (pieces.Count > 0) return pieces[0];
                else return null;
            }
        }
        public int zIndex => isOccupied ? pieces.Count : 0;
        public bool hasBlockedPiece => pieces.Count > 1;
        public bool isOccupied => pieces.Count > 0;
        public Tile(Cell cell)
        {
            this.cell = cell;
        }
        public void addPiece(Piece piece) { pieces.Insert(0, piece); }
        public Piece removePiece()
        {
            Piece returnable = pieces[0];
            pieces.RemoveAt(0);
            return returnable;
        }
    }
}
