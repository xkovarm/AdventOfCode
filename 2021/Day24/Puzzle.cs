namespace AdventOfCode.Day24
{
    internal class Puzzle : PuzzleBase
    {
        protected override string SampleResult => string.Empty;

        private readonly int[] P1 = { 1, 1, 1, 1, 26, 1, 1, 26, 1, 26, 26, 26, 26, 26 };
        private readonly int[] P2 = { 14, 13, 15, 13, -2, 10, 13, -15, 11, -9, -9, -7, -4, -6 };
        private readonly int[] P3 = { 0, 12, 14, 0, 3, 15, 11, 12, 1, 12, 3, 10, 14, 12 };

        protected override Task<string> GetSampleInputAsync()
        {
            return Task.FromResult(string.Empty);
        }

        private record Instruction(string Command, string Arg, string Arg2);

        protected override string DoCalculation(string input)
        {
            // sample is not available
            if (string.IsNullOrWhiteSpace(input)) return string.Empty;

            //var program = input
            //    .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
            //    .Select(l => l.Split(' ', StringSplitOptions.RemoveEmptyEntries))
            //    .Select(l => new Instruction(l[0], l[1], l.Length > 2 ? l[2] : null))
            //    .ToArray();

            //ReformatAndDumpProgram(program);

            (long? minValue, long? maxValue) = (null, null);

            for (long l = 10000000000000; l <= 99999999999999; l++)
            {
                var digits = l.ToString();
                long z = 0;

                for (int i = 0; i < 14; i++)
                {
                    var digit = digits[i] - '0';

                    var check = z % 26 + P2[i] == digit;

                    if (digit != 0 /*&& P1[i] == 26*/ && check)
                    {
                        z = z / P1[i];
                    }
                    else if (digit != 0 && P1[i] == 1 && !check)
                    {
                        z = 26 * z /* / P1[i] */ + digit + P3[i];
                    }
                    else
                    {
                        l += (long)Math.Pow(10, 13 - i) - 1;
                        break;
                    }
                }

                if (z == 0)
                {
                    if (!minValue.HasValue)
                    {
                        minValue = l;
                    }

                    maxValue = l;                    
                }
            }

            //return maxValue.Value.ToString();

            return minValue.Value.ToString();
        }

        void ReformatAndDumpProgram(Instruction[] program)
        {
            var table = new List<List<Instruction>>();

            var index = 0;
            foreach (var instruction in program)
            {
                if (instruction.Command == "inp")
                {
                    index = 0;
                }

                if (table.Count <= index)
                {
                    table.Insert(index, new List<Instruction>());
                }

                // instruction and the first argument must match, the second argument may differ
                if (!table[index].Any() || table[index][0].Command == instruction.Command && table[index][0].Arg == instruction.Arg)
                {
                    table[index].Add(instruction);
                }
                else
                {
                    throw new InvalidOperationException($"Unexpected instruction, got {instruction.Command}, expected {table[index][0].Command}");
                }

                index++;
            }

            foreach (var record in table)
            {
                Console.WriteLine($"{record[0].Command} {record[0].Arg}: {string.Join(' ', record.Select(s => s.Arg2).ToArray()) }");
            }
        }
    }
}
