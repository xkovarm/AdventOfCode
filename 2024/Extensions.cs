namespace AdventOfCode;
internal static class Extensions
{
    public static long LeastCommonMultiple(this IEnumerable<long> source)
    {
        return source.Aggregate(LeastCommonMultiple);
    }

    public static IEnumerable<int> AllIndexesOf(this string str, char ch)
    {
        int minIndex = str.IndexOf(ch);
        while (minIndex != -1)
        {
            yield return minIndex;
            minIndex = str.IndexOf(ch, minIndex + 1);
        }
    }

    private static long LeastCommonMultiple(long a, long b)
    {
        return a * b / GreatestCommonDivisor(a, b);
    }

    private static long GreatestCommonDivisor(long a, long b)
    {
        while (b != 0)
        {
            (a, b) = (b, a % b);
        }

        return a;
    }
}
