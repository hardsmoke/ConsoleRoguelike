using ConsoleRoguelike.CoreModule;
using ConsoleRoguelike.GameScene;

namespace ConsoleRoguelike.GameObjects
{
    internal class Wall : GameObject
    {
        public Wall(
            Vector2Int position, 
            IReadOnlyScene scene,
            SceneLayer sceneLayer,
            char renderedChar = '█') : base(position, renderedChar, scene, sceneLayer)
        {
        }
    }
}
