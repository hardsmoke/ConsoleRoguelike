using ConsoleRoguelike.CoreModule;

namespace ConsoleRoguelike.AIBehaviour.Walking
{
    internal abstract class WalkingBehaviour
    {
        public abstract Vector2Int GetNextMovePosition();
    }
}
