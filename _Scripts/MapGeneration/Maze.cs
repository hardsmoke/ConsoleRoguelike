namespace ConsoleRoguelike.MapGeneration
{
    internal class Maze
    {
        public readonly MazeCell[,] Cells;
        public MazeCell ExitCell;

        public Maze(MazeCell[,] cells)
        {
            Cells = cells;
        }

        public Maze(MazeCell[,] cells, MazeCell exitCell) : this(cells)
        {
            ExitCell = exitCell;
        }
    }
}
