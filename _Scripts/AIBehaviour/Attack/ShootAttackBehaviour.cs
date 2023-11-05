using ConsoleRoguelike.AIBehaviour.Attack;
using ConsoleRoguelike.CoreModule;
using ConsoleRoguelike.GameObjects;
using ConsoleRoguelike.GameObjects.AI;
using ConsoleRoguelike.MapGeneration;

namespace ConsoleRoguelike._Scripts.AIBehaviour.Attack
{
    internal class ShootAttackBehaviour : AttackBehaviour
    {
        public event Action<Bullet> Shooted;

        private readonly GameObject _gameObject;
        private readonly IReadOnlyMazeMemory _mazeMemory;
        private float _bulletDamage;

        public ShootAttackBehaviour(GameObject damagerGameObject, IReadOnlyMazeMemory mazeMemory, float bulletDamage)
        {
            _gameObject = damagerGameObject;
            _mazeMemory = mazeMemory;
            _bulletDamage = bulletDamage;
        }

        public override bool TryAttack()
        {
            if (_mazeMemory.PreviousPositions.Count > 1)
            {
                Shoot(GetPreviousPosition(), GetShootDirection(), out _);
                return true;
            }
            else
            {
                return false;
            }
        }

        private Vector2Int GetPreviousPosition()
        {
            return _mazeMemory.PreviousPositions[^2];
        }

        private Vector2Int GetShootDirection()
        {
            if (_mazeMemory.PreviousPositions.Count > 1)
            {
                Vector2Int previousPosition = GetPreviousPosition();
                Vector2Int shootDirection = previousPosition - _gameObject.Position;

                return shootDirection;
            }

            return Vector2Int.Zero;
        }

        private void Shoot(Vector2Int previousPosition, Vector2Int direction, out Bullet? bullet)
        {
            bullet = new Bullet(previousPosition, direction, _gameObject.Scene, _gameObject.SceneLayer, _bulletDamage);
            Shooted?.Invoke(bullet);
        }
    }
}
