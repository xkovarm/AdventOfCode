using System.Data;

namespace AdventOfCode.Day05;

internal class Puzzle : PuzzleBase
{
    // -- CALCULATION PARAMETERS AND RESULT ------------------------------------

    protected override string SampleResult => "46"; // PART 1: "35";

    // -------------------------------------------------------------------------

    protected override string DoCalculation(string input)
    {
        var (seeds, maps) = Parse(input);

        // PART 1
        //foreach (var map in maps)
        //{
        //    ApplyMap(seeds, map);
        //}
        //
        //return seeds.Min().ToString();

        var seedBlocks = GetSeedBlocks(seeds);

        foreach (var map in maps)
        {
            seedBlocks = ApplyMap2(seedBlocks, map);
        }

        return seedBlocks.Select(s => s.Start).Min().ToString();

        IEnumerable<Seeds> GetSeedBlocks(List<long> seeds)
        {
            for (int i = 0; i < seeds.Count; i += 2)
            {
                yield return new Seeds(seeds[i], seeds[i + 1]);
            }
        }
    }

    private IEnumerable<Seeds> ApplyMap2(IEnumerable<Seeds> seedBlocks, List<Mapping> map)
    {
        foreach (var block in seedBlocks)
        {
            var affectedMappings = map
                .Where(m =>
                    m.From >= block.From && m.From <= block.To
                    || m.To >= block.From && m.To <= block.To
                    || block.From >= m.From && block.To <= m.To)
                .OrderBy(a => a.From)
                .ToList();

            var current = block.From;
            var end = block.To;

            while (current < end)
            {
                if (affectedMappings.Any() && affectedMappings.First().From <= current && current <= affectedMappings.First().To)
                {
                    var mapping = affectedMappings.First();
                    affectedMappings.RemoveAt(0);
                    var length = Math.Min(end, mapping.To) - current + 1;

                    yield return new Seeds(mapping.Transform(current), length);

                    current += length;
                }
                else
                {
                    var followingMapping = affectedMappings.FirstOrDefault();
                    var length = (followingMapping != null ? followingMapping.From : end + 1) - current;

                    yield return new Seeds(current, length);

                    current += length;
                }
            }
        }
    }

    // Original Part 1
    private void ApplyMap(List<long> seeds, List<Mapping> map)
    {
        for (int i = 0; i < seeds.Count; i++)
        {
            var mapping = map.Where(m => seeds[i] >= m.Source && seeds[i] < m.Source + m.Length).SingleOrDefault();

            if (mapping != null)
            {
                seeds[i] = mapping.Transform(seeds[i]);
            }
        }
    }

    private (List<long> Seeds, List<List<Mapping>> Maps) Parse(string input)
    {
        var blocks = input
            .Split($"{Environment.NewLine}{Environment.NewLine}", StringSplitOptions.RemoveEmptyEntries)
            .ToList();

        var seeds = blocks[0]
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)[1..]
            .Select(long.Parse)
            .ToList();

        var maps = blocks[1..]
            .Select(b => b.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)[1..])
            .Select(q => q.Select(s => s.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray()))
            .Select(q => q.Select(s => new Mapping(s[1], s[0], s[2])).ToList())
            .ToList();

        return (seeds, maps);
    }

    private record Mapping(long Source, long Destination, long Length)
    {
        public long From => Source;
        public long To => Source + Length - 1;

        public long Transform(long value)
        {
            return Destination + value - Source;
        }
    }

    private record Seeds(long Start, long Length)
    {
        public long From => Start;
        public long To => Start + Length - 1;
    }
}
