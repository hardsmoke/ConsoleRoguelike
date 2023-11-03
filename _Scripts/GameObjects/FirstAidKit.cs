using ConsoleRoguelike.CreatureCondition;
using ConsoleRoguelike.GameScene;

namespace ConsoleRoguelike.GameObjects
{
    internal class FirstAidKit : GameObject, IHealer
    {
        public event Action<FirstAidKit> Healed;

        private float _healValue = 0;

        public FirstAidKit(
            Vector2Int position, 
            IReadOnlyScene scene, 
            SceneLayer sceneLayer, 
            float healValue = 100, 
            char renderedChar = '♥', 
            ConsoleColor color = ConsoleColor.Green) : base(position, renderedChar, scene, sceneLayer, color)
        {
            _healValue = healValue;
        }

        public override void OnCollision(GameObject collideGameObject)
        {
            if (collideGameObject is Player player)
            {
                Heal(player);
            }

            base.OnCollision(collideGameObject);
        }

        public void Heal(Player player)
        {
            player.Health.Heal(_healValue, this);
            Healed?.Invoke(this);
            Deinitialize();
        }
    }
}
