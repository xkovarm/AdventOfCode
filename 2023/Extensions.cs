namespace AdventOfCode;
internal static class Extensions
{
    public static long LeastCommonMultiple(this IEnumerable<long> source)
    {
        return source.Aggregate(LeastCommonMultiple);
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
