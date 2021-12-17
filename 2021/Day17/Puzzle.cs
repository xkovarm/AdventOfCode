using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Day17
{
    internal class Puzzle : PuzzleBase
    {
        protected override string SampleResult => "112"; // "45";

        protected override Task<string> GetSampleInputAsync()
        {
            return Task.FromResult(@"target area: x=20..30, y=-10..-5");
        }

        protected override string DoCalculation(string input)
        {
            var parts = input.Split(new[] {"target area: ", "x=", "y=", ", ", ".." }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
            var target = (Min: (X: parts[0], Y: parts[2]), Max: (X: parts[1], Y: parts[3]));

            var results = new HashSet<(int DX, int DY, int MaxY)>();

            // NOTE: The target area is in front of me and below the zero height, hence...            
            var yLimit = (int)Math.Sqrt(target.Max.X * target.Max.X + target.Min.Y * target.Min.Y); // just guessing by my feeling.... :) :) :)

            for (int xx = 0; xx <= target.Max.X; xx++)      // the probe cannot go back, negative values can be skipped
            {
                for (int yy = -yLimit; yy <= yLimit; yy++)
                {
                    (int x, int y, int dx, int dy, int maxy) = (0, 0, xx, yy, int.MinValue);

                    while (true)
                    {
                        (x, y, dx, dy) = (x + dx, y + dy, dx + (dx > 0 ? -1 : (dx < 0 ? 1 : 0)), dy - 1);

                        if (maxy < y)
                        {
                            maxy = y;
                        }

                        // the probe is too far or too below, there is no chance to return
                        if (x > target.Max.X || y < target.Min.Y)
                        {
                            break;
                        }
                        // hit
                        else if (x >= target.Min.X && y >= target.Min.Y && y <= target.Max.Y)
                        {
                            results.Add((xx, yy, maxy));
                            break;
                        }
                    }
                }
            }

            //return results.Max(v => v.MaxY).ToString();
            return results.Distinct().Count().ToString();
        }
    }
}
