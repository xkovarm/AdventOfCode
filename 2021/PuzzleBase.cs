using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    internal abstract class PuzzleBase
    {
        protected abstract string SampleResult { get; }

        protected abstract Task<string> GetSampleInputAsync();

        protected abstract Task<string> GetCheckInputAsync();

        protected abstract string DoCalculation(string input);

        public async Task<string> CalculateAsync()
        {
            var input = await GetCheckInputAsync();

            return DoCalculation(input);
        }

        public async Task AssertAsync()
        {
            var input = await GetSampleInputAsync();

            var result = DoCalculation(input);

            if (result != SampleResult)
            {
                Debugger.Break();
                throw new InvalidOperationException($"Assertion failed! Current result: {result}, expected: {SampleResult}");
            }
        }
    }
}
