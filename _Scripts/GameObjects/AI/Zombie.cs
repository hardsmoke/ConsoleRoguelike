using ConsoleRoguelike.AIBehaviour.Attack;
using ConsoleRoguelike.CoreModule;
using ConsoleRoguelike.CreatureCondition;
using ConsoleRoguelike.GameScene;

namespace ConsoleRoguelike.GameObjects.AI
{
    internal class Zombie : AliveEnemy, IDamager, IAttackingEnemy
    {
        private AttackBehaviour _attackBehaviour;
        public AttackBehaviour AttackBehaviour => _attackBehaviour;

        private float _damageValue;

        public Zombie(
            Vector2Int position, 
            IReadOnlyScene scene, 
            SceneLayer sceneLayer, 
            int mazeMemoryCapacity,
            char renderedChar = '☺',
            float damageValue = 25f) : base(position, renderedChar, scene, sceneLayer, mazeMemoryCapacity)
        {
            _damageValue = damageValue;
            _attackBehaviour = new AttackOnCollisionEnterBehaviour(this, this, _damageValue);
        }

        public string DeathReason => "KILLED BY ZOMBIE";

        public override void MakeNextStep()
        {
            if (_attackBehaviour.TryAttack())
            {
                MoveNextPosition();
            }
            else
            {
                MoveNextPosition();
                _attackBehaviour.TryAttack();
            }          
        }
    }
}
