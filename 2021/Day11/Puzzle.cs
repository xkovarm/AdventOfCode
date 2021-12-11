using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Day11
{
    internal class Puzzle : PuzzleBase
    {
        protected override string SampleResult => "195"; // "1656";

        public Puzzle()
        {
            DoCalculation(@"11111
19991
19191
19991
11111");
        }

        protected override Task<string> GetSampleInputAsync()
        {
            return Task.FromResult(@"5483143223
2745854711
5264556173
6141336146
6357385478
4167524645
2176841721
6882881134
4846848554
5283751526");
        }

        protected override string DoCalculation(string input)
        {
            var iterations = int.MaxValue; // 100

            var pool = input
                .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
                .Select(t => t.Select(a => (byte)(a - '0')).ToArray())
                .ToArray();

            var totalFlashes = 0;

            var sizeY = pool.Length;
            var sizeX = pool[0].Length;

            var flashed = new HashSet<(int X, int Y)>();

            for (int step = 0; step < iterations; step++)
            {
                flashed.Clear();

                for (int y = 0; y < sizeY; y++)
                {
                    for (int x = 0; x < sizeX; x++)
                    {
                        pool[y][x]++;
                    }
                }

                bool anyFlash;

                do {                
                    anyFlash = false;
                    
                    for (int y = 0; y < sizeY; y++)
                    {
                        for (int x = 0; x < sizeX; x++)
                        {
                            if (pool[y][x] > 9)
                            {
                                if (!flashed.Any(f => f.X == x && f.Y == y))
                                {
                                    anyFlash = true;
                                    flashed.Add((x, y));

                                    totalFlashes++;

                                    GetNeighbors(x, y, sizeX, sizeY).ForEach(n => pool[n.Y][n.X]++);
                                }
                            }
                        }
                    }
                } while (anyFlash);

                flashed.ForEach(f => pool[f.Y][f.X] = 0);

                if (pool.All(p => p.All(p => p == 0)))
                {
                    // part 2
                    return (step+1).ToString();
                }
            }

            // part 1
            return totalFlashes.ToString();
        }

        private readonly List<(int X, int Y)> Neighbors = new List<(int X, int Y)>
        {
            (-1, -1), (-1, 0), (-1, 1), (0, 1), (1, 1), (1, 0), (1, -1), (0, -1)
        };

        private IEnumerable<(int X, int Y)> GetNeighbors(int x, int y, int sizeX, int sizeY)
        {
            var neighbors = Neighbors.Select(n => (X: n.X + x, Y: n.Y + y)).Where(n => n.X >= 0 && n.Y >= 0 && n.X < sizeX && n.Y < sizeY).ToArray();

            return neighbors;
        }
    }
}
