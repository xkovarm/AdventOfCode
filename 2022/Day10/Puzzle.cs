using System.Text;

namespace AdventOfCode.Day10
{
    internal class Puzzle : PuzzleBase
    {
        // -- CALCULATION PARAMETERS AND RESULT ------------------------------------

        protected override string SampleResult => "13140";

        // -------------------------------------------------------------------------

        protected override Task<string> GetSampleInputAsync()
        {
            return Task.FromResult(@"addx 15
addx -11
addx 6
addx -3
addx 5
addx -1
addx -8
addx 13
addx 4
noop
addx -1
addx 5
addx -1
addx 5
addx -1
addx 5
addx -1
addx 5
addx -1
addx -35
addx 1
addx 24
addx -19
addx 1
addx 16
addx -11
noop
noop
addx 21
addx -15
noop
noop
addx -3
addx 9
addx 1
addx -3
addx 8
addx 1
addx 5
noop
noop
noop
noop
noop
addx -36
noop
addx 1
addx 7
noop
noop
noop
addx 2
addx 6
noop
noop
noop
noop
noop
addx 1
noop
noop
addx 7
addx 1
noop
addx -13
addx 13
addx 7
noop
addx 1
addx -33
noop
noop
noop
addx 2
noop
noop
noop
addx 8
noop
addx -1
addx 2
addx 1
noop
addx 17
addx -9
addx 1
addx 1
addx -3
addx 11
noop
noop
addx 1
noop
addx 1
noop
noop
addx -13
addx -19
addx 1
addx 3
addx 26
addx -30
addx 12
addx -1
addx 3
addx 1
noop
noop
noop
addx -9
addx 18
addx 1
addx 2
noop
noop
addx 9
noop
noop
noop
addx -1
addx 2
addx -37
addx 1
addx 3
noop
addx 15
addx -21
addx 22
addx -6
addx 1
noop
addx 2
addx 1
noop
addx -10
noop
noop
addx 20
addx 1
addx 2
addx 2
addx -6
addx -11
noop
noop
noop");
        }

        protected override string DoCalculation(string input)
        {
            var prg = input
                .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
                .ToList();

            var result = Execute(prg);

            return result.ToString(); 
        }

        private int Execute(List<string> input)
        {
            var screen = new StringBuilder();

            int x = 1;
            int cycle = 1;
            int result = 0;

            foreach (var line in input)
            {
                var cmd = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                if (cmd is ["addx", _])
                {
                    CheckCycle();
                    cycle++;
                    CheckCycle();
                    cycle++;
                    x += int.Parse(cmd[1]);
                }
                else if (cmd is ["noop"])
                {
                    CheckCycle();
                    cycle++;
                }
            }

            Console.WriteLine(screen);

            return result;

            void CheckCycle()
            {
                // PART 1:
                if ((cycle - 20) % 40 == 0) // 20th, 60th, 80th, ...
                {
                    result += cycle * x;
                }

                // PART 2:
                var pos = cycle % 40 == 0 ? 40 : cycle % 40;
                screen.Append(pos >= x && pos <= x + 2 ? '#' : ' ');

                if (pos == 40)
                {
                    screen.AppendLine();
                }
            }
        }
    }
}
