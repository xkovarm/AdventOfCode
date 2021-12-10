namespace AdventOfCode.Day10
{
    internal class Puzzle : PuzzleBase
    {
        protected override string SampleResult => "288957"; // "26397";

        private readonly Dictionary<char, char> Complements = new Dictionary<char, char>
        {
            ['('] = ')',
            ['{'] = '}',
            ['<'] = '>',
            ['['] = ']'
        };

        private readonly Dictionary<char, int> CorruptedScores = new Dictionary<char, int>
        {
            [')'] = 3,
            [']'] = 57,
            ['}'] = 1197,
            ['>'] = 25137
        };

        private readonly Dictionary<char, int> MissingScores = new Dictionary<char, int>
        {
            ['('] = 1,
            ['['] = 2,
            ['{'] = 3,
            ['<'] = 4
        };

        protected override Task<string> GetSampleInputAsync()
        {
            return Task.FromResult(@"[({(<(())[]>[[{[]{<()<>>
[(()[<>])]({[<{<<[]>>(
{([(<{}[<>[]}>{[]{[(<()>
(((({<>}<{<{<>}{[]{[]{}
[[<[([]))<([[{}[[()]]]
[{[{({}]{}}([{[{{{}}([]
{<[[]]>}<{[{[{[]{()[[[]
[<(<(<(<{}))><([]([]()
<{([([[(<>()){}]>(<<{{
<{([{{}}[<[[[<>{}]]]>[]]");
        }

        protected override string DoCalculation(string input)
        {
            long corruptedScore = 0;
            var scores = new List<long>();

            var data = input
                .Split(Environment.NewLine)
                .ToArray();

            var stack = new Stack<char>();

            foreach (var line in data)
            {
                char invalid = default;

                stack.Clear();      // don't forget to clear the stack !!!

                foreach (var ch in line)
                {
                    if ("([{<".Contains(ch))
                    {
                        stack.Push(ch);
                    } 
                    else
                    {
                        if (stack.Any() && Complements[stack.Peek()] == ch)
                        {
                            stack.Pop();
                        } 
                        else
                        {
                            invalid = ch;
                            break;
                        }
                    }
                }

                if (invalid != default)
                {
                    // corrupted line
                    corruptedScore += CorruptedScores[invalid];
                } 
                else 
                { 
                    if (stack.Any())
                    {
                        // incomplete line
                        long subScore = 0;

                        while (stack.Any())
                        {
                            subScore = 5 * subScore + MissingScores[stack.Pop()];
                        }

                        scores.Add(subScore);
                    } 
                }
            }

            var orderedScores = scores.OrderBy(i => i).ToArray();
            return orderedScores[orderedScores.Length / 2].ToString();

            //return corruptedScore.ToString();
        }
    }
}
