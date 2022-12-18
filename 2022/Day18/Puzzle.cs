namespace AdventOfCode.Day18
{
    internal class Puzzle : PuzzleBase
    {
        // -- CALCULATION PARAMETERS AND RESULT ------------------------------------

        protected override string SampleResult => "58"; // PART 1: "64";

        // -------------------------------------------------------------------------

        protected override string DoCalculation(string input)
        {
            var cubes = input
                .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries) 
                .Select(i => i.Split(',').ToList())
                .Select(i => new Cube(int.Parse(i[0]), int.Parse(i[1]), int.Parse(i[2])))
                .ToHashSet();

            // PART 1: var result = Calculate(cubes);
            var result = Calculate2(cubes);

            return result.ToString();
        }

        private int Calculate(HashSet<Cube> cubes)
        {
            return cubes.Select(c => GetFreeNeighbours(cubes, c).Count).Sum();
        }

        private int Calculate2(HashSet<Cube> cubes)
        {
            var result = 0;

            var min = new Cube(-1, -1, -1);
            var max = new Cube(cubes.Select(c => c.X).Max() + 1, cubes.Select(c => c.Y).Max() + 1, cubes.Select(c => c.Z).Max() + 1);

            var visited = new HashSet<Cube>();

            var queue = new Queue<Cube>();
            queue.Enqueue(min);

            while (queue.Any())
            {
                var current = queue.Dequeue();

                if (!visited.Contains(current))
                {
                    var neighbours = GetFreeNeighbours(cubes, current);

                    result += 6 - neighbours.Count;

                    neighbours.ForEach(n =>
                    {
                        if (!visited.Contains(n) && n >= min && n <= max)
                        {
                            queue.Enqueue(n);
                        }
                    });
                }

                visited.Add(current);
            }
            
            return result;
        }

        List<Cube> GetFreeNeighbours(HashSet<Cube> cubes, Cube cube)
        {
            var neighbours = new[] {
                    cube with { X = cube.X - 1 },
                    cube with { X = cube.X + 1 },
                    cube with { Y = cube.Y - 1 },
                    cube with { Y = cube.Y + 1 },
                    cube with { Z = cube.Z - 1 },
                    cube with { Z = cube.Z + 1 }};

            return neighbours.Where(c => !cubes.Contains(c)).ToList();
        }

        private record Cube(int X, int Y, int Z)
        {
            public static bool operator >=(Cube a, Cube b)
            {
                return a.X >= b.X && a.Y >= b.Y && a.Z >= b.Z;
            }

            public static bool operator <=(Cube a, Cube b)
            {
                return a.X <= b.X && a.Y <= b.Y && a.Z <= b.Z;
            }
        }
    }
}
