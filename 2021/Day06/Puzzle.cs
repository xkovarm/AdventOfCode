namespace AdventOfCode.Day06
{
    internal class Puzzle : PuzzleBase
    {
        protected override string SampleResult => "26984457539"; // "5934";

        protected override string DoCalculation(string input)
        {
            const int days = 256; // 80;
            const int maxAge = 9;
            const int ageAfterBirth = 6;

            var aquarium = new long[maxAge];

            input
                .Split(',')
                .Select(int.Parse)
                .GroupBy(i => i)
                .Select(i => new { i.Key, Count = i.Count() })
                .ToList()
                .ForEach(d => aquarium[d.Key] = d.Count);

            for (int i = 0; i < days; i++)
            {
                var parents = aquarium[0];

                for (int j = 0; j < maxAge-1; j++)
                {
                    aquarium[j] = aquarium[j + 1];
                }

                aquarium[ageAfterBirth] += parents;
                aquarium[maxAge-1] = parents;
            }

            return aquarium.Sum().ToString();
        }

        protected override Task<string> GetSampleInputAsync()
        {
            return Task.FromResult("3,4,3,1,2");
        }
    }
}
