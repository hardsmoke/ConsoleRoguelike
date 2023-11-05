using ConsoleRoguelike.CoreModule;

namespace ConsoleRoguelike.MapGeneration
{
    internal readonly struct MazeCell
    {
        public readonly Vector2Int Position;

        private readonly List<Vector2Int> _wallsLocalPositions = new List<Vector2Int>()
        {
            Vector2Int.Up,
            Vector2Int.Down,
            Vector2Int.Left,
            Vector2Int.Right,
        };

        public readonly IReadOnlyList<Vector2Int> WallsLocalPositions => _wallsLocalPositions;

        public MazeCell(Vector2Int position)
        {
            Position = position;
        }

        public readonly void RemoveUpWall()
        {
            RemoveWall(Vector2Int.Up);
        }

        public readonly void RemoveDownWall()
        {
            RemoveWall(Vector2Int.Down);
        }

        public readonly void RemoveLeftWall()
        {
            RemoveWall(Vector2Int.Left);
        }

        public readonly void RemoveRightWall()
        {
            RemoveWall(Vector2Int.Right);
        }

        public readonly void RemoveWall(Vector2Int wallLocalPosition)
        {
            _wallsLocalPositions.Remove(wallLocalPosition);
        }
    }
}
