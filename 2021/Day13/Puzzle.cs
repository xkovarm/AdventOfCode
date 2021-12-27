namespace AdventOfCode.Day13
{
    internal class Puzzle : PuzzleBase
    {
        protected override string SampleResult => "16";

        protected override Task<string> GetSampleInputAsync()
        {
            return Task.FromResult(@"6,10
0,14
9,10
0,3
10,4
4,11
6,0
6,12
4,1
0,13
10,12
3,4
3,0
8,4
1,10
2,14
8,10
9,0

fold along y=7
fold along x=5");
        }

        protected override string DoCalculation(string input)
        {
            var lines = input
                .Split(Environment.NewLine);

            var divide = Array.IndexOf(lines, String.Empty);

            var points = lines
                .Take(divide)
                .Select(s => s.Split(','))
                .Select(s => (X: int.Parse(s[0]), Y: int.Parse(s[1])))
                .ToArray();

            var folds = lines
                .Skip(divide+1)
                .Where(l => !string.IsNullOrWhiteSpace(l))
                .Select(l => l.Replace("fold along", "")
                .Trim().Split('='))
                .Select(s => (Axis: s[0], Position: int.Parse(s[1])))
                .ToArray();

            var transformed = new HashSet<(int X, int Y)>();

            folds.ToList().ForEach(fold =>
            {
                transformed.Clear();

                points.ToList().ForEach(point =>
                {
                    if (fold.Axis == "x" && point.X > fold.Position)
                    {
                        transformed.Add(point with { X = fold.Position - (point.X - fold.Position) });
                    }
                    else if (fold.Axis == "y" && point.Y > fold.Position)
                    {
                        transformed.Add(point with { Y = fold.Position - (point.Y - fold.Position) });
                    }
                    else
                    {
                        transformed.Add(point);
                    }

                    points = transformed.Distinct().ToArray();
                });
            });

            DumpResult(points);

            return points.Count().ToString();

            void DumpResult((int X, int Y)[] points)
            {
                var maxX = points.Select(p => p.X).Max();
                var maxY = points.Select(p => p.Y).Max();

                var dump = new char[maxY + 1][];
                points.ToList().ForEach(p => 
                {
                    if (dump[p.Y] == null)
                    {
                        dump[p.Y] = Enumerable.Repeat(' ', maxX + 1).ToArray();
                    }
                    dump[p.Y][p.X] = '#';
                });

                dump.ToList().ForEach(d => Console.WriteLine(new string(d)));
            }
        }
    }
}
