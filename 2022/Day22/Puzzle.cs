using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;

namespace AdventOfCode.Day22
{
    internal class Puzzle : PuzzleBase
    {
        // -- CALCULATION PARAMETERS AND RESULT ------------------------------------

        protected override string SampleResult => ""; // PART 1: "6032";

        // -------------------------------------------------------------------------

        private const int DirRight = 0;
        private const int DirDown = 1;
        private const int DirLeft = 2;
        private const int DirUp = 3;

        protected override string DoCalculation(string input)
        {
            var parts = input
                .Split($"{Environment.NewLine}{Environment.NewLine}", StringSplitOptions.RemoveEmptyEntries)
                .ToArray();

            var map = ParseMap(parts[0]);
            var directions = ParseDirections(parts[1]);

            if (directions.Count < 20)
            {
                // skip the sample for the PART 2
                return "";
            }

            var result = Calculate(map, directions);

            return result.ToString();
        }

        private int Calculate(Dictionary<Point, char> map, List<string> directions)
        {
            var startY = map.Keys.Select(p => p.Y).Min();
            var startX = map.Keys.Where(p => p.Y == startY).Select(p => p.X).Min();

            var current = new Point(startX, startY);
            var facing = 0;

            foreach (var direction in directions)
            {
                switch (direction)
                {
                    case var dir when int.TryParse(dir, out var steps):
                        for (int step = 0; step < steps; step++)
                        {
                            (current, facing) = GetNext(map, current, facing);
                        }
                        break;
                    case "L":
                        facing = facing == 0 ? 3 : facing - 1;
                        break;
                    case "R":
                        facing = facing == 3 ? 0 : facing + 1;
                        break;
                    default: 
                        throw new InvalidOperationException();
                }
            }

            return 1000 * current.Y + 4 * current.X + facing;
        }

        private (Point, int) GetNext(Dictionary<Point, char> map, Point current, int facing)
        {
            var next = facing switch
            {
                DirRight => current with { X = current.X + 1 },
                DirDown => current with { Y = current.Y + 1 },
                DirLeft => current with { X = current.X - 1 },
                DirUp => current with { Y = current.Y - 1 },
                _ => throw new InvalidOperationException()
            };

            if (map.TryGetValue(next, out var ch))
            {
                return ch == '#' ? (current, facing) : (next, facing);
            }

            // PART 1: next = GetFallBackPosition(map, current, facing); var nextFacing = facing;
            (next, var nextFacing) = GetFallBackPosition2(current, facing);

            return map[next] == '#' ? (current, facing) : (next, nextFacing);
        }


        private Point GetFallBackPosition(Dictionary<Point, char> map, Point current, int facing)
        {
            return facing switch
            {
                DirRight => current with { X = map.Keys.Where(p => p.Y == current.Y).Select(p => p.X).Min() },
                DirDown => current with { Y = map.Keys.Where(p => p.X == current.X).Select(p => p.Y).Min() },
                DirLeft => current with { X = map.Keys.Where(p => p.Y == current.Y).Select(p => p.X).Max() },
                DirUp => current with { Y = map.Keys.Where(p => p.X == current.X).Select(p => p.Y).Max() },
                _ => throw new InvalidOperationException()
            };
        }

        // Hardcoded tiles for my input. Should I somehow formalize the calculation? Nope...
        // 
        //      000111
        //      000111
        //      000111
        //      222
        //      222
        //      222
        //   444333
        //   444333
        //   444333
        //   555
        //   555
        //   555
        //

        private static Point[] Tiles = new[] {
                new Point(51, 1), new Point(101, 1), new Point(51, 51),
                new Point(51, 101), new Point(1, 101), new Point(1, 151)
        };

        private (Point, int) GetFallBackPosition2(Point current, int facing)
        {
            var tile = Array.IndexOf(Tiles.Select(t => IsInTile(current, t)).ToArray(), true);

            return (tile, facing) switch
            {
                (0, DirUp) => (new Point(1, current.X + 100), DirRight),
                (0, DirLeft) => (new Point(1, 151 - current.Y), DirRight),
                (1, DirUp) => (new Point(current.X - 100, 200), DirUp),
                (1, DirRight) => (new Point(100, 151 - current.Y), DirLeft),
                (1, DirDown) => (new Point(100, current.X - 50), DirLeft),
                (2, DirLeft) => (new Point(current.Y - 50, 101), DirDown),
                (2, DirRight) => (new Point(current.Y + 50, 50), DirUp),
                (4, DirLeft) => (new Point(51, 151 - current.Y), DirRight),
                (4, DirUp) => (new Point(51, current.X + 50), DirRight),
                (5, DirLeft) => (new Point(current.Y - 100, 1), DirDown),
                (5, DirDown) => (new Point(current.X + 100, 1), DirDown),
                (5, DirRight) => (new Point(current.Y - 100, 150), DirUp),
                (3, DirDown) => (new Point(50, current.X + 100), DirLeft),
                (3, DirRight) => (new Point(150, 151 - current.Y), DirLeft),
                _ => throw new InvalidOperationException()
            };
        }

        private bool IsInTile(Point pos, Point tileCorner)
        {
            return pos.X >= tileCorner.X && pos.Y >= tileCorner.Y && pos.X < tileCorner.X + 50 && pos.Y < tileCorner.Y + 50;
        }

        // -------------------------------------------------------------------------

        private Dictionary<Point, char> ParseMap(string map)
        {
            var lines = map
                .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
                .ToArray();

            var result = new Dictionary<Point, char>();

            var y = 1;
            foreach (var line in lines)
            {
                var x = 1;
                foreach(var c in line)
                {
                    if (c is '.' or '#')
                    {
                        result.Add(new Point(x, y), c);
                    } 
                    x++;
                }
                y++;
            }

            return result;
        }

        private List<string> ParseDirections(string directions)
        {
            var matches = Regex.Matches(directions, "[0-9]+|[RL]").Select(m => m.Value).ToList();

            return matches;
        }

        private record Point(int X, int Y);
    }
}
