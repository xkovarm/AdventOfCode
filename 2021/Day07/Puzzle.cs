using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Day07
{
    internal class Puzzle : PuzzleBase
    {
        protected override string SampleResult => "168"; // "37";

        protected override Task<string> GetSampleInputAsync()
        {
            return Task.FromResult(@"16,1,2,0,4,2,7,1,2,14");
        }

        protected override string DoCalculation(string input)
        {
            const bool useIncrementalCosts = true; // false;

            var positions = input
                .Split(',')
                .Select(int.Parse)
                .ToArray();

            var totalCosts = int.MaxValue;

            for (int position = positions.Min(); position <= positions.Max(); position++)
            {
                var currentCosts = 0;

                for (int j = 0; j < positions.Length; j++)
                {
                    var steps = Math.Abs(positions[j] - position);
                    currentCosts += !useIncrementalCosts ? steps : Enumerable.Range(1, steps).Sum();
                }

                if (currentCosts < totalCosts)
                {
                    totalCosts = currentCosts;
                }
            }

            return totalCosts.ToString();
        }
    }
}
