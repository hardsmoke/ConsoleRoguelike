using ConsoleRoguelike.CreatureCondition;

namespace ConsoleRoguelike.Render
{
    internal interface IReadOnlyHealthBarRenderer : IConsoleLineRenderer
    {
        public IReadOnlyHealth Health { get; }
    }
}
