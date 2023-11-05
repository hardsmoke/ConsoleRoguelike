using ConsoleRoguelike.CoreModule;

namespace ConsoleRoguelike.MapGeneration
{
    internal class MazeMemory : IReadOnlyMazeMemory
    {
        private List<Vector2Int> _previousPositions = new List<Vector2Int>();
        public IReadOnlyList<Vector2Int> PreviousPositions => _previousPositions;

        public readonly int MazeMemoryCapacity = 0;

        public MazeMemory(int mazeMemoryCapacity)
        {
            MazeMemoryCapacity = mazeMemoryCapacity;
        }

        public void ReverseMemory()
        {
            _previousPositions.Reverse();
        }

        /// <summary>
        /// return -1 if not found
        /// </summary>
        /// <param name="possibleToMovePositions"></param>
        /// <returns></returns>
        public int GetOlderPositionIndexInMemory(List<Vector2Int> possibleToMovePositions)
        {
            if (possibleToMovePositions.Count != 0)
            {
                int olderPositionIndex = _previousPositions.IndexOf(possibleToMovePositions[0]);

                for (int i = 1; i < possibleToMovePositions.Count; i++)
                {
                    int index = _previousPositions.IndexOf(possibleToMovePositions[i]);
                    if (olderPositionIndex > index)
                    {
                        olderPositionIndex = index;
                    }
                }

                return olderPositionIndex;
            }

            return -1;
        }

        public void AddPositionToMemory(Vector2Int position)
        {
            if (_previousPositions.Count >= MazeMemoryCapacity)
            {
                _previousPositions.RemoveAt(0);
            }

            _previousPositions.Add(position);
        }
    }
}
