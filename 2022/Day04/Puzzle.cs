namespace AdventOfCode.Day04
{
    internal class Puzzle : PuzzleBase
    {
        // -- CALCULATION PARAMETERS AND RESULT ------------------------------------

        protected override string SampleResult => "4"; // PART 1 :"2"; 

        // -------------------------------------------------------------------------

        protected override string DoCalculation(string input)
        {
            var sections = input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
                .Select(i => i.Split(','))
                .Select(i => new[] { i[0].Split('-'), i[1].Split('-') })
                .Select(i => new[] { (From: int.Parse(i[0][0]), To: int.Parse(i[0][1])), (From: int.Parse(i[1][0]), To: int.Parse(i[1][1])) })
                .ToList();

            var count = 0;

            foreach (var section in sections)
            {
                var s1 = GetString(section[0].From, section[0].To);
                var s2 = GetString(section[1].From, section[1].To);

                // PART 1: count += Compare(s1, s2) ? 1 : 0;
                count += Compare2(s1, s2) ? 1 : 0;
            }
           
            return count.ToString();

            string GetString(int from, int to)
            {
                var chars = new char[to - from + 1];

                int index = 0;

                for (int i = from; i <= to; i++)
                {
                    chars[index++] = (char) (i+31);
                }

                return new string(chars);
            }

            bool Compare(string s1, string s2)
            {
                return s1.Contains(s2) || s2.Contains(s1);
            }

            bool Compare2(string s1, string s2)
            {
                return s1.Intersect(s2).Any();
            }

        }
    }
}
