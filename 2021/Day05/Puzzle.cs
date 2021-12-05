namespace AdventOfCode.Day05
{
    internal class Puzzle : PuzzleBase
    {
        protected override string SampleResult => "12"; // "5";

        protected override string DoCalculation(string input)
        {
            var data = input
                .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
                .Select(d => d.Replace(" -> ", ",").Split(','))
                .Select(d => (X1: int.Parse(d[0]), Y1: int.Parse(d[1]), X2: int.Parse(d[2]), Y2: int.Parse(d[3])))
                .ToArray();

            var selection = data.ToArray(); // data.Where(d => d.X1 == d.X2 || d.Y1 == d.Y2).ToArray();

            var maxX = Math.Max(selection.Select(s => s.X1).Max(), selection.Select(s => s.X2).Max());
            var maxY = Math.Max(selection.Select(s => s.Y1).Max(), selection.Select(s => s.Y2).Max());

            var field = new byte[maxY+1, maxX+1];

            foreach (var line in selection) 
            {
                var deltaX = line.X2 - line.X1;
                var deltaY = line.Y2 - line.Y1;

                var diffX = Math.Abs(deltaX);
                var diffY = Math.Abs(deltaY);

                decimal stepX = diffX > diffY ? deltaX / diffX : deltaX / diffY;
                decimal stepY = diffX > diffY ? deltaY / diffX : deltaY / diffY;

                var x = (decimal)line.X1;
                var y = (decimal)line.Y1;

                while (x != line.X2 || y != line.Y2)
                {
                    field[(int)y, (int)x] += 1;
                    x += stepX;
                    y += stepY;
                }

                field[(int)y, (int)x] += 1;
            }

            int count = 0;
            for (int y = 0; y <= maxY; y++)
            {
                for (int x = 0; x <= maxX; x++)
                {
                    if (field[y, x] > 1)
                    {
                        count++;
                    }
                }
            }

            return count.ToString();            
        }    

        protected override Task<string> GetCheckInputAsync()
        {
            return File.ReadAllTextAsync(@"Day05\input.txt");
        }

        protected override Task<string> GetSampleInputAsync()
        {
            return Task.FromResult(@"0,9 -> 5,9
8,0 -> 0,8
9,4 -> 3,4
2,2 -> 2,1
7,0 -> 7,4
6,4 -> 2,0
0,9 -> 2,9
3,4 -> 1,4
0,0 -> 8,8
5,5 -> 8,2
");
        }
    }
}
