using System.Text;

namespace AdventOfCode.Day25
{
    internal class Puzzle : PuzzleBase
    {
        // -- CALCULATION PARAMETERS AND RESULT ------------------------------------

        protected override string SampleResult => "2=-1=0";

        // -------------------------------------------------------------------------

        public Puzzle()
        {
            Debug.Assert(FromSnafu("2=") == 8);
            Debug.Assert(FromSnafu("1=11-2") == 2022);
            Debug.Assert(FromSnafu("1-0---0") == 12345);
            Debug.Assert(FromSnafu("1121-1110-1=0") == 314159265);

            Debug.Assert(ToSnafu(8) == "2=");
            Debug.Assert(ToSnafu(2022) == "1=11-2");
            Debug.Assert(ToSnafu(12345) == "1-0---0");
            Debug.Assert(ToSnafu(314159265) == "1121-1110-1=0");
        }

        protected override string DoCalculation(string input)
        {
            var lines = input
                .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries) 
                .ToList();

            var result = Calculate(lines);

            return result;
        }

        private string Calculate(List<string> lines)
        {
            var value = checked(lines.Select(FromSnafu).Sum());

            return ToSnafu(value);
        }

        private long FromSnafu(string input)
        {
            long value = 0;
            long multiplier = 1;

            foreach(char digit in input.Reverse())
            {
                value += multiplier * GetValue(digit);
                multiplier *= 5;
            }

            return value;
        }

        private string ToSnafu(long input)
        {
            var value = new StringBuilder();

            do
            {
                var mod = input % 5;
                input /= 5;

                if (mod > 2)
                {
                    mod -= 5;
                    input += 1;
                } 

                value.Append(GetDigit(mod));
            } 
            while (input != 0);

            return new String(value.ToString().Reverse().ToArray());
        }

        public char GetDigit(long value)
        {
            return value switch
            {
                0 => '0',
                1 => '1',
                2 => '2',
                -1 => '-',
                -2 => '=',
                _ => throw new InvalidOperationException()
            };
        }

        public long GetValue(char digit)
        {
            return digit switch
            {
                '0' => 0,
                '1' => 1,
                '2' => 2,
                '-' => -1,
                '=' => -2,
                _ => throw new InvalidOperationException()
            };
        }
    }
}
