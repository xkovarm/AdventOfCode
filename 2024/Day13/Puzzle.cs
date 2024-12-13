using System.Text.RegularExpressions;

namespace AdventOfCode.Day13;

internal class Puzzle : PuzzleBase
{
    // -- CALCULATION PARAMETERS AND RESULT ------------------------------------

    protected override string SampleResult => "875318608908"; // "480";

    // -------------------------------------------------------------------------

    protected override string DoCalculation(string input)
    {
        var games = String.Join('|', input
            .Split(Environment.NewLine)
            .ToList())
            .Split("||")
            .Select(i => Regex.Matches(i, "[0-9]+"))
            //.Select(i => new Game(decimal.Parse(i[0].Value), decimal.Parse(i[1].Value), decimal.Parse(i[2].Value), decimal.Parse(i[3].Value), decimal.Parse(i[4].Value), decimal.Parse(i[5].Value)))
            .Select(i => new Game(decimal.Parse(i[0].Value), decimal.Parse(i[1].Value), decimal.Parse(i[2].Value), decimal.Parse(i[3].Value), 10000000000000 + decimal.Parse(i[4].Value), 10000000000000 + decimal.Parse(i[5].Value)))
            .ToList();

        var result = games.Select(g => Calculate(g /*, 100*/)).Sum();

        return result.ToString();

        decimal Calculate(Game game, decimal maxIterations = decimal.MaxValue)
        {
            decimal b = (game.AX * game.PY - game.AY * game.PX) / (game.AX * game.BY - game.AY * game.BX);

            decimal a = (game.PX - b * game.BX) / game.AX;

            if (a > maxIterations || b > maxIterations || a < 0 || b < 0 || Math.Floor(a) != a || Math.Floor(b) != b)
            {
                return 0;
            }

            return a * 3 + b;
        }
    }

    private record Game(decimal AX, decimal AY, decimal BX, decimal BY, decimal PX, decimal PY);
}
