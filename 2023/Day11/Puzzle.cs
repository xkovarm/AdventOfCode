namespace AdventOfCode.Day11;

internal class Puzzle : PuzzleBase
{
    // -- CALCULATION PARAMETERS AND RESULT ------------------------------------

    protected override string SampleResult => "82000210"; // "374";

    // -------------------------------------------------------------------------

    protected override string DoCalculation(string input)
    {
        // PART 1:
        //var galaxies = GetGalaxies(input, 2).ToList();
        var galaxies = GetGalaxies(input, 1000000).ToList();

        var result = 0L;

        for (int i = 0; i < galaxies.Count; i++)
        {
            for (int j = i + 1; j < galaxies.Count; j++)
            {
                result += galaxies[i].GetDistance(galaxies[j]);
            }
        }

        return result.ToString();
    }

    private IEnumerable<Coord> GetGalaxies(string input, int expandSpaces)
    {
        var lines = input
            .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
            .ToList();

        var (spaceX, spaceY) = ScanSpaces(lines);

        long realY = 0;

        for (int y = 0; y < lines.Count; y++)
        {
            if (spaceY.Contains(y))
            {
                realY += expandSpaces;
                continue;
            }

            long realX = 0;

            for (int x = 0; x < lines[y].Length; x++)
            {
                if (spaceX.Contains(x))
                {
                    realX += expandSpaces;
                    continue;
                }

                if (lines[y][x] == '#')
                {
                    yield return new(realX, realY);
                }

                realX++;
            }

            realY++;
        }
    }

    private (HashSet<int> SpacesX, HashSet<int> SpacesY) ScanSpaces(List<string> lines)
    {
        var rows = Enumerable.Range(0, lines.Count).ToHashSet();
        var cols = Enumerable.Range(0, lines[0].Length).ToHashSet();

        for (int l = 0; l < lines.Count; l++)
        {
            var indexes = GetAllIndexes('#', lines[l]).ToList();
            if (indexes.Any())
            {
                rows.Remove(l);
                indexes.ForEach(i => cols.Remove(i));
            }
        }

        return (cols, rows);
        
        IEnumerable<int> GetAllIndexes(char ch, string s)
        {
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == ch)
                {
                    yield return i;
                }
            }
        }
    }

    private record Coord(long X, long Y)
    {
        public long GetDistance(Coord coord)
        {
            return Math.Abs(coord.X - X) + Math.Abs(coord.Y - Y);
        }
    }
}
