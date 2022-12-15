using System.Text.RegularExpressions;
using MoreLinq;

namespace AdventOfCode.Day15
{
    internal class Puzzle : PuzzleBase
    {
        // -- CALCULATION PARAMETERS AND RESULT ------------------------------------

        protected override string SampleResult => "56000011"; // PART 1: "26";

        // -------------------------------------------------------------------------

        protected override string DoCalculation(string input)
        {
            var records = input
                .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
                .Select(i => Regex.Matches(i, "-?[0-9]+").Select(m => int.Parse(m.Value)).ToArray())
                .ToList();

            var distances = records
                .Select(r => new { Sensor = new Point(r[0], r[1]), Beacon = new Point(r[2], r[3]) })
                .Select(r => (r.Sensor, Distance: r.Sensor.Distance(r.Beacon)))
                .ToList();

            bool isSample = records.Count == 14;  // hack

            // PART 1: var result = Calculate(distances, isSample ? 10 : 2000000);
            var result = Calculate2(distances, isSample ? 20 : 4000000);

            return result.ToString();
        }

        private long Calculate2(List<(Point Sensor, int Distance)> distances, int lines)
        {
            for (int y = 0; y < lines; y++)
            {
                var result = GetLines(distances, y);

                if (result.Count == 2)
                {
                    result = result.OrderBy(l => l.P1.X).ToList();

                    var x = result.Last().P1.Distance(result.First().P2) == 2 
                        ? result.First().P2.X + 1 
                        : throw new InvalidOperationException("something wrong");

                    return (long)x * 4000000 + y;
                }
            }

            return 0;
        }

        private int Calculate(List<(Point Sensor, int Distance)> distances, int y)
        {
            var lines = GetLines(distances, y);

            return lines.Sum(l => l.Length);
        }

        private IList<Line> GetLines(List<(Point Sensor, int Distance)> distances, int y)
        {
            var lines = distances
                .Select(d => new { d.Sensor, XDistance = d.Distance - Math.Abs(d.Sensor.Y - y) })
                .Where(d => d.XDistance >= 0)
                .Select(d => new Line(new Point(d.Sensor.X - d.XDistance, y), new Point(d.Sensor.X + d.XDistance, y)))
                .ToList();

            var joinedLines = new List<Line>();

            foreach (var line in lines)
            {
                var i = 0;
                var toBeJoined = line;

                while (i < joinedLines.Count())
                {
                    var union = toBeJoined.Union(joinedLines[i]);

                    if (union is not null)
                    {
                        joinedLines.RemoveAt(i);
                        toBeJoined = union;
                    }
                    else
                    {
                        i++;
                    }
                }
             
                joinedLines.Add(toBeJoined);
            }

            return joinedLines;
        }

        private record Point(int X, int Y)
        {
            public int Distance(Point point)
            {
                return Math.Abs(X - point.X) + Math.Abs(Y - point.Y);
            }
        }

        private record Line(Point P1, Point P2)
        {
            public Line? Union(Line l)
            {
                if (l.P2.X < P1.X || P2.X < l.P1.X)
                {
                    return null;
                }

                var x = new[] { P1.X, P2.X, l.P1.X, l.P2.X };

                return new Line(new Point(x.Min(), P1.Y), new Point(x.Max(), P1.Y));
            }

            public int Length => P1.Distance(P2);
        }
    }
}
