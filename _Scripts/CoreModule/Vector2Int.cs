using System.Collections;
using System.Numerics;

namespace ConsoleRoguelike.CoreModule
{
    internal struct Vector2Int : 
        IComparable<Vector2Int>, 
        IEquatable<Vector2Int>, 
        IStructuralEquatable,
        IAdditionOperators<Vector2Int, Vector2Int, Vector2Int>, 
        ISubtractionOperators<Vector2Int, Vector2Int, Vector2Int>,
        IMultiplyOperators<Vector2Int, Vector2Int, Vector2Int>,
        IDivisionOperators<Vector2Int, int, Vector2Int>,
        IUnaryNegationOperators<Vector2Int, Vector2Int>,
        IEqualityOperators<Vector2Int, Vector2Int, bool>,
        IComparisonOperators<Vector2Int, Vector2Int, bool>
    {
        public int X;
        public int Y;

        public Vector2Int(int x, int y)
        {
            X = x;
            Y = y;
        }

        public readonly double Magnitude => Math.Sqrt(X * X + Y * Y);

        public readonly int CompareTo(Vector2Int other)
        {
            return (int)Magnitude - (int)other.Magnitude;
        }

        public readonly bool Equals(Vector2Int other)
        {
            return this == other;
        }

        public readonly bool Equals(object? other, IEqualityComparer comparer)
        {
            return comparer.Equals(other);
        }

        public readonly int GetHashCode(IEqualityComparer comparer)
        {
            return comparer.GetHashCode();
        }

        public static Vector2Int Random(int xMinExclusive, int yMinExclusive, int xMaxExclusive, int yMaxExclusive)
        {
            return new Vector2Int()
            {
                X = xMinExclusive + System.Random.Shared.Next(xMaxExclusive - xMinExclusive),
                Y = yMinExclusive + System.Random.Shared.Next(yMaxExclusive - yMinExclusive),
            };
        }

        public static Vector2Int Random(Vector2Int minExsclusive, Vector2Int maxExsclusive)
        {
            return Random(minExsclusive.X, minExsclusive.Y, maxExsclusive.X, maxExsclusive.Y);
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
            return first == second == false;
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

        public static bool operator >=(Vector2Int first, Vector2Int second)
        {
            return first.X >= second.X && first.Y >= second.Y;
        }

        public static bool operator <=(Vector2Int first, Vector2Int second)
        {
            return first >= second == false;
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
