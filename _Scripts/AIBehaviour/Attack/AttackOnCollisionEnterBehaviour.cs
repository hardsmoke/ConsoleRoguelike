using ConsoleRoguelike.CreatureCondition;
using ConsoleRoguelike.GameObjects;

namespace ConsoleRoguelike.AIBehaviour.Attack
{
    internal class AttackOnCollisionEnterBehaviour : AttackBehaviour
    {
        private readonly GameObject _damagerGameObject;
        private readonly IDamager _damager;
        private float _damage;

        public AttackOnCollisionEnterBehaviour(GameObject damagerGameObject, IDamager damager, float damage)
        {
            _damagerGameObject = damagerGameObject; 
            _damager = damager;
            _damage = damage;
        }

        public override bool TryAttack()
        {
            List<GameObject> gameObjects = _damagerGameObject.SceneLayer.GetGameObjectsOnPosition(_damagerGameObject.Position);
            for (int i = 0; i < gameObjects.Count; i++)
            {
                if (gameObjects[i] is Player playerGameObject)
                {
                    playerGameObject.Health.Damage(_damage, _damager);
                    return true;
                }
            }

            return false;
        }
    }
}
