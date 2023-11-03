using ConsoleRoguelike.GameScene;

namespace ConsoleRoguelike.GameObjects.AI
{
    internal abstract class Enemy : GameObject
    {
        protected Enemy(Vector2Int position, char renderedChar, IReadOnlyScene scene, SceneLayer sceneLayer, ConsoleColor color = ConsoleColor.White) : base(position, renderedChar, scene, sceneLayer, color)
        {
        }

        public abstract void MakeNextStep();

        public abstract bool TryToAttack();

        public abstract void MoveNext();

        public List<Vector2Int> GetPossibleToMovePositions()
        {
            List<Vector2Int> possibleToMovePositions = new List<Vector2Int>();

            Vector2Int topPosition = Position + Vector2Int.Down;
            Vector2Int bottomPosition = Position + Vector2Int.Up;
            Vector2Int leftPosition = Position + Vector2Int.Left;
            Vector2Int rightPosition = Position + Vector2Int.Right;

            if (CanMove(topPosition))
                possibleToMovePositions.Add(topPosition);

            if (CanMove(bottomPosition))
                possibleToMovePositions.Add(bottomPosition);

            if (CanMove(leftPosition))
                possibleToMovePositions.Add(leftPosition);

            if (CanMove(rightPosition))
                possibleToMovePositions.Add(rightPosition);

            return possibleToMovePositions;
        }
    }
}
