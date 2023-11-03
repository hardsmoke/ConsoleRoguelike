namespace ConsoleRoguelike._Scripts.Extensions
{
    public static class IReadOnlyListExtensions
    {
        public static int IndexOf<T>(this IReadOnlyList<T> self, T elementToFind)
        {
            for (int i = 0; i < self.Count; i++)
                if (Equals(self[i], elementToFind))
                    return i;

            return -1;
        }
    }
}
