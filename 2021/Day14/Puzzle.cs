namespace AdventOfCode.Day14
{
    internal class Puzzle : PuzzleBase
    {
        protected override string SampleResult => "2188189693529"; // "1588";

        protected override Task<string> GetSampleInputAsync()
        {
            return Task.FromResult(@"NNCB

CH -> B
HH -> N
CB -> H
NH -> C
HB -> C
HC -> B
HN -> C
NN -> C
BH -> H
NC -> B
NB -> B
BN -> B
BB -> N
BC -> B
CC -> N
CN -> C");
        }

        protected override string DoCalculation(string input)
        {
            var generations = 40; // 10;

            var lines = input.
                Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

            var polymerTemplate = lines[0].Trim();

            var rules = lines[1..]
                .Select(l => l.Split("->"))
                .ToDictionary(l => l[0].Trim(), l => l[1].Trim());

            var currentGeneration = rules.Keys.ToDictionary(r => r, r => 0L);
            var nextGeneration = rules.Keys.ToDictionary(r => r, r => 0L);

            for (int i = 0; i < polymerTemplate.Length - 1; i++)
            {
                currentGeneration[polymerTemplate.Substring(i, 2)]++;
            }

            for (int i = 0; i < generations; i++)
            {
                currentGeneration.ForEach(r =>
                {
                    var pair = r.Key;           // "AB"
                    var rule = rules[pair];     // "C"

                    nextGeneration[$"{pair[0]}{rule}"] += r.Value;  // "AC"
                    nextGeneration[$"{rule}{pair[1]}"] += r.Value;  // "CB"
                });

                (currentGeneration, nextGeneration) = (nextGeneration, currentGeneration);
                nextGeneration.Keys.ForEach(k => nextGeneration[k] = 0);
            }

            var counts = currentGeneration
                .GroupBy(x => x.Key[0])
                // do not forget to handle the last character of the template
                .Select(x => (Character: x.Key, Count: x.Select(y => y.Value).Sum() + (x.Key == polymerTemplate.Last() ? 1 : 0)))
                .OrderBy(x => x.Count)
                .ToList();

            return (counts.Last().Count - counts.First().Count).ToString();
        }        
    }
}
