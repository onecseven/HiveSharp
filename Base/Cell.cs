using Sylves;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hive
{
    public enum HexOrientation
    {
        PointyTopped,
        FlatTopped
    }

    public struct Cell : IEquatable<Cell>
    {
        public int x;

        public int y;

        public int z;

        [DebuggerStepThrough]
        public Cell(int x, int y)
        {
            this.x = x;
            this.y = y;
            z = 0;
        }

        [DebuggerStepThrough]
        public Cell(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public bool Equals(Cell other)
        {
            if (x == other.x && y == other.y)
            {
                return z == other.z;
            }

            return false;
        }

        public override bool Equals(object other)
        {
            if (other is Cell)
            {
                Cell other2 = (Cell)other;
                return Equals(other2);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return (x, y, z).GetHashCode();
        }

        public static Cell operator +(Cell cell, Vector3Int offset)
        {
            return new Cell(cell.x + offset.x, cell.y + offset.y, cell.z + offset.z);
        }

        public static Cell operator +(Vector3Int offset, Cell cell)
        {
            return new Cell(cell.x + offset.x, cell.y + offset.y, cell.z + offset.z);
        }

        public static Cell operator -(Cell cell, Vector3Int offset)
        {
            return new Cell(cell.x - offset.x, cell.y - offset.y, cell.z - offset.z);
        }

        public static Cell operator -(Vector3Int offset, Cell cell)
        {
            return new Cell(cell.x - offset.x, cell.y - offset.y, cell.z - offset.z);
        }

        public static explicit operator Vector3Int(Cell c)
        {
            return new Vector3Int(c.x, c.y, c.z);
        }

        public static explicit operator Cell(Vector3Int c)
        {
            return new Cell(c.x, c.y, c.z);
        }

        public static bool operator ==(Cell lhs, Cell rhs)
        {
            if (lhs.x == rhs.x && lhs.y == rhs.y)
            {
                return lhs.z == rhs.z;
            }

            return false;
        }

        public static bool operator !=(Cell lhs, Cell rhs)
        {
            return !(lhs == rhs);
        }

        public override string ToString()
        {
            return $"({x}, {y}, {z})";
        }
    }
}
