namespace AdventOfCode.Day11;

internal class Puzzle : PuzzleBase
{
    // -- CALCULATION PARAMETERS AND RESULT ------------------------------------

    protected override string SampleResult => "65601038650482"; //"55312";

    protected override Task<string> GetSampleInputAsync() => Task.FromResult("125 17");

    // -------------------------------------------------------------------------

    protected override string DoCalculation(string input)
    {
        var stones = input
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Select(long.Parse)
            .ToList();

        //var result = Calculate(stones, 25);
        var result = Calculate(stones, 75);

        return result.ToString();
    }

    long Calculate(List<long> stones, int iterations)
    {
        var source = new Dictionary<long, long>();
        var work = new Dictionary<long, long>();

        stones.ForEach(s => source.Add(s, 1));

        for (int i = 0; i < iterations; i++)
        {
            work.Clear();

            foreach (var stone in source)
            {
                if (stone.Key == 0)
                {
                    Add(work, 1, stone.Value);
                }
                else if (stone.Key.ToString().Length % 2 == 0)
                {
                    var s = stone.Key.ToString();

                    Add(work, long.Parse(s.Substring(0, s.Length / 2)), stone.Value);
                    Add(work, long.Parse(s.Substring(s.Length / 2)), stone.Value);
                }
                else
                {
                    Add(work, stone.Key * 2024, stone.Value);
                }
            }

            (source, work) = (work, source);
        }

        return source.Values.Sum();

        void Add(Dictionary<long, long> d, long key, long value)
        {
            d[key] = value + (d.TryGetValue(key, out var result) ? result : 0);
        }
    }
}
