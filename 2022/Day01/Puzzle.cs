namespace AdventOfCode.Day01
{
    internal class Puzzle : PuzzleBase
    {
        // -- CALCULATION PARAMETERS AND RESULT ------------------------------------

        protected override string SampleResult => "45000"; //"24000";  

        // -------------------------------------------------------------------------

        protected override Task<string> GetSampleInputAsync()
        {
            return Task.FromResult(
@"1000
2000
3000

4000

5000
6000

7000
8000
9000

10000");
        }

        protected override string DoCalculation(string input)
        {
            var sums = input
                .Split($"{Environment.NewLine}{Environment.NewLine}", StringSplitOptions.RemoveEmptyEntries) 
                .Select(v => 
                    v.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
                    .Select(int.Parse)
                    .Sum()
                )
                .ToList();

            //return sums.Max().ToString();
            return sums.OrderByDescending(s => s).Take(3).Sum().ToString();
        }
    }
}
