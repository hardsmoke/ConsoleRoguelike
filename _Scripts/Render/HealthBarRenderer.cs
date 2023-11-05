using ConsoleRoguelike.CoreModule;
using ConsoleRoguelike.CreatureCondition;
using System.Text;

namespace ConsoleRoguelike.Render
{
    internal class HealthBarRenderer : IReadOnlyHealthBarRenderer
    {
        private readonly IReadOnlyHealth _health;
        public IReadOnlyHealth Health => _health;

        private readonly ConsoleColor _color;
        private int _barWidth = 0;
        public int Width => _barWidth;

        private Vector2Int _startRenderPosition;
        public Vector2Int StartRenderPosition { get => _startRenderPosition; set => _startRenderPosition = value; }

        public HealthBarRenderer(IReadOnlyHealth health, ConsoleColor color = ConsoleColor.White)
        {
            _health = health;
            _color = color;

            Health.HealthChanged += OnHealthChanged;
            Health.Died += OnDied;
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
            ClearRender();
            Console.ForegroundColor = _color;
            Console.SetCursorPosition(StartRenderPosition.X, StartRenderPosition.Y);
            string healthBarText = $"Health: {Health.Value}%";
            _barWidth = healthBarText.Length;
            Console.Write(healthBarText);
        }

        public Vector2Int GetBottomLeftRenderedPosition()
        {
            return _startRenderPosition;
        }

        private void OnHealthChanged(float healthBefore, float currentHealth)
        {
            Render();
        }

        private void OnDied(IDamager killer)
        {
            Render();
        }
    }
}
