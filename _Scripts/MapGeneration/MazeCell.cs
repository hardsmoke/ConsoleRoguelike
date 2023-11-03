namespace ConsoleRoguelike.MapGeneration
{
    internal class MazeCell
    {
        public readonly Vector2Int Position;

        public List<Vector2Int> WallsLocalPositions = new List<Vector2Int>()
        {
            Vector2Int.Up,
            Vector2Int.Down,
            Vector2Int.Left,
            Vector2Int.Right,
        };

        public MazeCell(Vector2Int position)
        {
            Position = position;
        }
    }
}
