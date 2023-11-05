using ConsoleRoguelike._Scripts.AIBehaviour.Attack;
using ConsoleRoguelike.AIBehaviour.Attack;
using ConsoleRoguelike.CoreModule;
using ConsoleRoguelike.GameScene;

namespace ConsoleRoguelike.GameObjects.AI
{
    internal class Shooter : AliveEnemy, IAttackingEnemy
    {
        public event Action<Bullet> Shooted;

        private readonly AttackBehaviour _attackBehaviour;
        public AttackBehaviour AttackBehaviour => _attackBehaviour;

        private int _stepsLeftToShoot = 0;
        private int _requiredStepsToShoot = 0;

        public Shooter(
            Vector2Int position, 
            IReadOnlyScene scene, 
            SceneLayer sceneLayer, 
            int mazeMemoryCapacity,
            char renderedChar = '♀',
            float bulletDamage = 50,
            int stepsToShoot = 3) : base(position, renderedChar, scene, sceneLayer, mazeMemoryCapacity)
        {
            _attackBehaviour = new ShootAttackBehaviour(this, MazeMemory, bulletDamage);

            ShootAttackBehaviour shootAttackBehaviour = (ShootAttackBehaviour)_attackBehaviour;
            shootAttackBehaviour.Shooted += OnShooted;

            _requiredStepsToShoot = stepsToShoot;
        }

        public override void MakeNextStep()
        {
            MoveNextPosition();
            _stepsLeftToShoot--;

            if (_stepsLeftToShoot <= 0)
            {
                _stepsLeftToShoot = _requiredStepsToShoot;
                _attackBehaviour.TryAttack();
            }
        }

        private void OnShooted(Bullet bullet)
        {
            Shooted?.Invoke(bullet);
        }
    }
}
