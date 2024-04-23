namespace Hive
{
    public partial class Board
    {
        public Dictionary<Cell, Tile> tiles = new Dictionary<Cell, Tile>();
        public List<Cell> filteredPiecesInPlay { get { return tiles.Where(kvp => kvp.Value.isOccupied).ToList().Select(KeyValuePair => KeyValuePair.Key).ToList(); } }
        public Board()
        {
            foreach (Cell cell in HexUtils.HexGen(36, 24, HexOrientation.PointyTopped))
            {
                tiles.Add(cell, new Tile(cell));
            }
        }
        public void movePiece(Cell origin, Piece piece)
        {
            tiles[origin].removePiece();
            tiles[piece.location].addPiece(piece);
        }
        public void placePiece(Piece pieceToPlace) => tiles[pieceToPlace.location].addPiece(pieceToPlace);
     
        //SEND TO SPIDER/ANT RESPECTIVELY
        public List<Cell> hypotheticalAdjacentLegalCells(Cell cell, Cell exclude)
        {
            List<Cell> empty = getEmptyNeighbors(cell);
            List<Cell> neighbors = getOccupiedNeighbors(cell);
            neighbors.Remove(exclude);
            HashSet<Cell> neighbor_adjacent = new HashSet<Cell>();
            foreach (Cell neighbor in neighbors)
            {
                List<Cell> neighborAdjacentEmpties = getEmptyNeighbors(neighbor);
                neighborAdjacentEmpties.ForEach(temp_tile => neighbor_adjacent.Add(temp_tile));
            }
            var prelim = empty.Intersect(neighbor_adjacent).Where(next => hypotheticallCanMoveBetween(cell, next, exclude)).ToList();
            return prelim.ToList();
        }
        //FIXME WONTFIX this is awful
        public List<Cell> hypotheticalAdjacentLegalCellsForAnts(Cell cell, List<Cell> exclude)
        {
            List<Cell> empty = getEmptyNeighbors(cell);
            List<Cell> neighbors = getOccupiedNeighbors(cell);
            foreach (Cell toExclude in exclude)
            {
                neighbors.Remove(toExclude);
            }
            HashSet<Cell> neighbor_adjacent = new HashSet<Cell>();
            foreach (Cell neighbor in neighbors)
            {
                List<Cell> neighborAdjacentEmpties = getEmptyNeighbors(neighbor);
                neighborAdjacentEmpties.ForEach(temp_tile => neighbor_adjacent.Add(temp_tile));
            }
            var prelim = empty.Intersect(neighbor_adjacent).Where(next => hypotheticallCanMoveBetweenForAnts(cell, next, exclude)).ToList();
            return prelim.ToList();
        }

        public bool hypotheticallCanMoveBetween(Cell a, Cell b, Cell exclude)
        {
            List<Cell> adjacents = connectingAdjacents(a, b);
            if (adjacents.Contains(exclude)) return true;
            else return !adjacents.All(cell => tileIsOccupied(cell));
        }
        public bool hypotheticallCanMoveBetweenForAnts(Cell a, Cell b, List<Cell> exclude)
        {
            List<Cell> adjacents = connectingAdjacents(a, b);
            if (adjacents.Intersect(exclude).Count() > 0) return true;
            else return !adjacents.All(cell => tileIsOccupied(cell));
        }

    }
}
