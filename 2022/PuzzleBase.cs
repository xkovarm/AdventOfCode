using System.Text.RegularExpressions;

namespace AdventOfCode
{
    internal abstract class PuzzleBase
    {
        protected abstract string SampleResult { get; }

        protected abstract Task<string> GetSampleInputAsync();

        protected abstract string DoCalculation(string input);

        public async Task<string> CalculateAsync()
        {
            var input = await GetCheckInputAsync();

            return DoCalculation(input);
        }

        public async Task AssertAsync()
        {
            var input = await GetSampleInputAsync();

            var result = DoCalculation(input);

            if (result != SampleResult)
            {
                Debugger.Break();
                throw new InvalidOperationException($"Assertion failed! Current result: {result}, expected: {SampleResult}");
            }
        }

        protected virtual async Task<string> GetCheckInputAsync()
        {
            var dayMatch = Regex.Match(GetType().FullName, ".Day([0-9][0-9]).");

            if (!dayMatch.Success)
            {
                throw new InvalidOperationException("Invalid namespace used");
            }

            var day = int.Parse(dayMatch.Groups[1].Value);
            var filename = $"Day{day:D02}\\input.txt";

            if (!File.Exists(filename))
            {
                var session = await File.ReadAllTextAsync(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".AoC.cookie"));
                using var client = new HttpClient();
                client.DefaultRequestHeaders.Add("Cookie", $"session={session.Trim()}");
                using var response = await client.GetAsync($"https://adventofcode.com/2022/day/{day}/input");
                response.EnsureSuccessStatusCode();

                var content = (await response.Content.ReadAsStringAsync()).Replace("\n", Environment.NewLine);
                Directory.CreateDirectory(Path.GetDirectoryName(filename));
                await File.WriteAllTextAsync(filename, content);

                return content;
            }
            else
            {
                return await File.ReadAllTextAsync(filename);
            }
        }
    }
}
