namespace AdventOfCode.Day06
{
    internal class Puzzle : PuzzleBase
    {
        // -- CALCULATION PARAMETERS AND RESULT ------------------------------------

        protected override string SampleResult => "19"; // PART 1: "7"; 

        // -------------------------------------------------------------------------

        protected override string DoCalculation(string input)
        {
            // PART 1: return Calculate(input, 4).ToString();
            return Calculate(input, 14).ToString();
        }

        private int Calculate(string input, int headerLength)
        {
            for (int i = 0; i < input.Length - headerLength; i++)
            {
                var distinctLength = input.Skip(i).Take(headerLength).Distinct().Count();
                
                if (distinctLength == headerLength)
                {
                    return i + headerLength;
                }
            }

            return -1;
        }        
    }
}
