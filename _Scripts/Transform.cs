namespace ConsoleRoguelike
{
    internal class Transform
    {
        /// <summary>
        /// <params>1st Transform - transform itself</params>
        /// <params>2nd Vector2Int - previous position</params>
        /// <params>3rd Vector2Int - new position</params>
        /// </summary>
        public event Action<Transform, Vector2Int, Vector2Int> PositionChanged;

        private Vector2Int _position;
        public Vector2Int Position
        {
            get => _position;
            set
            {
                if (CanMove(value) == false)
                    return;

                Vector2Int previousPosition = _position;
                _position = value;

                OnPositionChanged();
                PositionChanged?.Invoke(this, previousPosition, _position);
            }
        }

        public Transform(Vector2Int initPosition)
        {
            _position = initPosition;
        }

        public void MoveOn(Vector2Int originPositionOffset)
        {
            Position += originPositionOffset;
        }

        public void MoveTo(Vector2Int newPosition)
        {
            Position = newPosition;
        }

        public void MoveUp() => MoveOn(Vector2Int.Down);
        public void MoveDown() => MoveOn(Vector2Int.Up);
        public void MoveLeft() => MoveOn(Vector2Int.Left);
        public void MoveRight() => MoveOn(Vector2Int.Right);

        public virtual bool CanMove(Vector2Int toMove)
        {
            if (toMove.X < 0 || toMove.Y < 0)
            {
                return false;
            }

            return true;
        }

        public virtual void OnPositionChanged() { }
    }
}
