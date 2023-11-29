namespace AdventOfCode.Day17
{
    internal class Puzzle : PuzzleBase
    {
        // -- CALCULATION PARAMETERS AND RESULT ------------------------------------

        protected override string SampleResult => "3068";

        // -------------------------------------------------------------------------

        protected override Task<string> GetSampleInputAsync()
        {
            return Task.FromResult(">>><<><>><<<>><>>><<<>>><<<><<<>><>><<>>");
        }

        private List<List<(int X, int Y)>> Rocks = new ()
        {
            new () { (0, 0), (1, 0), (2, 0), (3, 0) },
            new () { (1, 0), (0, 1), (1, 1), (2, 1), (1, 2) },
            new () { (0, 0), (1, 0), (2, 0), (2, 1), (2, 2) },
            new () { (0, 0), (0, 1), (0, 2), (0, 3) },
            new () { (0, 0), (0, 1), (1, 0), (1, 1) }
        };

        protected override string DoCalculation(string input)
        {
            input = input.Trim();

            int jet = 0;

            HashSet<(int X, int Y)> area = new();

            var rock = 0;

            int yMax = !area.Any() ? -1 : area.Select(a => a.Y).Max();

            int x = 2;
            int y = yMax + 4;

            int counter = 0;

            while (true)
            {
                if (input[jet] == '<')
                {
                    if (x - 1 + Rocks[rock].Select(r => r.X).Min() >= 0 && !Rocks[rock].Any(r => area.Contains((x - 1 + r.X, y + r.Y))))
                    {
                        x -= 1;
                    }
                }
                else if (x + 1 + Rocks[rock].Select(r => r.X).Max() < 7 && !Rocks[rock].Any(r => area.Contains((x + 1 + r.X, y + r.Y))))
                {
                    x += 1;
                }

                jet = (jet + 1) % input.Length;

                if (y - 1 == -1 || Rocks[rock].Any(r => area.Contains((x + r.X, y - 1 + r.Y))))
                {
                    Rocks[rock].ForEach(r =>
                    {
                        area.Add((x + r.X, y + r.Y));
                    });

                    yMax = !area.Any() ? -1 : area.Select(a => a.Y).Max();

                    x = 2;
                    y = yMax + 4;
                    
                    rock = (rock + 1) % 5;

                    counter++;

                    if (counter == 1010 /* 1000000000000*/)
                    {
                        return (yMax + 1).ToString();
                    }

                    continue;
                }

                y--;
            }          
        }
    }
}
