using ConsoleRoguelike.CreatureCondition;
using ConsoleRoguelike.GameScene;

namespace ConsoleRoguelike.GameObjects.AI
{
    internal class Bullet : LifelessEnemy, IDamager
    {
        public event Action<Bullet> Destroyed;

        private float _damageValue = 0;
        private int _maxLifeTime = 0;
        private Vector2Int _moveDirection = Vector2Int.Zero;

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
            _moveDirection = moveDirection;
            _damageValue = damageValue;
            _maxLifeTime = maxLifeTime;
        }

        public override void MakeNextStep()
        {
            if (TryDestroyIfLifeTimeIsOver() == false)
            {
                TryToAttack();
                MoveNext();
                TryToAttack();
            }
        }

        public override bool TryToAttack()
        {
            if (_isDestroyed == false)
            {
                List<GameObject> gameObjects = SceneLayer.GetGameObjectsOnPosition(Position);
                for (int i = 0; i < gameObjects.Count; i++)
                {
                    if (gameObjects[i] is Player playerGameObject)
                    {
                        playerGameObject.Health.Damage(_damageValue, this);
                        Destroy();
                        return true;
                    }
                }
            }
            
            return false;
        }

        public override void MoveNext()
        {
            if (_isDestroyed == false)
            {
                if (CanMoveToDirection())
                {
                    Position += _moveDirection;
                    _lifeTime++;
                }
                else
                {
                    Destroy();
                }
            }
        }

        public bool CanMoveToDirection()
        {
            return CanMove(Position + _moveDirection);
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
