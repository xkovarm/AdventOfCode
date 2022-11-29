namespace AdventOfCode.Day01
{
    internal class Puzzle : PuzzleBase
    {
        // -- CALCULATION PARAMETERS AND RESULT ------------------------------------

        private const int WindowSize = 3; // part 1: 1;

        protected override string SampleResult => "5";  // part 1: "7";

        // -------------------------------------------------------------------------

        protected override Task<string> GetSampleInputAsync()
        {
            return File.ReadAllTextAsync(@"Day01\sample.txt");
        }

        protected override string DoCalculation(string input)
        {
            var values = input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();

            var result = 0;
            var previous = -1;

            for (int i = 0; i < values.Length - WindowSize + 1; i++)
            {
                var current = values[i..(i + WindowSize)].Sum();

                if (current > previous && previous != -1)
                {
                    result++;
                }

                previous = current;
            }

            return result.ToString();
        }
    }
}
