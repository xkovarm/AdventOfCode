namespace AdventOfCode.Day07;

internal class Puzzle : PuzzleBase
{
    // -- CALCULATION PARAMETERS AND RESULT ------------------------------------

    protected override string SampleResult => "5905"; // PART 1: "6440";

    // PART 1: private const string CardValues = "23456789TJQKA";
    private const string CardValues = "J23456789TQKA";

    // -------------------------------------------------------------------------

    protected override string DoCalculation(string input)
    {
        var hands = Parse(input);

        var ordered = hands.Order(new HandsComparer()).ToList();

        var rank = Enumerable.Range(1, ordered.Count).Zip(ordered, (r, o) => r * o.Rank).Sum();

        return rank.ToString();
    }

    private List<Hand> Parse(string input)
    {
        return input
            .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
            .Select(l => l.Split(' ', StringSplitOptions.RemoveEmptyEntries))
            .Select(l => new Hand(l[0], int.Parse(l[1])))
            .ToList();
    }

    private class HandsComparer : IComparer<Hand>
    {
        public int Compare(Hand? x, Hand? y)
        {
            if (x == null || y == null)
            {
                throw new InvalidOperationException();
            }

            if (x.Value < y.Value)
            {
                return -1;
            } 
            else if (x.Value > y.Value)
            {
                return 1;
            }

            for (int i = 0; i < x.Cards.Length; i++)
            {
                var xi = CardValues.IndexOf(x.Cards[i]);
                var yi = CardValues.IndexOf(y.Cards[i]);

                if (xi < yi)
                {
                    return -1;
                } 
                else if (xi > yi)
                {
                    return 1;
                }
            }

            return 0;
        }
    }

    private record Hand(string Cards, int Rank)
    {
        public int Value { get; init; } = EvaluateHand(Cards);

        private static int EvaluateHand(string cards)
        {
            var groups = cards.GroupBy(a => a).Select(a => new { Card = a.Key, Count = a.Count() });

            // Modification for PART 2, comment the following 3 lines out for PART 1:
            var strongestGroup = groups.OrderByDescending(g => g.Count).FirstOrDefault(g => g.Card != 'J');
            var replacedJoker = strongestGroup != null ? cards.Replace('J', strongestGroup.Card) : cards;
            groups = replacedJoker.GroupBy(a => a).Select(a => new { Card = a.Key, Count = a.Count() });

            if (groups.Any(g => g.Count == 5))
            {
                return 7;
            }

            if (groups.Any(g => g.Count == 4))
            {
                return 6;
            }

            if (groups.Any(g => g.Count == 3) && groups.Any(g => g.Count == 2))
            {
                return 5;
            }

            if (groups.Any(g => g.Count == 3))
            {
                return 4;
            }

            if (groups.Where(g => g.Count == 2).Count() == 2)
            {
                return 3;
            }

            if (groups.Any(g => g.Count == 2))
            {
                return 2;
            }

            return 1;
        }
    }
}
