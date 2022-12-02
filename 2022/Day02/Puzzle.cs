namespace AdventOfCode.Day02
{
    internal class Puzzle : PuzzleBase
    {
        // -- CALCULATION PARAMETERS AND RESULT ------------------------------------

        protected override string SampleResult => "12"; // PART 1: "15";

        // -------------------------------------------------------------------------

        protected override string DoCalculation(string input)
        {
            var rounds = input
                .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
                .Select(l => l.Split(" "))
                .Select(l => (l[0], l[1]))
                .ToList();

            var points = Play(rounds);

            return points.ToString();
        }

        private int Play(IList<(string He, string Me)> rounds)
        {
            var points = 0;

            foreach (var round in rounds)
            {
                // PART 1: var me = Translate1(round.Me);
                var me = Translate2(round.Me, round.He);

                points += me switch
                {
                    "A" => 1,
                    "B" => 2,
                    "C" => 3
                };

                if (round.He == me)
                {
                    points += 3;
                } 
                else if ((me, round.He) == ("A", "C") || (me, round.He) == ("B", "A") || (me, round.He) == ("C", "B")) 
                {
                    points += 6;
                }
            }

            return points;

            string Translate1(string me)
            {
                return me switch
                {
                    "X" => "A",
                    "Y" => "B",
                    "Z" => "C",
                };
            }

            string Translate2(string me, string he)
            {
                return me switch
                {
                    "X" => he switch { "A" => "C", "B" => "A", "C" => "B" },
                    "Y" => he switch { "A" => "A", "B" => "B", "C" => "C" },
                    "Z" => he switch { "A" => "B", "B" => "C", "C" => "A" },
                };
            }
        }
    }
}
