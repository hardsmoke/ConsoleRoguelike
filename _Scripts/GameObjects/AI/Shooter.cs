using ConsoleRoguelike.GameScene;

namespace ConsoleRoguelike.GameObjects.AI
{
    internal class Shooter : AliveEnemy
    {
        public event Action<Bullet> Shooted;

        private int _stepsLeftToShoot = 0;
        private int _requiredStepsToShoot = 0;

        public Shooter(
            Vector2Int position, 
            IReadOnlyScene scene, 
            SceneLayer sceneLayer, 
            int mazeMemoryCapacity,
            char renderedChar = '♀',
            int stepsToShoot = 3) : base(position, renderedChar, scene, sceneLayer, mazeMemoryCapacity)
        {
            _requiredStepsToShoot = stepsToShoot;
        }

        public override bool TryToAttack()
        {
            if (_stepsLeftToShoot <= 0)
            {
                _stepsLeftToShoot = _requiredStepsToShoot;
                TryShoot(out _);
            }

            return true;
        }

        public override void MakeNextStep()
        {
            MoveNext();

            if (TryToAttack())
            {
                _stepsLeftToShoot--;
            }
        }

        public bool TryShoot(out Bullet? bullet)
        {
            if (MazeMemory.PreviousPositions.Count > 1)
            {
                Vector2Int previousPosition = MazeMemory.PreviousPositions[^2];
                Vector2Int shootDirection = previousPosition - Position;
                bullet = new Bullet(previousPosition, shootDirection, Scene, SceneLayer);
                Shooted?.Invoke(bullet);
                return true;
            }

            bullet = null;
            return false;
        }
    }
}
