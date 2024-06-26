﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hive
{
    public class Mosquito : Piece
    {
        public static List<Path> _getLegalMoves(Board board, Cell origin)
        {

            List<Path> result = new List<Path>();
            Piece originalPiece = board.tiles[origin].activePiece;
            List<Cell> occupied_guys = board.getOccupiedNeighbors(origin);
            if (originalPiece.type == Pieces.MOSQUITO && board.tiles[origin].hasBlockedPiece)
            {
                return Piece.getLegalMoves(Piece.create(Pieces.BEETLE, originalPiece.owner, origin, -1), board);
            }
            foreach (Cell occupied in occupied_guys)
            {
                Pieces current_type = board.tiles[occupied].activePiece.type;
                if (current_type == Pieces.MOSQUITO) continue;
                List<Path> convertedMoves = Piece.getLegalMoves(Piece.create(current_type, originalPiece.owner, origin, -1), board);
                result.AddRange(convertedMoves);
            }
            return result;
        }

        public Mosquito(Players p, Cell l) : base(Pieces.MOSQUITO, p, l, 0) { }
    }
}
