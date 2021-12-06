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

        protected override Task<string> GetCheckInputAsync()
        {
            return Task.FromResult("1,1,1,1,1,5,1,1,1,5,1,1,3,1,5,1,4,1,5,1,2,5,1,1,1,1,3,1,4,5,1,1,2,1,1,1,2,4,3,2,1,1,2,1,5,4,4,1,4,1,1,1,4,1,3,1,1,1,2,1,1,1,1,1,1,1,5,4,4,2,4,5,2,1,5,3,1,3,3,1,1,5,4,1,1,3,5,1,1,1,4,4,2,4,1,1,4,1,1,2,1,1,1,2,1,5,2,5,1,1,1,4,1,2,1,1,1,2,2,1,3,1,4,4,1,1,3,1,4,1,1,1,2,5,5,1,4,1,4,4,1,4,1,2,4,1,1,4,1,3,4,4,1,1,5,3,1,1,5,1,3,4,2,1,3,1,3,1,1,1,1,1,1,1,1,1,4,5,1,1,1,1,3,1,1,5,1,1,4,1,1,3,1,1,5,2,1,4,4,1,4,1,2,1,1,1,1,2,1,4,1,1,2,5,1,4,4,1,1,1,4,1,1,1,5,3,1,4,1,4,1,1,3,5,3,5,5,5,1,5,1,1,1,1,1,1,1,1,2,3,3,3,3,4,2,1,1,4,5,3,1,1,5,5,1,1,2,1,4,1,3,5,1,1,1,5,2,2,1,4,2,1,1,4,1,3,1,1,1,3,1,5,1,5,1,1,4,1,2,1");
        }

        protected override Task<string> GetSampleInputAsync()
        {
            return Task.FromResult("3,4,3,1,2");
        }
    }
}
