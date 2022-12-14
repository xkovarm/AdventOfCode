using MoreLinq.Extensions;

namespace AdventOfCode.Day14
{
    internal class Puzzle : PuzzleBase
    {
        // -- CALCULATION PARAMETERS AND RESULT ------------------------------------

        protected override string SampleResult => "93"; // PART 1: "24";

        // -------------------------------------------------------------------------

        protected override string DoCalculation(string input)
        {
            var lines = input
                .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
                .Distinct()
                .Select(s => s.Split("->"))
                .Select(s => s.Select(s => s.Trim().Split(",")).Select(s => (int.Parse(s[0]), int.Parse(s[1]))).ToList())
                .Distinct()
                .ToList();

            var result = Calculate(lines);

            return result.ToString();
        }

        int Calculate(List<List<(int X, int Y)>> lines)        
        {
            var source = (500, 0);
            var maxY = lines.SelectMany(l => l.Select(q => q.Y)).Max() + 2; // PART 1: + 0

            var scene = new HashSet<(int X, int Y)>();

            foreach (var points in lines)
            {
                for (int i = 0; i < points.Count-1; i++)
                {
                    PlaceRocks(points[i], points[i + 1]);
                }
            }

            int count = 0;
            while (TraceSand(source, maxY))
            {
                count++;
            }

            return count;

            bool TraceSand((int X, int Y) point, int maxY)
            {
                var nullFollower = (X: -1, Y: -1);

                if (scene.Contains(point))
                {
                    return false;
                }

                while (true)
                {
                    var followers = new[] {
                        point with { Y = point.Y + 1 },
                        point with { X = point.X - 1, Y = point.Y + 1 },
                        point with { X = point.X + 1, Y = point.Y + 1 }
                    };

                    var follower = followers.FirstOrDefault(f => !scene.Contains(f), nullFollower);

                    if (follower == nullFollower)
                    {
                        scene.Add(point);
                        return true;
                    }


                    if (follower.Y >= maxY)
                    {
                        //PART 1: return false;

                        // PART 2: Handle the virtual infinite floor
                        scene.Add(point);
                        return true;
                    }

                    point = follower;
                }
            }

            void PlaceRocks((int X, int Y) from, (int X, int Y) to)
            {
                var current = from;
                var diff = (X: Math.Sign(to.X - from.X), Y: Math.Sign(to.Y - from.Y));

                scene.Add(current);

                while (current != to)
                {
                    current = (current.X + diff.X, current.Y + diff.Y);
                    scene.Add(current);
                }
            }
        }
    }
}
