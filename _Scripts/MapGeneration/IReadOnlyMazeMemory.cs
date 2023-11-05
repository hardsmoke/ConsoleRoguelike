using ConsoleRoguelike.CoreModule;

namespace ConsoleRoguelike.MapGeneration
{
    internal interface IReadOnlyMazeMemory
    {
        public IReadOnlyList<Vector2Int> PreviousPositions { get; }
        public int GetOlderPositionIndexInMemory(List<Vector2Int> possibleToMovePositions);
    }
}
