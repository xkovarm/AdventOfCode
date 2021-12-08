namespace AdventOfCode.Day08
{
    internal class Puzzle : PuzzleBase
    {
        protected override string SampleResult => "61229"; // "26";

        private readonly List<string> Segments = new List<string>(new[] { "abcefg", "cf", "acdeg", "acdfg", "bcdf", "abdfg", "abdefg", "acf", "abcdefg", "abcdfg" });

        public Puzzle()
        {
            Debug.Assert(DoCalculation("acedgfb cdfbe gcdfa fbcad dab cefabd cdfgeb eafb cagedb ab | cdfeb fcadb cdfeb cdbaf") == "5353");
        }

        protected override Task<string> GetCheckInputAsync()
        {
            return File.ReadAllTextAsync(@"Day08\input.txt");
        }

        protected override Task<string> GetSampleInputAsync()
        {
            return File.ReadAllTextAsync(@"Day08\sample.txt");
        }

        protected override string DoCalculation(string input)
        {
            var data = input
                .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
                .Select(i => i.Split('|'))
                .Select(i => (Signals: i[0].Split(' ', StringSplitOptions.RemoveEmptyEntries), Digits: i[1].Split(' ', StringSplitOptions.RemoveEmptyEntries)))
                .ToArray();

            //return DoCalculation1(data);
            return DoCalculation2(data);
        }

        private string DoCalculation1((string[] Signals, string[] Digits)[] data)
        {
            var s = data.SelectMany(d => d.Digits.Where(s => s.Count() is 2 or 3 or 4 or 7)).Count();
            return s.ToString();
        }

        private string DoCalculation2((string[] Signals, string[] Digits)[] data)
        {
            int sum = 0;
            foreach (var item in data)
            {
                var mappings = GetSegmentsMapping(item.Signals);

                var value = string.Join("", item.Digits.Select(d => Convert.ToString(Segments.IndexOf(new string(d.Select(c => mappings[c]).OrderBy(c => c).ToArray())))).ToArray());

                sum += int.Parse(value);
            }

            return sum.ToString();

            Dictionary<char, char> GetSegmentsMapping(string[] signals)
            {
                var digits = new string[10];

                // map unique digits: "1", "4", "7" a "8"
                digits[1] = signals.Single(s => s.Length == 2);
                digits[4] = signals.Single(s => s.Length == 4);
                digits[7] = signals.Single(s => s.Length == 3);
                digits[8] = signals.Single(s => s.Length == 7);

                // "7" vs "1" -> determine segment A
                char segA = digits[7].Single(d => !digits[7].Intersect(digits[1]).Contains(d));

                // "0" | "6" | "9" vs "1" and "4"
                var sel = signals.Where(s => s.Length == 6).ToArray();
                digits[6] = sel.Single(s => s.Intersect(digits[1]).Count() == 1);
                digits[9] = sel.Single(s => s.Intersect(digits[4]).Count() == 4);
                digits[0] = sel.Single(s => s != digits[6] && s != digits[9]);

                // "1" vs "6" -> segments C and F
                char segF = digits[1].Intersect(digits[6]).Single();
                char segC = digits[1].Single(d => d != segF);

                // map "2", "3" and "5" using known segments
                sel = signals.Where(s => s.Length == 5).ToArray();
                digits[2] = sel.Single(s => s.Contains(segC) && !s.Contains(segF));
                digits[3] = sel.Single(s => s.Contains(segC) && s.Contains(segF));
                digits[5] = sel.Single(s => !s.Contains(segC) && s.Contains(segF));

                // map remaining segments
                var segE = digits[6].Single(c => !digits[6].Intersect(digits[5]).Contains(c));
                var segB = digits[0].Where(c => !digits[0].Intersect(digits[3]).Contains(c)).Single(c => c != segE);
                var segD = digits[4].Single(c => c != segB && c != segC && c != segF);
                var segG = digits[5].Single(c => c != segA && c != segB && c != segD && c != segF);

                return new Dictionary<char, char>
                {
                    [segA] = 'a',
                    [segB] = 'b',
                    [segC] = 'c',
                    [segD] = 'd',
                    [segE] = 'e',
                    [segF] = 'f',
                    [segG] = 'g'
                };
            }
        }
    }
}
