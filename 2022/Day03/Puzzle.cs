namespace AdventOfCode.Day03
{
    internal class Puzzle : PuzzleBase
    {
        // -- CALCULATION PARAMETERS AND RESULT ------------------------------------

        protected override string SampleResult => "70"; // PART 1: "157";

        // -------------------------------------------------------------------------

        private int GetPoints(char ch) => ch > 'Z' ? ch - 'a' + 1 : ch - 'A' + 27;

        protected override string DoCalculation(string input)
        {
            var rucksacks = input
                .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
                .ToList();

            // PART 1: return Calculate1(rucksacks).ToString();
            return Calculate2(rucksacks).ToString();
        }

        private int Calculate1(IList<string> rucksacks)
        {
            var points = 0;

            foreach (var rucksack in rucksacks)
            {
                var c1 = rucksack.Substring(0, rucksack.Length / 2);
                var c2 = rucksack.Substring(rucksack.Length / 2);

                var i = c1.Intersect(c2).Single();

                points += GetPoints(i);
            }

            return points;
        }

        private int Calculate2(IList<string> rucksacks)
        {
            var points = 0;

            for (int r = 0; r < rucksacks.Count; r += 3)
            {
                var i = rucksacks[r].Intersect(rucksacks[r + 1]).Intersect(rucksacks[r + 2]).Single();

                points += GetPoints(i);
            }

            return points;
        }
    }
}
