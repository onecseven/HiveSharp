namespace Hive
{
    public partial class Hive
    {
        bool mustPlayBee(Players player)
        {
            bool hasPlayedBee = moves.Where(move => move.player == player).Where(move => move.type == MoveType.INITIAL_PLACE || move.type == MoveType.PLACE).Any(move => move.piece == Pieces.BEE);
            bool hasPlayedThreeMoves = (moves.Where(move => move.player == player).ToList().Count) == 3;
            bool belowLimit = (moves.Where(move => move.player == player).ToList().Count) < 3;
            if (hasPlayedThreeMoves && !hasPlayedBee) return true;
            else if (!belowLimit && !hasPlayedThreeMoves && !hasPlayedBee)
            {
                throw new ArgumentException("illegal game state");
            }
            return false;
        }
    }
}

