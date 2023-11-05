using ConsoleRoguelike.AIBehaviour.Walking;

namespace ConsoleRoguelike.GameObjects.AI
{
    internal interface IWalkingEnemy
    {
        public WalkingBehaviour WalkingBehaviour { get; }
    }
}
