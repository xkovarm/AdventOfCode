using MoreLinq;

namespace AdventOfCode.Day03;

internal class Puzzle : PuzzleBase
{
    // -- CALCULATION PARAMETERS AND RESULT ------------------------------------

    protected override string SampleResult => "467835"; // PART 1: "4361";

    // -------------------------------------------------------------------------

    protected override string DoCalculation(string input)
    {
        var schematic = ParseFile(input);

        var parts = schematic.OfType<Part>().ToList();
        var symbols = schematic.OfType<Symbol>().ToList();      

        var mapOfItems = schematic
            .Select(i => i.GetCoords().Select(c => new { Item = i, Coord = c }))
            .SelectMany(i => i)
            .ToDictionary(i => i.Coord, i => i.Item);

        // PART 1
        //var result = parts
        //    .Where(p => mapOfItems.Keys.Intersect(p.GetNeighbours()).Any())
        //    .Sum(p => p.Value);

        // PART 2
        long result = 0;

        foreach (var symbol in symbols.Where(s => s.Value == '*'))
        {
            var neighbours = mapOfItems
                .Keys
                .Intersect(symbol.GetNeighbours())
                .Select(n => mapOfItems[n])
                .OfType<Part>()
                .Distinct()
                .ToList();

            if (neighbours.Count == 2)
            {
                result += neighbours[0].Value * neighbours[1].Value;
            }
        }

        return result.ToString();
    }

    private List<Item> ParseFile(string input)
    {
        var parsed = new List<Item>();
        int y = 0;

        foreach (var line in input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries))
        {
            for (int x = 0; x < line.Length; x++)
            {
                if (line[x] != '.')
                {
                    if (IsNumber(line[x]))
                    {
                        int length = 0;

                        while (x + length < line.Length && IsNumber(line[x + length])) 
                        {
                            length++;
                        }

                        parsed.Add(new Part { Reference = new Point(x, y), Length = length, Value = int.Parse(line.Substring(x, length)) });

                        x += length - 1;
                    } 
                    else
                    {
                        parsed.Add(new Symbol { Reference = new Point(x, y), Length = 1, Value = line[x] });
                    }
                }
            }

            y++;
        }

        return parsed;

        bool IsNumber(char ch) => ch >= '0' && ch <= '9';
    }

    private record Point(int X, int Y);

    private class Item
    {
        public Point Reference { get; set; }

        public int Length { get; set; }

        public IEnumerable<Point> GetCoords()
        {
            return Enumerable.Range(Reference.X, Length).Select(r => Reference with { X = r });
        }

        public IEnumerable<Point> GetNeighbours()
        {
            for (int x = Reference.X - 1; x <= Reference.X + Length; x++)
            {
                yield return Reference with { X = x, Y = Reference.Y - 1 };
            }

            yield return Reference with { X = Reference.X - 1 };
            yield return Reference with { X = Reference.X + Length };

            for (int x = Reference.X - 1; x <= Reference.X + Length; x++)
            {
                yield return Reference with { X = x, Y = Reference.Y + 1 };
            }
        }
    }

    private class Part : Item
    {
        public int Value { get; set; }
    }

    private class Symbol : Item
    {
        public char Value { get; set; }
    }
}
