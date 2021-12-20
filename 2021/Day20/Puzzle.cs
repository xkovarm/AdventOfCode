namespace AdventOfCode.Day20
{
    internal class Puzzle : PuzzleBase
    {
        protected override string SampleResult => "3351";   // "35";

        protected override Task<string> GetSampleInputAsync()
        {
            return Task.FromResult(@"..#.#..#####.#.#.#.###.##.....###.##.#..###.####..#####..#....#..#..##..###..######.###...####..#..#####..##..#.#####...##.#.#..#.##..#.#......#.###.######.###.####...#.##.##..#..#..#####.....#.#....###..#.##......#.....#..#..#..##..#...##.######.####.####.#.#...#.......#..#.#.#...####.##.#......#..#...##.#.##..#...##.#.##..###.#......#.#.......#.#.#.####.###.##...#.....####.#..#..#.##.#....##..#.####....##...##..#...#......#.#.......#.......##..####..#...#.#.#...##..#.#..###..#####........#..####......#..#

#..#.
#....
##..#
..#..
..###");
        }

        protected override string DoCalculation(string input)
        {
            const int iterations = 50;

            var data = input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).ToArray();

            var algorithm = data[0];
            var image = data[1..];

            char space = '.';

            for (int i = 0; i < iterations; i++)
            {
                image = EnhanceImage(image, space).ToArray();
                space = space == '#' ? algorithm.Last() : algorithm.First();
            }

            var litPixels = image.Select(i => i.ToArray().Where(c => c == '#').Count()).Sum();
            return litPixels.ToString();

            IEnumerable<string> EnhanceImage(string[] image, char space)
            {
                var width = image[0].Length;
                var height = image.Length;

                for (int y = 0; y < height + 2; y++)
                {
                    var line = new char[width + 2];

                    for (int x = 0; x < width + 2; x++)
                    {
                        int index = 0;

                        for (int yy = y - 1; yy <= y + 1; yy++)
                        {
                            for (int xx = x - 1; xx <= x + 1; xx++)
                            {
                                var ch = yy < 1 || xx < 1 || xx > width || yy > height ? space : image[yy - 1][xx - 1];
                                index = index << 1 | (ch == '#' ? 1 : 0);
                            }
                        }

                        line[x] = algorithm[index];
                    }

                    yield return new string(line);
                }
            }
        }
    }
}
