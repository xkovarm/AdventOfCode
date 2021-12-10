using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Day02
{
    internal class Puzzle : PuzzleBase
    {
        // -- CALCULATION PARAMETERS AND RESULT ------------------------------------

        protected override string SampleResult => "900"; // "150"

        // -------------------------------------------------------------------------

        protected override string DoCalculation(string input)
        {
            var path = input
                .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Split(' '))
                .Select(s => (Direction: s[0], Step: int.Parse(s[1])))
                .ToArray();

            //return Solution1(path);
            return Solution2(path);
        }

        protected override Task<string> GetSampleInputAsync()
        {
            return Task.FromResult(@"
forward 5
down 5
forward 8
up 3
down 8
forward 2");
        }

        private string Solution1((string Direction, int Step)[] path)
        {
            var horizonal = 0;
            var depth = 0;

            foreach (var step in path)
            {
                switch (step.Direction)
                {
                    case "forward":
                        horizonal += step.Step;
                        break;
                    case "down":
                        depth += step.Step;
                        break;
                    case "up":
                        depth -= step.Step;
                        break;
                }
            }

            var total = horizonal * depth;
            return total.ToString();
        }

        private string Solution2((string Direction, int Step)[] path)
        {
            var horizonal = 0;
            var depth = 0;
            var aim = 0;

            foreach (var step in path)
            {
                switch (step.Direction)
                {
                    case "forward":
                        horizonal += step.Step;
                        depth += aim * step.Step;
                        break;
                    case "down":
                        aim += step.Step;
                        break;
                    case "up":
                        aim -= step.Step;
                        break;
                }
            }

            var total = horizonal * depth;
            return total.ToString();
        }
    }
}
