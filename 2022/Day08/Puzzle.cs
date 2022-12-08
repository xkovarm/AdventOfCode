namespace AdventOfCode.Day08
{
    internal class Puzzle : PuzzleBase
    {
        // -- CALCULATION PARAMETERS AND RESULT ------------------------------------

        protected override string SampleResult => "8"; //PART 1: "21";

        // -------------------------------------------------------------------------

        protected override string DoCalculation(string input)
        {
            var trees = input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).ToArray();

            // Let's assume that the grid is a square
            if (trees.Length != trees[0].Length)
            {
                throw new InvalidOperationException();
            }

            //PART 1: var result = Calculate(trees);
            var result = Calculate2(trees);

            return result.ToString();
        }

        private int Calculate(string[] trees)
        {
            var maxIndex = trees.Length - 1;

            var result = 0;

            for (int row = 0; row <= maxIndex; row++)
            {
                for (int col = 0; col <= maxIndex; col++)
                {
                    if (IsVisible(i => trees[i][col], row, -1)
                        || IsVisible(i => trees[i][col], row, 1)
                        || IsVisible(i => trees[row][i], col, -1)
                        || IsVisible(i => trees[row][i], col, 1))
                    {
                        result++;
                    }
                }
            }

            return result;

            bool IsVisible(Func<int, char> selector, int start, int direction)
            {
                var index = start + direction;
                bool result = true;

                while (index >= 0 && index <= maxIndex)
                {
                    if (selector(index) >= selector(start))
                    {
                        result = false;
                        break;
                    }

                    index += direction;
                }

                return result;
            }
        }

        private int Calculate2(string[] trees)
        {
            var maxIndex = trees.Length - 1;

            var result = 0;

            for (int row = 0; row <= maxIndex; row++)
            {
                for (int col = 0; col <= maxIndex; col++)
                {
                    var score =
                        GetVisibleDistance(i => trees[i][col], row, -1)
                        * GetVisibleDistance(i => trees[i][col], row, 1)
                        * GetVisibleDistance(i => trees[row][i], col, -1)
                        * GetVisibleDistance(i => trees[row][i], col, 1)
                        ?? 0;

                    if (score > result)
                    {
                        result = score;
                    }
                }
            }

            return result;


            int? GetVisibleDistance(Func<int, char> selector, int start, int direction)
            {
                var index = start + direction;
                int? result = null;

                while (index >= 0 && index <= maxIndex)
                {
                    result = Math.Abs(index - start);

                    if (selector(index) >= selector(start))
                    {
                        break;
                    }

                    index += direction;
                }

                return result;
            }
        }
    }
}
