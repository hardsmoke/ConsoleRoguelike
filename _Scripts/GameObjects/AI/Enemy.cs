using ConsoleRoguelike.CoreModule;
using ConsoleRoguelike.GameScene;

namespace ConsoleRoguelike.GameObjects.AI
{
    internal abstract class Enemy : GameObject
    {
        public Enemy(
            Vector2Int position, 
            char renderedChar, 
            IReadOnlyScene scene, 
            SceneLayer sceneLayer, 
            ConsoleColor color = ConsoleColor.White) : base(position, renderedChar, scene, sceneLayer, color)
        {

        }

        public abstract void MakeNextStep();
    }
}
