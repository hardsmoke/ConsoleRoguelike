using ConsoleRoguelike.AIBehaviour.Attack;

namespace ConsoleRoguelike.GameObjects.AI
{
    internal interface IAttackingEnemy
    {
        public AttackBehaviour AttackBehaviour { get; }
    }
}
