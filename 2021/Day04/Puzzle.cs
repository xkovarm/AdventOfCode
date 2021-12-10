namespace AdventOfCode.Day04
{
    internal class Puzzle : PuzzleBase
    {
        protected override string SampleResult => "1924"; //"4512";

        private const int BoardDimension = 5;

        protected override string DoCalculation(string input)
        {
            var data = input
                .Split(Environment.NewLine + Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
                .ToArray();

            var numbers = data[0].Split(',').Select(int.Parse).ToArray();
            var boards = ParseBoards(data[1..]).ToArray();

            return Play(numbers, boards);

            IEnumerable<int[]> ParseBoards(string[] input)
            {
                foreach (var line in input)
                {
                    var board = line
                        .Replace(Environment.NewLine, " ")
                        .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                        .Select(int.Parse)
                        .ToArray();

                    yield return board;
                }
            }
        }

        private string Play(int[] numbers, int[][] boards)
        {
            bool findFirst = false; // true;

            var winningBoards = new Dictionary<int[], bool>();
            boards.ForEach(b => winningBoards.Add(b, false));

            foreach (var number in numbers)
            {
                foreach (var board in boards)
                {
                    if ((Array.IndexOf(board, number) is var index) && index > -1)
                    {
                        board[index] = -1; 

                        if (CheckBoard(board, index))
                        {
                            winningBoards[board] = true;

                            if (findFirst || winningBoards.Values.All(t => t))
                            {
                                var sum = board.Where(t => t >= 0).Sum();

                                return (sum * number).ToString();
                            }
                        }
                    }
                }
            }

            return String.Empty;

            bool CheckBoard(int[] board, int index)
            {
                var row = index / BoardDimension;
                var col = index % BoardDimension;

                // check values in a column
                var found = true;
                for (int i = 0; i < BoardDimension; i++)
                {
                    if (board[i * BoardDimension + col] >= 0)
                    {
                        found = false;
                        break;
                    }
                }

                if (found)
                {
                    return true;
                }

                // check values in a row
                found = true;
                for (int i = 0; i < BoardDimension; i++)
                {
                    if (board[row * BoardDimension + i] >= 0)
                    {
                        found = false;
                        break;
                    }
                }

                return found;
            }
        }

        protected override Task<string> GetSampleInputAsync()
        {
            return Task.FromResult(@"7,4,9,5,11,17,23,2,0,14,21,24,10,16,13,6,15,25,12,22,18,20,8,19,3,26,1

22 13 17 11  0
 8  2 23  4 24
21  9 14 16  7
 6 10  3 18  5
 1 12 20 15 19

 3 15  0  2 22
 9 18 13 17  5
19  8  7 25 23
20 11 10 24  4
14 21 16 12  6

14 21 17 24  4
10 16 15  9 19
18  8 23 26 20
22 11 13  6  5
 2  0 12  3  7
");
        }
    }
}
