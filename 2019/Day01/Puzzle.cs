namespace AdventOfCode.Day01
{
    internal class Puzzle : PuzzleBase
    {
        // -- CALCULATION PARAMETERS AND RESULT ------------------------------------

        protected override string SampleResult => "966"; // PART 1: "654";

        // -------------------------------------------------------------------------

        protected override Task<string> GetSampleInputAsync()
        {
            return Task.FromResult("1969");
        }

        protected override string DoCalculation(string input)
        {
            var masses = input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();

            var total = masses.Select(Calculate).Sum();

            return total.ToString();

            int GetFuel(int mass)
            {
                return mass / 3 - 2;
            }

            int Calculate(int mass)
            {
                var x = GetFuel(mass);

                // PART 1: return x;   

                if (x <= 0)
                {
                    return 0;
                }

                return x + Calculate(x);
            }
        }
    }
}
