using ConsoleRoguelike.CoreModule;
using ConsoleRoguelike.GameScene;

namespace ConsoleRoguelike.GameObjects.AI
{
    internal abstract class LifelessEnemy : Enemy
    {
        protected LifelessEnemy(
            Vector2Int position, 
            char renderedChar, 
            IReadOnlyScene scene, 
            SceneLayer sceneLayer, 
            ConsoleColor color = ConsoleColor.White) : base(position, renderedChar, scene, sceneLayer, color)
        {
        }
    }
}
