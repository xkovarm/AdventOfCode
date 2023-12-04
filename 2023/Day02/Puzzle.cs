namespace AdventOfCode.Day02;

internal class Puzzle : PuzzleBase
{
    // -- CALCULATION PARAMETERS AND RESULT ------------------------------------

    protected override string SampleResult => "2286"; // PART 1: "8";

    // -------------------------------------------------------------------------

    protected override string DoCalculation(string input)
    {
        var games = ParseFile(input);

        // PART 1
        //var result = games.Where(g => g.Games.All(a => a.Red <= 12 && a.Green <= 13 && a.Blue <= 14)).Sum(r => r.Id);

        // PART 2
        var result = games.Select(g => GetPower(g.Games)).Sum().ToString();

        return result.ToString();

        long GetPower(IList<Game> games)
        {
            var maxRed = games.Select(g => g.Red).Max();
            var maxGreen = games.Select(g => g.Green).Max();
            var maxBlue = games.Select(g => g.Blue).Max();

            return maxRed * maxGreen * maxBlue;
        }
    }

    private IList<Config> ParseFile(string file)
    {
        var games = file
            .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
            .Select(a => a.Split([':', ';']))
            .Select(a => new Config(
                int.Parse(a[0].Split(' ')[1]), a[1..].Select(ParseGame).ToList()))
            .ToList();

        return games;

        Game ParseGame(string s)
        {
            var cubes = s.Split(',')
                .Select(s => s.Trim())
                .Select(s => new
                {
                    Red = s.EndsWith("red") ? int.Parse(s.Split(' ')[0]) : 0,
                    Green = s.EndsWith("green") ? int.Parse(s.Split(' ')[0]) : 0,
                    Blue = s.EndsWith("blue") ? int.Parse(s.Split(' ')[0]) : 0
                });

            return new Game(cubes.Max(a => a.Red), cubes.Max(a => a.Green), cubes.Max(a => a.Blue));
        }
    }

    private record Config(int Id, IList<Game> Games);

    private record Game(int Red, int Blue, int Green);
}
