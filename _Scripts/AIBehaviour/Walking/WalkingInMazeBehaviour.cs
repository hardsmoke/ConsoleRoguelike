using ConsoleRoguelike.CoreModule;
using ConsoleRoguelike.MapGeneration;

namespace ConsoleRoguelike.AIBehaviour.Walking
{
    internal class WalkingInMazeBehaviour : WalkingBehaviour
    {
        private readonly IReadOnlyTransform _readOnlyAITransform;
        private readonly MazeMemory _mazeMemory;

        private int _numberOfMovingBack = 0;

        public WalkingInMazeBehaviour(int mazeMemoryCapacity, IReadOnlyTransform aiTransform) : this(aiTransform)
        {
            _mazeMemory = new MazeMemory(mazeMemoryCapacity);
        }

        public WalkingInMazeBehaviour(MazeMemory mazeMemory, IReadOnlyTransform aiTransform) : this(aiTransform)
        {
            _mazeMemory = mazeMemory;
        }

        private WalkingInMazeBehaviour(IReadOnlyTransform aiTransform)
        {
            _readOnlyAITransform = aiTransform;
            _readOnlyAITransform.PositionChanged += OnEnemyPositionChanged;
        }

        public override Vector2Int GetNextMovePosition()
        {
            List<Vector2Int> possibleToMovePositions = GetPossibleToMovePositions();

            List<Vector2Int> unvisitedPositions = GetUnvisitedPositions(possibleToMovePositions);
            Vector2Int randomUnvisitedPosition = GetRandomPosition(unvisitedPositions);

            if (unvisitedPositions.Count == 0)
            {
                return GetBackPosition(possibleToMovePositions);
            }
            else
            {
                _numberOfMovingBack = 0;
                return randomUnvisitedPosition;
            }
        }

        public List<Vector2Int> GetPossibleToMovePositions()
        {
            List<Vector2Int> possibleToMovePositions = new List<Vector2Int>();

            Vector2Int[] positions = new Vector2Int[4]
            {
                _readOnlyAITransform.Position + Vector2Int.Down,
                _readOnlyAITransform.Position + Vector2Int.Up,
                _readOnlyAITransform.Position + Vector2Int.Left,
                _readOnlyAITransform.Position + Vector2Int.Right,
            };

            for (int i = 0; i < positions.Length; i++)
            {
                Vector2Int position = positions[i];
                if (_readOnlyAITransform.CanMove(position))
                {
                    possibleToMovePositions.Add(position);
                }
            }

            return possibleToMovePositions;
        }

        public List<Vector2Int> GetUnvisitedPositions(IReadOnlyList<Vector2Int> possibleToMovePositions)
        {
            List<Vector2Int> unsivitedPositions = new List<Vector2Int>();

            for (int i = 0; i < possibleToMovePositions.Count; i++)
            {
                Vector2Int position = possibleToMovePositions[i];
                if (_mazeMemory.PreviousPositions.Contains(position) == false)
                {
                    unsivitedPositions.Add(position);
                }
            }

            return unsivitedPositions;
        }

        /// <summary>
        /// Return Vector2Int.One * -1 if position can not be found
        /// </summary>
        /// <param name="positions"></param>
        /// <returns></returns>
        private Vector2Int GetRandomPosition(List<Vector2Int> positions)
        {
            if (positions.Count != 0)
            {
                Random rand = new Random();
                int randomPositionIndex = rand.Next(positions.Count);
                Vector2Int randomPosition = positions[randomPositionIndex];

                return randomPosition;
            }

            return Vector2Int.One * -1;
        }

        /// <summary>
        /// Return Vector2Int.One * -1 if position can not be found
        /// </summary>
        /// <param name="possibleToMovePositions"></param>
        /// <returns></returns>
        private Vector2Int GetBackPosition(List<Vector2Int> possibleToMovePositions)
        {
            int olderPositionIndex = _mazeMemory.GetOlderPositionIndexInMemory(possibleToMovePositions);

            if (olderPositionIndex != -1)
            {
                _numberOfMovingBack = _mazeMemory.PreviousPositions.Count - 2 - olderPositionIndex;
                Vector2Int previousPosition = _mazeMemory.PreviousPositions[olderPositionIndex];

                if (_readOnlyAITransform.CanMove(previousPosition))
                {
                    _numberOfMovingBack++;

                    if (_numberOfMovingBack == _mazeMemory.PreviousPositions.Count - 1)
                    {
                        _mazeMemory.ReverseMemory();
                    }

                    return previousPosition;
                }
            }

            return Vector2Int.One * -1;
        }

        private void OnEnemyPositionChanged(Transform transform, Vector2Int from, Vector2Int to)
        {
            _mazeMemory.AddPositionToMemory(from);
            _mazeMemory.AddPositionToMemory(to);
        }
    }
}
