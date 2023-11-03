using ConsoleRoguelike.CreatureCondition;
using System.Text;

namespace ConsoleRoguelike.Render
{
    internal class HealthBarRenderer : IConsoleRenderer
    {
        private Health _health;
        private int _barWidth = 0;
        private ConsoleColor _color;

        private Vector2Int _startRenderPosition;
        public Vector2Int StartRenderPosition { get => _startRenderPosition; set => _startRenderPosition = value; }

        public HealthBarRenderer(Health health, ConsoleColor color = ConsoleColor.White)
        {
            _health = health;
            _color = color;

            _health.OnHealthChange += OnHealthChanged;
            _health.OnDie += OnDied;
        }

        public void ClearHealthBar()
        {
            Console.SetCursorPosition(_startRenderPosition.X, _startRenderPosition.Y);
            StringBuilder builder = new StringBuilder();
            builder.Append(' ', _barWidth);
            Console.WriteLine($"{builder}");
        }

        public void RenderHealthBar()
        {
            Console.ForegroundColor = _color;
            ClearHealthBar();
            Console.SetCursorPosition(_startRenderPosition.X, _startRenderPosition.Y);
            string healthBarText = $"Health: {_health.Value}%";
            _barWidth = healthBarText.Length;
            Console.Write(healthBarText);
        }

        public void RenderDeathScreen(IDamager killer)
        {
            ClearHealthBar();
            Console.SetCursorPosition(_startRenderPosition.X, _startRenderPosition.Y);
            string deathScreenText = $"{killer.DeathReason} | PRESS F TO PAY RESPECT";
            _barWidth = deathScreenText.Length;
            Console.Write(deathScreenText);
        }

        public void Render()
        {
            if (_health.Value > 0)
            {
                RenderHealthBar();
            }
        }

        public Vector2Int GetBottomLeftRenderedPosition()
        {
            return _startRenderPosition;
        }

        private void OnHealthChanged(float changeDelta)
        {
            Render();
        }

        private void OnDied(IDamager killer)
        {
            RenderDeathScreen(killer);
        }
    }
}
