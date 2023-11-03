namespace ConsoleRoguelike.GameObjects
{
    using ConsoleRoguelike.CreatureCondition;
    using GameScene;

    internal class Player : GameObject
    {
        private Health _health;
        public Health Health => _health;

        private SceneLayer _disabledCollisionSceneLayer;
        private SceneLayer _enabledCollisionSceneLayer;

        public bool IsCollisionEnabled() => SceneLayer == _enabledCollisionSceneLayer;

        public Player(Vector2Int position, char ch, IReadOnlyScene scene, SceneLayer sceneLayer, SceneLayer disabledCollisionSceneLayer, float health = 100, ConsoleColor color = ConsoleColor.Blue) : base(position, ch, scene, sceneLayer, color)
        {
            _enabledCollisionSceneLayer = sceneLayer;
            _disabledCollisionSceneLayer = disabledCollisionSceneLayer;

            _health = new Health(health, health);
        }

        public void DisableCollision()
        {
            ChangeSceneLayer(_disabledCollisionSceneLayer);
        }

        public void EnableCollision()
        {
            ChangeSceneLayer(_enabledCollisionSceneLayer);
        }

        public void SwitchCollisionMode()
        {
            if (IsCollisionEnabled())
            {
                DisableCollision();
            }
            else
            {
                EnableCollision();
            }
        }
    }
}
