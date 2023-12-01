using MoreLinq;

namespace AdventOfCode.Day01;

internal class Puzzle : PuzzleBase
{
    // -- CALCULATION PARAMETERS AND RESULT ------------------------------------

    protected override string SampleResult => "281";

    private string[] Words = { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };

    // -------------------------------------------------------------------------

    protected override Task<string> GetSampleInputAsync()
    {
        return Task.FromResult(@"two1nine
eightwothree
abcone2threexyz
xtwone3four
4nineeightseven2
zoneight234
7pqrstsixteen");
    }

    protected override string DoCalculation(string input)
    {
        var sum = input
            .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
            .Select(GetNumber)
            .Select(int.Parse)
            .Sum();

        return sum.ToString();
    }

    private string GetNumber(string input) 
    {
        var span = input.AsSpan();

        return $"{Scan(span, true)}{Scan(span, false)}";

        int Scan(ReadOnlySpan<char> span, bool forward)
        {
            for (int i = forward ? 0 : input.Length - 1; i < input.Length && forward || i >= 0 && !forward ; i += forward ? 1 : -1)
            {
                var found = CheckLiteral(span.Slice(i));

                if (found.HasValue)
                {
                    return found.Value;
                }
            }

            throw new InvalidOperationException();
        }

        int? CheckLiteral(ReadOnlySpan<char> span)
        {
            if (span[0] >= '0' && span[0] <= '9')
            {
                return span[0] - '0';
            }

            for (int i = 0; i < Words.Length; i++)
            {
                if (span.StartsWith(Words[i]))
                {
                    return i + 1;
                }
            }

            return null;
        }
    }
}
