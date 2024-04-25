namespace Hive
{
    public class Path
    {
        public List<Cell> steps;
        public Cell last
        {
            get
            {
                if (steps.Count < 1) throw new ArgumentOutOfRangeException("path class");
                return steps[steps.Count - 1];
            }
        }
        public Cell penult
        {
            get
            {
                if (steps.Count < 2) throw new ArgumentOutOfRangeException("path class");
                return steps[steps.Count - 2];
            }
        }
        public bool isSingleStep { get => steps.Count == 1; }
        public List<(Cell first, Cell last)> pairs
        {
            get
            {
                if (isSingleStep) { return new List<(Cell first, Cell last)>(); }
                List<(Cell first, Cell last)> result = new List<(Cell first, Cell last)>();
                for (int i = 0; i < steps.Count - 2; i++)
                {
                    //int next = i + 1 > steps.Count - 1 ? 0 : i + 1;
                    result.Add((steps[i], steps[i + 1]));
                }
                return result;
            }
        }
        public Pieces pathType;
        public Path(List<Cell> steps, Pieces type)
        {
            this.steps = steps;
            this.pathType = type;
        }
        public bool isNullPath { get => steps.Count == 0; }
        public Path(Cell step, Pieces type)
        {
            this.steps = new List<Cell>() { step };
            this.pathType = type;
        }
        public Path(params Cell[] step)
        {
            this.steps = step.ToList();
        }
    }

}
