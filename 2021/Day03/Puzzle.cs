namespace AdventOfCode.Day03
{
    internal class Puzzle : PuzzleBase
    {
        protected override string SampleResult =>  "230"; // "198"; 

        protected override string DoCalculation(string input)
        {
            var path = input
                .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
                .ToList();

            //return Solution1(path);
            return Solution2(path);
        }

        protected override Task<string> GetCheckInputAsync()
        {
            return File.ReadAllTextAsync(@"Day03\input.txt");
        }

        protected override Task<string> GetSampleInputAsync()
        {
            return Task.FromResult(@"
00100
11110
10110
10111
10101
01111
00111
11100
10000
11001
00010
01010");
        }

        private string Solution1(string[] data)
        {
            var ones = new int[data[0].Length];

            for (int bit = 0; bit < ones.Length; bit++)
            {
                ones[bit] = data.Count(d => d[bit] == '1');
            }

            var output = new string(ones.Select(i => i * 2 > data.Length ? '1' : '0').ToArray());

            var gammaRate = Convert.ToInt32(output, 2);
            var epsilon = Convert.ToInt32(new string(output.Select(ch => ch == '1' ? '0' : '1').ToArray()), 2);

            return (gammaRate * epsilon).ToString();
        }

        private string Solution2(List<string> data)
        {
            var dataCopy = new List<string>(data);
            var length = data[0].Length;

            for (int bit = 0; bit < length; bit++)
            {
                ReduceData(data, bit, true);
            }

            for (int bit = 0; bit < length; bit++)
            {
                ReduceData(dataCopy, bit, false);
            }

            var oxygenGeneratorRating = Convert.ToInt32(data.Single(), 2);
            var co2ScrubberRating = Convert.ToInt32(dataCopy.Single(), 2);

            return (oxygenGeneratorRating * co2ScrubberRating).ToString();

            void ReduceData(List<string> data, int bit, bool mostCommonValue)
            {
                if (data.Count == 1)
                {
                    return;
                }

                int ones = data.Count(d => d[bit] == '1');

                var filter = mostCommonValue ? (ones * 2 >= data.Count ? '0' : '1') : (ones * 2 < data.Count ? '0' : '1');

                data.RemoveAll(d => d[bit] == filter);
            }
        }
    }
}
