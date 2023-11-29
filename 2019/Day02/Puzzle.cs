namespace AdventOfCode.Day02
{
    internal class Puzzle : PuzzleBase
    {
        // -- CALCULATION PARAMETERS AND RESULT ------------------------------------

        protected override string SampleResult => "3500";

        // -------------------------------------------------------------------------

        protected override Task<string> GetSampleInputAsync()
        {
            return Task.FromResult("1,9,10,3,2,3,11,0,99,30,40,50");
        }

        protected override string DoCalculation(string input)
        {
            var instructions = input.Split(",", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();

            if (instructions.Count() > 20)
            {
                instructions[1] = 12;
                instructions[2] = 2;
            }

            var result = RunProgram(instructions);

            return result.ToString();
        }

        protected int RunProgram(List<int> instructions)
        {
            int index = 0;

            while (instructions[index] != 99)
            {
                var a = instructions[instructions[index + 1]];
                var b = instructions[instructions[index + 2]];

                instructions[instructions[index + 3]] = instructions[index] switch
                {
                    1 => a + b,
                    2 => a * b
                };

                index += 4;
            }

            return instructions[0];
        }
    }
}
