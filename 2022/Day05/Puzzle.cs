namespace AdventOfCode.Day05
{
    internal class Puzzle : PuzzleBase
    {
        // -- CALCULATION PARAMETERS AND RESULT ------------------------------------

        protected override string SampleResult => "MCD"; // PART 1: "CMZ";

        // -------------------------------------------------------------------------

        private record Move(int Count, int From, int To);

        protected override string DoCalculation(string input)
        {
            var (stacks, moves) = ParseInput(input);

            // PART 1: Play(stacks, moves);
            Play2(stacks, moves);

            return new string(stacks.Select(s => s.Peek()).ToArray());
        }

        private void Play(Stack<char>[] stacks, List<Move> moves)
        {
            foreach (var move in moves)
            {
                for (int m = 0; m < move.Count; m++)
                {
                    stacks[move.To - 1].Push(stacks[move.From - 1].Pop());
                }
            }
        }

        private void Play2(Stack<char>[] stacks, List<Move> moves)
        {
            var crateMoverStack = new Stack<char>();

            foreach(var move in moves)
            {
                for (int m = 0; m < move.Count; m++)
                {
                    crateMoverStack.Push(stacks[move.From - 1].Pop());
                }

                while (crateMoverStack.Any())
                {
                    stacks[move.To - 1].Push(crateMoverStack.Pop());
                }
            }
        }

        private (Stack<char>[], List<Move>) ParseInput(string input)
        {
            var parts = input.Split($"{Environment.NewLine}{Environment.NewLine}");

            var config = parts[0]
                .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
                .ToArray();

            var stacksCount = config[^1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Count();

            // (╯°□°）╯︵ ┻━┻

            var stacks = new Stack<char>[stacksCount];

            for (int i = 0; i < stacksCount; i++)
            {
                stacks[i] = new Stack<char>();
                var columnIndex = config[^1].IndexOf($"{i + 1}");

                for (int j = 2; j <= config.Length; j++)
                {
                    var ch = config[^j][columnIndex];

                    if (ch == ' ')
                    {
                        break;
                    }

                    stacks[i].Push(ch);
                }
            }

            var moves = parts[1]
                .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
                .Select(m => m.Split(new[] { " ", "move", "from", "to" }, StringSplitOptions.RemoveEmptyEntries))
                .Select(m => new Move(int.Parse(m[0]), int.Parse(m[1]), int.Parse(m[2])))
                .ToList();

            return (stacks, moves);
        }
    }
}
