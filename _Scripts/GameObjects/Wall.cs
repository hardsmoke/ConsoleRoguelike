namespace ConsoleRoguelike.GameObjects
{
    using GameScene;

    internal class Wall : GameObject
    {
        public Wall(Vector2Int position, char renderedChar, IReadOnlyScene scene, SceneLayer sceneLayer) : base(position, renderedChar, scene, sceneLayer)
        {
        }
    }
}
