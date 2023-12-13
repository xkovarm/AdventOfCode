namespace AdventOfCode.Day13;

internal class Puzzle : PuzzleBase
{
    // -- CALCULATION PARAMETERS AND RESULT ------------------------------------

    protected override string SampleResult => "400"; // PART 1: "405";

    // -------------------------------------------------------------------------

    protected override string DoCalculation(string input)
    {
        var result =
            input.Split($"{Environment.NewLine}{Environment.NewLine}", StringSplitOptions.RemoveEmptyEntries)
            .Select(ParsePattern)
            //.Select(ProcessPattern_Part1)
            .Select(ProcessPattern_Part2)
            .Select(r => r.X + 100 * r.Y)
            .Sum();

        return result.ToString();
    }

    private Coord ProcessPattern_Part1(IEnumerable<Coord> pattern)
    {
        return FindReflection(pattern)!;
    }

    private Coord ProcessPattern_Part2(IEnumerable<Coord> pattern)
    {
        var original = pattern.ToHashSet();

        var maxX = original.Select(o => o.X).Max();
        var maxY = original.Select(o => o.Y).Max();

        var originalReflection = FindReflection(original);

        for (int x = 1; x <= maxX; x++)
        {
            for (int y = 1; y <= maxY; y++)
            {
                var clone = original.ToHashSet();
                var current = new Coord(x, y);

                if (clone.Contains(current))
                {
                    clone.Remove(current);
                }
                else
                {
                    clone.Add(current);
                }

                var reflection = FindReflection(clone, originalReflection);

                if (reflection is not null)
                {
                    return reflection;
                }
            }
        }

        throw new InvalidOperationException();
    }

    Coord? FindReflection(IEnumerable<Coord> pattern, Coord? originalReflection = null)
    {
        var maxX = pattern.Select(x => x.X).Max();
        var maxY = pattern.Select(y => y.Y).Max();

        for (var x = 1; x < maxX; x++)
        {
            var left = pattern
                .Where(p => p.X <= x && p.X > x - Math.Min(x, maxX - x)); // trim to the equal width
            
            var right = pattern
                .Where(p => p.X > x)
                .Select(p => p with { X = 2 * x - p.X + 1})
                .Where(p => p.X > 0); // mirror and trim

            if (left.Count() != right.Count())
            {
                continue;
            }

            var intersection = left.Intersect(right);

            if (left.Count() == intersection.Count() && (originalReflection == null || x != originalReflection.X))
            {
                return new Coord(x, 0);
            }
        }

        for (var y = 1; y < maxY; y++)
        {
            var top = pattern
                .Where(p => p.Y <= y && p.Y > y - Math.Min(y, maxY - y)); // trim to the equal height

            var bottom = pattern
                .Where(p => p.Y > y)
                .Select(p => p with { Y = 2 * y - p.Y + 1 })
                .Where(p => p.Y > 0); // mirror and trim

            if (top.Count() != bottom.Count())
            {
                continue;
            }

            var intersection = top.Intersect(bottom);
            
            if (top.Count() == intersection.Count() && (originalReflection == null || y != originalReflection.Y))
            {
                return new Coord(0, y);
            }
        }

        return null;
    }

    private IEnumerable<Coord> ParsePattern(string pattern)
    {
        var lines = pattern.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

        var y = 1;
        foreach (var line in lines)
        {
            var x = 1;
            foreach (var ch in line)
            {
                if (ch == '#')
                {
                    yield return new Coord(x, y);
                }
                x++;
            }
            y++;
        }
    }

    private record Coord(int X, int Y);
}
