using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace AdventOfCode
{
    internal abstract class PuzzleBase
    {
        protected abstract string SampleResult { get; }

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

        protected virtual Task<string> GetSampleInputAsync()
        {
            return GetFile("Day{0:D02}\\sample.txt", "https://adventofcode.com/2022/day/{0}",
                content =>
                {
                    var doc = new HtmlDocument();
                    doc.LoadHtml(content);

                    var node = doc.DocumentNode.SelectSingleNode("//pre/code");

                    return node.InnerText;
                });
        }

        protected virtual Task<string> GetCheckInputAsync()
        {
            return GetFile("Day{0:D02}\\input.txt", "https://adventofcode.com/2022/day/{0}/input");
        }

        protected virtual async Task<string> GetFile(string targetFilePattern, string urlPattern, Func<string, string>? transformContent = null)
        {
            var dayMatch = Regex.Match(GetType().FullName, ".Day([0-9][0-9]).");

            if (!dayMatch.Success)
            {
                throw new InvalidOperationException("Invalid namespace used");
            }

            var day = int.Parse(dayMatch.Groups[1].Value);
            var filename = string.Format(targetFilePattern, day);

            if (!File.Exists(filename))
            {
                var session = await File.ReadAllTextAsync(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".AoC.cookie"));
                using var client = new HttpClient();
                client.DefaultRequestHeaders.Add("Cookie", $"session={session.Trim()}");
                using var response = await client.GetAsync(string.Format(urlPattern, day));
                response.EnsureSuccessStatusCode();

                var content = (await response.Content.ReadAsStringAsync()).Replace("\n", Environment.NewLine);

                content = transformContent?.Invoke(content) ?? content;

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
