namespace ConsoleRoguelike.MapGeneration
{
    internal class MazeGeneratorCell
    {
        public readonly MazeCell Cell;

        public bool IsVisited = false;
        public int DistanceFromStart = 0;

        public MazeGeneratorCell(MazeCell cell)
        {
            Cell = cell;
        }
    }
}
