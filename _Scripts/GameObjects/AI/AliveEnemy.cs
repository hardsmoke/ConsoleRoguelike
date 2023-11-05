using ConsoleRoguelike.AIBehaviour.Walking;
using ConsoleRoguelike.CoreModule;
using ConsoleRoguelike.GameScene;
using ConsoleRoguelike.MapGeneration;

namespace ConsoleRoguelike.GameObjects.AI
{
    internal abstract class AliveEnemy : Enemy, IWalkingEnemy
    {
        private WalkingBehaviour _walkingBehaviour;
        public WalkingBehaviour WalkingBehaviour => _walkingBehaviour;

        private readonly MazeMemory _mazeMemory;
        protected IReadOnlyMazeMemory MazeMemory => _mazeMemory;

        protected AliveEnemy(
            Vector2Int position, 
            char renderedChar, 
            IReadOnlyScene scene, 
            SceneLayer sceneLayer, 
            int mazeMemoryCapacity, 
            ConsoleColor color = ConsoleColor.Red) : base(position, renderedChar, scene, sceneLayer, color)
        {
            _mazeMemory = new MazeMemory(mazeMemoryCapacity);
            _walkingBehaviour = new WalkingInMazeBehaviour(_mazeMemory, this);
        }

        public void MoveNextPosition()
        {
            Vector2Int nextMovePosition = WalkingBehaviour.GetNextMovePosition();
            MoveTo(nextMovePosition);
        }
    }
}
