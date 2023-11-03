using ConsoleRoguelike.GameScene;
using ConsoleRoguelike.MapGeneration;

namespace ConsoleRoguelike.GameObjects.AI
{
    internal abstract class AliveEnemy : Enemy
    {
        private readonly MazeMemory _mazeMemory;
        public IReadOnlyMazeMemory MazeMemory => _mazeMemory;

        private int _numberOfMovingBack = 0;

        protected AliveEnemy(
            Vector2Int position, 
            char renderedChar, 
            IReadOnlyScene scene, 
            SceneLayer sceneLayer, 
            int mazeMemoryCapacity, 
            ConsoleColor color = ConsoleColor.Red) : base(position, renderedChar, scene, sceneLayer, color)
        {
            _mazeMemory = new MazeMemory(mazeMemoryCapacity);

            PositionChanged += OnEnemyPositionChanged;
        }

        public override void MoveNext()
        {
            List<Vector2Int> unvisitedPositions = GetUnvisitedPositions();
            List<Vector2Int> possibleToMovePositions = GetPossibleToMovePositions();

            if (TryMoveRandomly(unvisitedPositions) == false)
            {
                MoveBack(possibleToMovePositions);
            }
            else
            {
                _numberOfMovingBack = 0;
            }
        }

        private List<Vector2Int> GetUnvisitedPositions()
        {
            List<Vector2Int> unsivitedPositions = new List<Vector2Int>();

            Vector2Int topPosition = Position + Vector2Int.Down;
            Vector2Int bottomPosition = Position + Vector2Int.Up;
            Vector2Int leftPosition = Position + Vector2Int.Left;
            Vector2Int rightPosition = Position + Vector2Int.Right;

            if (CanMove(topPosition) && _mazeMemory.PreviousPositions.Contains(topPosition) == false)
                unsivitedPositions.Add(topPosition);

            if (CanMove(bottomPosition) && _mazeMemory.PreviousPositions.Contains(bottomPosition) == false)
                unsivitedPositions.Add(bottomPosition);

            if (CanMove(leftPosition) && _mazeMemory.PreviousPositions.Contains(leftPosition) == false)
                unsivitedPositions.Add(leftPosition);

            if (CanMove(rightPosition) && _mazeMemory.PreviousPositions.Contains(rightPosition) == false)
                unsivitedPositions.Add(rightPosition);

            return unsivitedPositions;
        }

        private bool TryMoveRandomly(List<Vector2Int> positions)
        {
            if (positions.Count != 0)
            {
                Random rand = new Random();
                int randomPositionIndex = rand.Next(positions.Count);
                Vector2Int randomPosition = positions[randomPositionIndex];
                MoveTo(randomPosition);

                return true;
            }

            return false;
        }

        private void MoveBack(List<Vector2Int> possibleToMovePositions)
        {
            int olderPositionIndex = _mazeMemory.GetOlderPositionIndexInMemory(possibleToMovePositions);

            if (olderPositionIndex != -1)
            {
                _numberOfMovingBack = _mazeMemory.PreviousPositions.Count - 2 - olderPositionIndex;
                Vector2Int previousPosition = _mazeMemory.PreviousPositions[olderPositionIndex];

                if (CanMove(previousPosition))
                {
                    _numberOfMovingBack++;
                    MoveTo(previousPosition);

                    if (_numberOfMovingBack == _mazeMemory.PreviousPositions.Count - 1)
                    {
                        _mazeMemory.ReverseMemory();
                    }
                }
            }
        }

        private void OnEnemyPositionChanged(Transform transform, Vector2Int from, Vector2Int to)
        {
            _mazeMemory.AddPositionInMemory(from);
            _mazeMemory.AddPositionInMemory(to);
        }
    }
}
