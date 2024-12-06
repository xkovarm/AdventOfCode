namespace AdventOfCode.Day06;

internal class Puzzle : PuzzleBase
{
    // -- CALCULATION PARAMETERS AND RESULT ------------------------------------

    protected override string SampleResult => "6"; // PART 1: "41";

    // -------------------------------------------------------------------------

    protected override string DoCalculation(string input)
    {
        var (map, guard, width, height) = ProcessInput(input);

        // PART 1:
        //var count = TraceOpenPath(map, guard).Count;

        var count = 0;

        var path = TraceOpenPath(map, guard);
        path.Remove(guard);

        foreach (var coord in path)
        {
            var copy = map.ToHashSet();
            // add an obstacle
            copy.Add(coord);

            count += TraceOpenPath(copy, guard) is null ? 1 : 0;
        }

        return count.ToString();

        HashSet<Coord> TraceOpenPath(HashSet<Coord> map, Coord guard)
        {
            var visited = new HashSet<CoordAndHeading>();

            var heading = '^';

            while (guard.X >= 0 && guard.Y >= 0 && guard.X < width && guard.Y < height)
            {
                var pos = new CoordAndHeading(guard, heading);

                if (visited.Contains(pos))
                {
                    // cycle detected, open path does not exist
                    return null;
                }

                visited.Add(new CoordAndHeading(guard, heading));

                // follow the path
                var following = heading switch
                {
                    '^' => guard with { Y = guard.Y - 1 },
                    'v' => guard with { Y = guard.Y + 1 },
                    '<' => guard with { X = guard.X - 1 },
                    '>' => guard with { X = guard.X + 1 },
                };

                // check an obstacle
                if (map.Contains(following))
                {
                    // turn right
                    heading = heading switch
                    {
                        '^' => '>',
                        '>' => 'v',
                        'v' => '<',
                        '<' => '^'
                    };

                    continue;
                }

                // no obstacle -> move forward
                guard = following;
            }

            return visited.Select(v => v.Coord).ToHashSet();
        }

    }

    private (HashSet<Coord> Map, Coord Guard, int Width, int Height) ProcessInput(string input)
    {
        var map = new HashSet<Coord>();
        var y = 0;
        var guard = default(Coord);

        var lines = input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).ToList();

        foreach (var line in lines)
        {
            var g = line.IndexOf('^');

            if (g != -1)
            {
                guard = new Coord(g, y);
            }

            map.UnionWith(line.AllIndexesOf('#').Select(i => new Coord(i, y)));

            y++;
        }

        return (map, guard, lines[0].Length, lines.Count);
    }

    private record Coord(int X, int Y);

    private record CoordAndHeading(Coord Coord, char Heading);
}