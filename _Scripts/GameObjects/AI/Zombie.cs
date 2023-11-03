using ConsoleRoguelike.CreatureCondition;
using ConsoleRoguelike.GameScene;

namespace ConsoleRoguelike.GameObjects.AI
{
    internal class Zombie : AliveEnemy, IDamager
    {
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
        }

        public string DeathReason => "KILLED BY ZOMBIE";

        public override void MakeNextStep()
        {
            if (TryToAttack())
            {
                MoveNext();
            }
            else
            {
                MoveNext();
                TryToAttack();
            }          
        }

        public override bool TryToAttack()
        {
            List<GameObject> gameObjects = SceneLayer.GetGameObjectsOnPosition(Position);
            for (int i = 0; i < gameObjects.Count; i++)
            {
                if (gameObjects[i] is Player playerGameObject)
                {
                    playerGameObject.Health.Damage(_damageValue, this);
                    return true;
                }
            }

            return false;
        }
    }
}
