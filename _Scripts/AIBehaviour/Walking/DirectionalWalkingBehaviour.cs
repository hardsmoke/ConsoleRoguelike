using ConsoleRoguelike.CoreModule;

namespace ConsoleRoguelike.AIBehaviour.Walking
{
    internal class DirectionalWalkingBehaviour : WalkingBehaviour
    {
        private readonly IReadOnlyTransform _aiTransform;
        public Vector2Int MoveDirection = Vector2Int.Zero;

        public DirectionalWalkingBehaviour(IReadOnlyTransform AITransform)
        {
            _aiTransform = AITransform;
        }

        public DirectionalWalkingBehaviour(IReadOnlyTransform AITransform, Vector2Int moveDirection) : this(AITransform)
        {
            MoveDirection = moveDirection;
        }

        public override Vector2Int GetNextMovePosition()
        {
            return _aiTransform.Position += MoveDirection;
        }

        public bool CanMoveToDirection()
        {
            return _aiTransform.CanMove(_aiTransform.Position + MoveDirection);
        }
    }
}
