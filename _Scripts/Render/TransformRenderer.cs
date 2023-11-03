namespace ConsoleRoguelike.Render
{
    internal class TransformRenderer : IConsoleRenderer
    {
        public readonly Transform Transform;
        public readonly char PlayerRenderChar;
        public readonly ConsoleColor Color;

        public Vector2Int StartRenderPosition { get => Transform.Position; }

        public TransformRenderer(Transform transform, char renderChar, ConsoleColor color)
        {
            Transform = transform;
            PlayerRenderChar = renderChar;

            transform.PositionChanged += OnPositionChanged;
            Color = color;
        }

        private void OnPositionChanged(Transform transform, Vector2Int previousPosition, Vector2Int newPosition)
        {
            Render();
        }

        public Vector2Int GetBottomLeftRenderedPosition()
        {
            return Transform.Position;
        }

        public void Render()
        {
            Console.ForegroundColor = Color;
            Console.SetCursorPosition(Transform.Position.X, Transform.Position.Y);
            Console.Write(PlayerRenderChar);
        }

        public static List<TransformRenderer> CreateRenderers<T>(List<T> transforms, char renderChar, ConsoleColor color) where T : Transform
        {
            List<TransformRenderer> renderers = new List<TransformRenderer>();

            for (int i = 0; i < transforms.Count; i++)
            {
                renderers.Add(new TransformRenderer(transforms[i], renderChar, color));
            }

            return renderers;
        }
    }
}
