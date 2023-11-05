using ConsoleRoguelike.CoreModule;
using ConsoleRoguelike.CreatureCondition;
using System.Text;

namespace ConsoleRoguelike.Render
{
    internal class DeathScreenRenderer : IConsoleLineRenderer
    {
        private readonly IReadOnlyHealthBarRenderer _healthBarRenderer;
        private readonly ConsoleColor _color;

        private int _barWidth = 0;
        public int Width => _barWidth;

        public Vector2Int StartRenderPosition => _healthBarRenderer.GetBottomLeftRenderedPosition() + Vector2Int.Up;

        public DeathScreenRenderer(IReadOnlyHealthBarRenderer healthBarRenderer, ConsoleColor color = ConsoleColor.White)
        {
            _healthBarRenderer = healthBarRenderer;
            _healthBarRenderer.Health.Died += RenderDeathScreen;
            _healthBarRenderer.Health.HealthChanged += OnHealthValueChanged;

            _color = color;
        }

        public Vector2Int GetBottomLeftRenderedPosition()
        {
            return StartRenderPosition;
        }

        public void ClearRender()
        {
            Console.SetCursorPosition(StartRenderPosition.X, StartRenderPosition.Y);
            StringBuilder builder = new StringBuilder();
            builder.Append(' ', Width);
            Console.Write($"{builder}");
        }

        public void Render()
        {
            RenderDeathScreen(null);
        }

        public void RenderDeathScreen(IDamager? killer)
        {
            Console.ForegroundColor = _color;
            Console.SetCursorPosition(StartRenderPosition.X, StartRenderPosition.Y);
            string deathScreenText = $"{killer?.DeathReason} | PRESS F TO PAY RESPECT";
            _barWidth = deathScreenText.Length;
            Console.Write(deathScreenText);
        }

        private void OnHealthValueChanged(float previous, float current)
        {
            if (current != 0)
            {
                ClearRender();
            }
        }
    }
}
