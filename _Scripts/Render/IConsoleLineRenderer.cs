using System.Text;

namespace ConsoleRoguelike.Render
{
    internal interface IConsoleLineRenderer : IConsoleRenderer
    {
        public int Width { get; }
    }
}
