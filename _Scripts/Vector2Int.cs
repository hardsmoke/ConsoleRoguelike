namespace ConsoleRoguelike
{
    internal struct Vector2Int
    {
        public int X;
        public int Y;

        public Vector2Int(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static Vector2Int Random(int xMinExclusive, int yMinExclusive, int xMaxExclusive, int yMaxExclusive)
        {
            Random rand = new Random();
            return new Vector2Int()
            {
                X = xMinExclusive + rand.Next(xMaxExclusive - xMinExclusive),
                Y = yMinExclusive + rand.Next(yMaxExclusive - yMinExclusive),
            };
        }

        public static Vector2Int Random(Vector2Int minEsclusive, Vector2Int maxEsclusive)
        {
            return Random(minEsclusive.X, minEsclusive.Y, maxEsclusive.X, maxEsclusive.Y);
        }

        public static bool IsFirstLowerSecond(Vector2Int first, Vector2Int second, bool orEquals = false)
        {
            return orEquals ? first.Y <= second.Y : first.Y < second.Y;
        }

        public static bool IsFirstHigherSecond(Vector2Int first, Vector2Int second, bool orEquals = false)
        {
            return orEquals ? first.Y >= second.Y : first.Y > second.Y;
        }

        public static bool IsFirstLefterSecond(Vector2Int first, Vector2Int second, bool orEquals = false)
        {
            return orEquals ? first.X <= second.X : first.X < second.X;
        }

        public static bool IsFirstRighterSecond(Vector2Int first, Vector2Int second, bool orEquals = false)
        {
            return orEquals ? first.X >= second.X : first.X > second.X;
        }

        public static Vector2Int Zero => new Vector2Int(0, 0);
        public static Vector2Int One => new Vector2Int(1, 1);
        public static Vector2Int Up => new Vector2Int(0, 1);
        public static Vector2Int Down => new Vector2Int(0, -1);
        public static Vector2Int Left => new Vector2Int(-1, 0);
        public static Vector2Int Right => new Vector2Int(1, 0);

        public static bool operator !=(Vector2Int first, Vector2Int second)
        {
            return (first == second) == false;
        }

        public static bool operator ==(Vector2Int first, Vector2Int second)
        {
            return first.X == second.X && first.Y == second.Y;
        }

        public static bool operator >(Vector2Int first, Vector2Int second)
        {
            return first.X > second.X && first.Y > second.Y;
        }

        public static bool operator <(Vector2Int first, Vector2Int second)
        {
            return first > second == false;
        }

        public static Vector2Int operator *(Vector2Int first, int num)
        {
            return new Vector2Int(first.X * num, first.Y * num);
        }

        public static Vector2Int operator /(Vector2Int first, int num)
        {
            return new Vector2Int(first.X / num, first.Y / num);
        }

        public static Vector2Int operator *(Vector2Int first, Vector2Int second)
        {
            return new Vector2Int(first.X * second.X, first.Y * second.Y);
        }

        public static Vector2Int operator +(Vector2Int first, Vector2Int second)
        {
            return new Vector2Int(first.X + second.X, first.Y + second.Y);
        }

        public static Vector2Int operator -(Vector2Int first, Vector2Int second)
        {
            return new Vector2Int(first.X - second.X, first.Y - second.Y);
        }

        public static Vector2Int operator -(Vector2Int first)
        {
            return new Vector2Int(-first.X, -first.Y);
        }
    }
}
