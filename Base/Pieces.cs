namespace Hive
{
    public enum Pieces
    {
        BEE,
        ANT,
        SPIDER,
        GRASSHOPPER,
        MOSQUITO,
        BEETLE,
        LADYBUG,
        //TODO
        //the loathsome
        //PILLBUG,
    }
    public class Piece
    {
        public Pieces type;
        public Players owner;
        public Cell location;
        public int id;

        public Piece(Pieces t, Players o, Cell l, int i)
        {
            type = t;
            owner = o;
            location = l;
            id = i;
        }
        public static Piece create(Pieces piece, Players player, Cell destination, int id)
        {
            switch (piece)
            {
                case Pieces.BEE:
                    return new Bee(player, destination);
                case Pieces.ANT:
                    return new Ant(player, destination, id);
                case Pieces.SPIDER:
                    return new Spider(player, destination, id);
                case Pieces.GRASSHOPPER:
                    return new Grasshopper(player, destination, id);
                case Pieces.MOSQUITO:
                    return new Mosquito(player, destination);
                case Pieces.BEETLE:
                    return new Beetle(player, destination, id);
                case Pieces.LADYBUG:
                    return new Ladybug(player, destination);
                default:
                    throw new ArgumentException("unrecognized piece type cannot be created");
            }
        }
        public static List<Path> getLegalMoves(Piece piece, Board board)
        {
            switch (piece.type)
            {
                case Pieces.BEE:
                    return Bee._getLegalMoves(board, piece.location);
                case Pieces.SPIDER:
                    return Spider._getLegalMoves(board, piece.location);
                case Pieces.BEETLE:
                    return Beetle._getLegalMoves(board, piece.location);
                case Pieces.LADYBUG:
                    return Ladybug._getLegalMoves(board, piece.location);
                case Pieces.GRASSHOPPER:
                    return Grasshopper._getLegalMoves(board, piece.location);
                case Pieces.MOSQUITO:
                    return Mosquito._getLegalMoves(board, piece.location);
                case Pieces.ANT:
                    return Ant._getLegalMoves(board, piece.location);
                default:
                    throw new NotImplementedException();
            }
        }
    }

}
