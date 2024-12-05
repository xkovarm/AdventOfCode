namespace AdventOfCode.Day05;

internal class Puzzle : PuzzleBase
{
    // -- CALCULATION PARAMETERS AND RESULT ------------------------------------

    protected override string SampleResult => "123"; // PART 1: "143";

    // -------------------------------------------------------------------------

    protected override string DoCalculation(string input)
    {
        var list = input
            .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
            .ToList();

        var orders = list
            .Where(l => l.Contains('|'))
            .Select(l => l.Split('|').Select(int.Parse).ToArray())
            .Select(l => new Order(l[0], l[1]))
            .ToHashSet();

        var sequences = list
            .Where(l => !l.Contains("|"))
            .Select(l => l.Split(',').Select(int.Parse).ToArray())
            .ToList();

        var comparer = new Comparer(orders);

        var result = sequences
            .Select(s => new { Sequence = s, Ordered = s.OrderBy(s => s, comparer).ToList() })
            // PART 1: .Where(s => s.Sequence.SequenceEqual(s.Ordered))
            .Where(s => !s.Sequence.SequenceEqual(s.Ordered))
            .Sum(s => s.Ordered[s.Ordered.Count / 2]);

        return result.ToString();
    }

    private class Comparer : IComparer<int>
    {
        private readonly HashSet<Order> _orders;

        public Comparer(HashSet<Order> orders)
        {
            _orders = orders;
        }

        public int Compare(int x, int y)
        {
            if (x == y) return 0;

            if (_orders.Contains(new Order(x, y)))
            {
                return -1;
            }
            else if (_orders.Contains(new Order(y, x)))
            {
                return 1;
            }
            else
            {
                throw new InvalidOperationException();
            }
        }
    }

    private record Order(int Page1, int Page2);
}
