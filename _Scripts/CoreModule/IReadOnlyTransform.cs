namespace ConsoleRoguelike.CoreModule
{
    internal interface IReadOnlyTransform
    {
        public event Action<Transform, Vector2Int, Vector2Int> PositionChanged;
        public Vector2Int Position { get; set; }

        public bool CanMove(Vector2Int toMove);
    }
}
