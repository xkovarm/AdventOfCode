namespace AdventOfCode.Day25
{
    internal class Puzzle : PuzzleBase
    {
        protected override string SampleResult => "58";

        protected override Task<string> GetSampleInputAsync()
        {
            return Task.FromResult(@"v...>>.vv>
.vv>>.vv..
>>.>v>...v
>>v>>.>.v.
v>v.vv.v..
>.>>..v...
.vv..>.>v.
v.v..>>v.v
....v..v.>");
        }

        protected override string DoCalculation(string input)
        {
            var data = input
                .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
                .Select(i => i.ToArray())
                .ToArray();

            var maxX = data[0].Length;
            var maxY = data.Length;

            var data2 = new char[maxY][];
            for (int i = 0; i < maxY; i++)
            {
                data2[i] = new char[maxX];
            }

            var iteration = 0;
            bool moved1 = false;
            bool moved2 = false;

            do
            {
                moved1 = MoveRight(data, data2);
                (data, data2) = (data2, data);

                moved2 = MoveDown(data, data2);
                (data, data2) = (data2, data);

                iteration++;
            } 
            while (moved1 || moved2);

            return iteration.ToString();

            bool MoveRight(char[][] source, char[][] destination)
            {
                bool moved = false;

                for (int y = 0; y < maxY; y++)
                {
                    for (int x = maxX - 1; x >= 0 ; x--)
                    {
                        if (source[y][x - 1 < 0 ? maxX - 1 : x - 1] == '>' && source[y][x] == '.')
                        {
                            destination[y][x] = '>';
                            source[y][x - 1 < 0 ? maxX - 1 : x - 1] = '#';

                            moved = true;
                        } 
                        else
                        {
                            destination[y][x] = source[y][x] == '#' ? '.' : source[y][x];
                        }
                    }

                    if (source[y][maxX-1] == '#')
                    {
                        destination[y][maxX - 1] = '.';
                    }
                }

                return moved;
            }

            bool MoveDown(char[][] source, char[][] destination)
            {
                bool moved = false;

                for (int x = 0; x < maxX; x++)
                {
                    for (int y = maxY - 1; y >= 0; y--)
                    {
                        if (source[y - 1 < 0 ? maxY - 1 : y - 1][x] == 'v' && source[y][x] == '.')
                        {
                            destination[y][x] = 'v';
                            source[y - 1 < 0 ? maxY - 1 : y - 1][x] = '#';

                            moved = true;
                        }
                        else
                        {
                            destination[y][x] = source[y][x] == '#' ? '.' : source[y][x];
                        }
                    }

                    if (source[maxY - 1][x] == '#')
                    {
                        destination[maxY - 1][x] = '.';
                    }
                }

                return moved;
            }
        }
    }
}
