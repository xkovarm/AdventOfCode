namespace AdventOfCode.Day19
{
    internal class Puzzle : PuzzleBase
    {
        private const int MinimumOverlap = 12;

        // Manually created using a pencil and a paper. :)
        private readonly Transformation[] Transformations = new[]
        {
            // X & Y, positive Z
            new Transformation(1, 0, 0, 0, 1, 0, 0, 0, 1),
            new Transformation(0, -1, 0, 1, 0, 0, 0, 0, 1),
            new Transformation(-1, 0, 0, 0, -1, 0, 0, 0, 1),
            new Transformation(0, 1, 0, -1, 0, 0, 0, 0, 1),
            // X & Y, negative Z (flipped)
            new Transformation(1, 0, 0, 0, -1, 0, 0, 0, -1),
            new Transformation(0, -1, 0, -1, 0, 0, 0, 0, -1),
            new Transformation(-1, 0, 0, 0, 1, 0, 0, 0, -1),
            new Transformation(0, 1, 0, 1, 0, 0, 0, 0, -1),

            // Y & Z, positive X
            new Transformation(0, 0, 1, 1, 0, 0, 0, 1, 0),
            new Transformation(0, 0, 1, 0, -1, 0, 1, 0, 0),
            new Transformation(0, 0, 1, -1, 0, 0, 0, -1, 0),
            new Transformation(0, 0, 1, 0, 1, 0, -1, 0, 0),
            // Y & Z, negative X (flipped)
            new Transformation(0, 0, -1, 1, 0, 0, 0, -1, 0),
            new Transformation(0, 0, -1, 0, -1, 0, -1, 0, 0),
            new Transformation(0, 0, -1, -1, 0, 0, 0, 1, 0),
            new Transformation(0, 0, -1,  0, 1, 0, 1, 0, 0),

            // X & Z, positive Y
            new Transformation(0, 1, 0, 0, 0, 1, 1, 0, 0),
            new Transformation(1, 0, 0, 0, 0, 1, 0, -1, 0),
            new Transformation(0, -1, 0, 0, 0, 1, -1, 0, 0),
            new Transformation(-1, 0, 0, 0, 0, 1, 0, 1, 0),
            // X & Z, negative Y (flipped)
            new Transformation(0, -1, 0, 0, 0, -1, 1, 0, 0),
            new Transformation(-1, 0, 0, 0, 0, -1, 0, -1, 0),
            new Transformation(0, 1, 0, 0, 0, -1, -1, 0, 0),
            new Transformation(1, 0, 0, 0, 0, -1,  0, 1, 0)
        };

        protected override string SampleResult => "3621"; // "79"; 

        protected override Task<string> GetSampleInputAsync()
        {
            return File.ReadAllTextAsync(@"Day19\sample.txt");
        }

        protected override string DoCalculation(string input)
        {
            var scanners = ParseInput(input).ToHashSet();

            var current = new HashSet<Scanner>();
            var processed = new HashSet<Scanner>();

            current.Add(scanners.First());

            while (current.Count > 0)
            {
                var scanner = current.First();
                current.Remove(scanner);
                scanners.Remove(scanner);

                foreach (var scannerToCheck in scanners)
                {
                    if (Check(scanner, scannerToCheck))
                    {
                        if (!current.Contains(scannerToCheck))
                        {
                            current.Add(scannerToCheck);
                        }
                    }
                }

                processed.Add(scanner);
            }

            Debug.Assert(!scanners.Any());

            var result = new HashSet<Vector>();
            processed.ToList().ForEach(p => result.UnionWith(p.Vectors));

            //return result.Count.ToString();

            var maxDistance = processed.Select(s => processed.Select(ss => ss.Position.GetDistance(s.Position)).ToArray()).SelectMany(s => s).Max();

            return maxDistance.ToString();

            bool Check(Scanner baseScanner, Scanner scannerToCheck)
            {
                foreach (var vectorsToCheck in Transform(scannerToCheck.Vectors))
                {
                    foreach (var baseVector in baseScanner.Vectors)
                    {
                        foreach (var vectorToCheck in vectorsToCheck)
                        {
                            var offset = baseVector - vectorToCheck;

                            var translatedVectors = vectorsToCheck.Select(v => v + offset).ToHashSet();
                            var intersection = baseScanner.Vectors.Intersect(translatedVectors);

                            if (intersection.Count() >= MinimumOverlap)
                            {
                                scannerToCheck.Position = offset;
                                scannerToCheck.Vectors = translatedVectors;

                                return true;
                            }
                        }
                    }
                }

                return false;
            }

            IEnumerable<HashSet<Vector>> Transform(HashSet<Vector> vectors)
            {
                foreach (var transformation in Transformations)
                {
                    yield return vectors.Select(v => v * transformation).ToHashSet();
                }
            }
        }

        private IEnumerable<Scanner> ParseInput(string input)
        {
            var data = input
                .Split("--- scanner", StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim()
                    .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
                    .Skip(1)
                    .Select(s => s.Split(','))
                    .Select(i => new Vector(int.Parse(i[0]), int.Parse(i[1]), int.Parse(i[2])))
                    .ToHashSet()
                    )
                .Select(h => new Scanner { Vectors = h });

            return data;
        }

        private class Scanner {
            public HashSet<Vector> Vectors { get; set; }
            public Vector Position { get; set; } = new Vector(0, 0, 0);
        }

        private record Transformation
        {
            public int[] Matrix { get; private set; }

            public Transformation(params int[] matrix)
            {
                Matrix = matrix;
            }

            public int this[int row, int column]
            {
                get
                {
                    return Matrix[3 * row + column];
                }
            }
        }

        private record Vector(int X, int Y, int Z)
        {
            public int GetDistance(Vector p)
            {
                return Math.Abs(X - p.X) + Math.Abs(Y - p.Y) + Math.Abs(Z - p.Z);
            }

            public static Vector operator *(Vector vector, Transformation transformation)
            {
                return new Vector(
                    vector.X * transformation[0, 0] + vector.Y * transformation[0, 1] + vector.Z * transformation[0, 2],
                    vector.X * transformation[1, 0] + vector.Y * transformation[1, 1] + vector.Z * transformation[1, 2],
                    vector.X * transformation[2, 0] + vector.Y * transformation[2, 1] + vector.Z * transformation[2, 2]
                    );
            }

            public static Vector operator -(Vector vector1, Vector vector2)
            {
                return new Vector(vector1.X - vector2.X, vector1.Y - vector2.Y, vector1.Z - vector2.Z);
            }

            public static Vector operator +(Vector vector1, Vector vector2)
            {
                return new Vector(vector1.X + vector2.X, vector1.Y + vector2.Y, vector1.Z + vector2.Z);
            }

            public override string ToString()
            {
                return $"[{X}, {Y}, {Z}]";
            }
        }
    }
}
