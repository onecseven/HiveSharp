using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;

namespace Hive
{
    public partial class Hive
    {
        public Phases game_status = Phases.PREPARED;
        public Players turn = Players.WHITE;
        public List<Move> moves = new List<Move>();
        public Dictionary<Players, Player> players = new Dictionary<Players, Player>
        {
            [Players.WHITE] = new Player(Players.WHITE),
            [Players.BLACK] = new Player(Players.BLACK),
        };
        public Board board = new Board();
        public Players? winner = null;
        //when the constructor is over
        public delegate void gameIsReadyEventHandler();
        public event gameIsReadyEventHandler gameIsReady;
        //when a move is accepted
        public delegate void onSuccessfulMoveEventHandler(Move move);
        public event onSuccessfulMoveEventHandler onSuccessfulMove;
        //when a move is rejected
        public delegate void onFailedMoveEventHandler(string error);
        public event onFailedMoveEventHandler onFailedMove;
        //when the game is done
        public delegate void onGameOverEventHandler();
        public event onGameOverEventHandler onGameOver;
        public Hive()
        {
        }
        public void send_move(Move move)
        {
            if (!moveIsValid(move) || game_status == Phases.GAME_OVER)
            {
                onFailedMove?.Invoke("Invalid Move");
                return;
            }
            switch (move.type)
            {
                case MoveType.INITIAL_PLACE:
                    this.initialPlace((INITIAL_PLACE)move);
                    break;
                case MoveType.MOVE_PIECE:
                    this.move((MOVE_PIECE)move);
                    break;
                case MoveType.PLACE:
                    this.place((PLACE)move);
                    break;
                case MoveType.AUTOPASS:
                    break;
            }
            moves.Add(move);
            onSuccessfulMove?.Invoke(move);
            if (wincon_check()) game_over();
            else advanceTurn();
        }
        public void advanceTurn()
        {

            turn = ((Players)((int)turn ^ 1));
            autopassCheck();
        }
        private void game_over()
        {
            game_status = Phases.GAME_OVER;
            List<KeyValuePair<Cell, Tile>> bees = board.tiles.Where(kvp => kvp.Value.isOccupied && kvp.Value.pieces.Any(pie => pie.type == Pieces.BEE)).ToList();
            if (bees.All(kvp => board.getOccupiedNeighbors(kvp.Key).Count == 6))
            {
            }
            else
            {
                var color = bees.Where(kvp => board.getOccupiedNeighbors(kvp.Key).Count != 6).ToList();
                winner = color[0].Value.pieces.Find(piece => piece.type == Pieces.BEE)?.owner;
            }
            onGameOver?.Invoke();
        }
        private void place(PLACE move)
        {
            Player pieceOwner = players[move.player];
            var id = pieceOwner.piecePlaced(move.piece);
            Piece newPiece = Piece.create(move.piece, move.player, (move.destination), id);
            board.placePiece(newPiece);
        }
        private void move(MOVE_PIECE move)
        {
            Piece originalPiece = board.tiles[move.origin].activePiece;
            if ((board.tiles[move.origin].isOccupied && !board.tiles[move.destination].isOccupied) ||
                (board.tiles[move.destination].isOccupied && (move.piece == Pieces.BEETLE || move.piece == Pieces.MOSQUITO))
                )
            {
                Piece newPiece = Piece.create(originalPiece.type, originalPiece.owner, move.destination, originalPiece.id);
                board.movePiece(move.origin, newPiece);
            }
            else
            {
                throw new Exception("Move passed checks but failed to play...?");
            }
        }
        private void initialPlace(INITIAL_PLACE move) => place(new PLACE(move.player, move.piece, move.destination));
        private bool moveIsValid(Move move)
        {
            if (move.player != turn || (mustPlayBee(move.player) && (move.piece != Pieces.BEE)))
            {
                return false;
            }
            switch (move.type)
            {
                case MoveType.INITIAL_PLACE:
                    INITIAL_PLACE initialPlaceCasted = (INITIAL_PLACE)move;
                    return initialPlacementCheck(initialPlaceCasted);
                case MoveType.PLACE:
                    PLACE placeCasted = (PLACE)move;
                    return placementLegalityCheck(placeCasted.destination, placeCasted.player, placeCasted.piece);
                case MoveType.MOVE_PIECE:
                    MOVE_PIECE moveCasted = (MOVE_PIECE)move;
                    if (moveIsLegal(moveCasted) == false || oneHiveRuleCheck(moveCasted.origin) == false) return false;
                    break;
                default:
                    break;
            }
            return true;
        }
    }
}
