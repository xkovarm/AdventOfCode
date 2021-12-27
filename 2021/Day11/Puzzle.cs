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

                                    for (int iy = y-1; iy <= y+1; iy++)
                                    {
                                        for (int ix = x-1; ix <= x+1; ix++)
                                        {
                                            if (ix >= 0 && iy >= 0 && ix < sizeX && iy < sizeY && !(ix == x && iy == y))
                                            {
                                                pool[iy][ix]++;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                } while (anyFlash);

                flashed.ToList().ForEach(f => pool[f.Y][f.X] = 0);

                if (pool.All(p => p.All(p => p == 0)))
                {
                    // part 2
                    return (step+1).ToString();
                }
            }

            // part 1
            return totalFlashes.ToString();
        }
    }
}
