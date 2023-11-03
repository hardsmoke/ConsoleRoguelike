namespace ConsoleRoguelike.Render
{
    internal interface IConsoleRenderer : IRenderer
    {
        public Vector2Int StartRenderPosition { get; }
        public Vector2Int GetBottomLeftRenderedPosition();
    }
}
