namespace AdventOfCode.Day04;

internal class Puzzle : PuzzleBase
{
    // -- CALCULATION PARAMETERS AND RESULT ------------------------------------

    protected override string SampleResult => "30"; // PART 1: "13";

    // -------------------------------------------------------------------------

    protected override string DoCalculation(string input)
    {
        var cards = Parse(input);

        var points = cards
            .Select(c => new { c.Id, Count = c.MyNumbers.Intersect(c.CardNumbers).Count() })
            .ToDictionary(d => d.Id, d => d.Count);

        // PART 1
        // var result = points.Values.Where(m => m > 0).Select(m => Math.Pow(2, m - 1)).Sum();

        // PART 2
        var counts = points
            .ToDictionary(m => m.Key, m => 1);

        foreach (var i in points.Keys.Order())
        {
            for (int j = i + 1; j <= i + points[i]; j++)
            {
                counts[j] += counts[i];
            }
        }

        var result = counts.Values.Sum().ToString();

        return result.ToString();
    }

    private List<(int Id, HashSet<int> CardNumbers, HashSet<int> MyNumbers)> Parse(string input)
    {
        var parsed = input
            .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
            .Select(s => s.Split(':', '|'))
            .Select(s =>
                (int.Parse(s[0].Split(' ', StringSplitOptions.RemoveEmptyEntries)[1]),
                new HashSet<int>(s[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse)),
                new HashSet<int>(s[2].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse))))
            .ToList();

        return parsed;
    }
}
