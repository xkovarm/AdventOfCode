namespace AdventOfCode.Day09;

internal class Puzzle : PuzzleBase
{
    // -- CALCULATION PARAMETERS AND RESULT ------------------------------------

    protected override string SampleResult => "2"; // PART 1: "114";

    // -------------------------------------------------------------------------

    protected override string DoCalculation(string input)
    {
        var arrays = Parse(input);

        var result = arrays.Select(Extrapolate).Sum();

        return result.ToString();
    }

    private long Extrapolate(List<long> list)
    {
        var values = new Stack<long>();

        while (!list.All(i => i == 0))
        {
            // PART 1: values.Push(list.Last());
            values.Push(list.First());
            Reduce(list);
        }

        // PART 1: return values.Sum();

        var result = 0L;
        while (values.Any())
        {
            result = values.Pop() - result;
        }

        return result;

        void Reduce(List<long> list)
        {
            for (int i = 0; i < list.Count - 1; i++)
            {
                list[i] = list[i + 1] - list[i];
            }

            list.RemoveAt(list.Count - 1);
        }
    }

    private List<List<long>> Parse(string input)
    {
        var lines = input
            .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
            .Select(l => l.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToList())
            .ToList();

        return lines;
    }
}
