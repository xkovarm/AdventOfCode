namespace AdventOfCode.Day06;

internal class Puzzle : PuzzleBase
{
    // -- CALCULATION PARAMETERS AND RESULT ------------------------------------

    protected override string SampleResult => "71503"; // PART 1: "288";

    // -------------------------------------------------------------------------

    protected override string DoCalculation(string input)
    {
        // for PART 1 - comment the following line out
        input = input.Replace(" ", "");

        var races = Parse(input);
        var result = CalculateResults(races).Aggregate((a, b) => a * b);
        return result.ToString();
    }

    private IEnumerable<long> CalculateResults(IEnumerable<Race> races)
    {
        foreach (var race in races)
        {
            yield return CalculateDistances(race).Where(r => r > race.Distance).Count();
        }
    }

    private IEnumerable<long> CalculateDistances(Race race)
    {
        for (int i = 0; i < race.Time; i++)
        {
            yield return (race.Time - i) * i;
        }
    }

    private IEnumerable<Race> Parse(string input)
    {
        var arr = input
            .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
            .Select(s => s.Split(':')[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray())
            .ToArray();

        return arr[0].Zip(arr[1], (a, b) => new Race(a, b));
    }

    private record Race(long Time, long Distance);
}
