using ConsoleRoguelike.AIBehaviour.Attack;
using ConsoleRoguelike.AIBehaviour.Walking;
using ConsoleRoguelike.CoreModule;
using ConsoleRoguelike.CreatureCondition;
using ConsoleRoguelike.GameScene;

namespace ConsoleRoguelike.GameObjects.AI
{
    internal class Bullet : LifelessEnemy, IDamager, IWalkingEnemy, IAttackingEnemy
    {
        public event Action<Bullet> Destroyed;

        private readonly AttackBehaviour _attackBehavior;
        public AttackBehaviour AttackBehaviour => _attackBehavior;

        private readonly WalkingBehaviour _walkingBehaviour;
        public WalkingBehaviour WalkingBehaviour => _walkingBehaviour;

        private float _damageValue = 0;
        private int _maxLifeTime = 0;

        private int _lifeTime = 0;
        private bool _isDestroyed = false;

        public string DeathReason => "KILLED BY SHOOTER'S BULLET";

        public Bullet(
            Vector2Int position,
            Vector2Int moveDirection,
            IReadOnlyScene scene, 
            SceneLayer sceneLayer,
            float damageValue = 50f, 
            int maxLifeTime = 10,
            char renderedChar = '☼',
            ConsoleColor color = ConsoleColor.Red) : base(position, renderedChar, scene, sceneLayer, color)
        {
            _damageValue = damageValue;
            _maxLifeTime = maxLifeTime;

            _walkingBehaviour = new DirectionalWalkingBehaviour(this, moveDirection);
            _attackBehavior = new AttackOnCollisionEnterBehaviour(this, this, _damageValue);
        }

        public override void MakeNextStep()
        {
            if (TryDestroyIfLifeTimeIsOver() == false)
            {
                Attack();
                MoveNextPosition();
                Attack();
            }
        }

        public void Attack()
        {
            if (_isDestroyed == false)
            {
                if (_attackBehavior.TryAttack())
                {
                    Destroy();
                }
            }
        }

        public void MoveNextPosition()
        {
            if (_isDestroyed == false)
            {
                Vector2Int nextMovePosition = _walkingBehaviour.GetNextMovePosition();
                if (CanMove(nextMovePosition))
                {
                    MoveTo(nextMovePosition);
                    _lifeTime++;
                }
                else
                {
                    Destroy();
                }
            }
        }

        private bool TryDestroyIfLifeTimeIsOver()
        {
            if (_lifeTime >= _maxLifeTime)
            {
                Destroy();
                return true;
            }

            return false;
        }

        private void Destroy()
        {
            if (_isDestroyed == false)
            {
                _isDestroyed = true;

                Deinitialize();

                Destroyed?.Invoke(this);
            }
        }
    }
}
