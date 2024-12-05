namespace AdventOfCode.Day04;

internal class Puzzle : PuzzleBase
{
    // -- CALCULATION PARAMETERS AND RESULT ------------------------------------

    protected override string SampleResult => "9"; //"18";

    protected override Task<string> GetSampleInputAsync()
    {
        return Task.FromResult(@"MMMSXXMASM
MSAMXMSMSA
AMXSXMAAMM
MSAMASMSMX
XMASAMXAMM
XXAMMXXAMA
SMSMSASXSS
SAXAMASAAA
MAMMMXMMMM
MXMXAXMASX");
    }

    // -------------------------------------------------------------------------

    private const string Find = "XMAS";
    private IEnumerable<(int R, int C)> Directions = [(0, 1), (1, 0), (0, -1), (-1, 0), (-1, -1), (-1, 1), (1, -1), (1, 1)];

    private HashSet<string> Patterns = new HashSet<string>(["MMSS", "SMMS", "SSMM", "MSSM"]);

    protected override string DoCalculation(string input)
    {
        var area = input
            .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
            .ToList();

        var count = 0;

        var cols = area[0].Length;
        var rows = area.Count;

        for (int row = 1; row < rows - 1; row++)
        {
            for (int col = 1; col < cols - 1; col++)
            {
                if (Check2(row, col))
                {
                    count++;
                }
            }
        }

        return count.ToString();

        bool Check2(int row, int col)
        {
            if (area[row][col] != 'A')
            {
                return false;
            }

            var pattern = $"{area[row - 1][col - 1]}{area[row - 1][col + 1]}{area[row + 1][col + 1]}{area[row + 1][col - 1]}";

            /* 
                Valid patterns:
             
                M M   S M   S S   M S
                 A     A     A     A
                S S   S M   M M   M S 
             */

            return Patterns.Contains(pattern);
        }


        int Check(int row, int col)
        {
            var count = 0;

            foreach (var direction in Directions)
            {
                var (r, c) = (row, col);

                count++;

                for (int i = 0; i < Find.Length; i++)
                {
                    if (r < 0 || c < 0 || r >= rows || c >= cols || area[r][c] != Find[i])
                    {
                        count--;
                        break;
                    }

                    (r, c) = (r + direction.R, c + direction.C);
                }
            }

            return count;
        }
    }
}

