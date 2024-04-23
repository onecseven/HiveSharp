namespace Hive
{
    public partial class Board
    {
        public List<Cell> getEmptyNeighbors(Cell cell) => getNeighbors(cell).Where(x => !tileIsOccupied(x)).ToList();
        public List<Cell> getOccupiedNeighbors(Cell cell) => getNeighbors(cell).Where(x => tileIsOccupied(x)).ToList();
        public List<Cell> getNeighbors(Cell origin) => HiveUtils.getNeighbors(origin).ToList();
        public List<Cell> adjacentLegalCells(Cell cell)
        {
            List<Cell> empty = getEmptyNeighbors(cell);
            List<Cell> neighbors = getOccupiedNeighbors(cell);
            HashSet<Cell> neighbor_adjacent = new HashSet<Cell>();
            foreach (Cell neighbor in neighbors)
            {
                List<Cell> neighborAdjacentEmpties = getEmptyNeighbors(neighbor);
                neighborAdjacentEmpties.ForEach(temp_tile => neighbor_adjacent.Add(temp_tile));
            }
            var prelim = empty.Intersect(neighbor_adjacent).ToList();
            return prelim.ToList();
        }
        public List<Cell> connectingAdjacents(Cell a, Cell b)
        {
            //this is for the freedom to move rule
            /*
             *returns {xy}

               /  \
              |  y |
             / \  /  \
            | a ||  b |
             \ /  \  /
               | x |     
             `  \ /
             */
            List<Cell> aNeighbors = HiveUtils.getNeighbors(a);
            List<Cell> bNeighbors = HiveUtils.getNeighbors(b);
            List<Cell> union = aNeighbors.Intersect(bNeighbors).ToList();
            if (union.Count > 0 && union.Count == 2) return union;
            else throw new Exception("connectingAdjacents fucked up somewhere!");
        }

    }
}
