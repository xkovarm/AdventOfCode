namespace AdventOfCode.Day23
{
    [ForcePuzzle]
    internal class Puzzle : PuzzleBase
    {
        // -- CALCULATION PARAMETERS AND RESULT ------------------------------------

        protected override string SampleResult => "20"; //"110";

        // -------------------------------------------------------------------------

        protected override string DoCalculation(string input)
        {
            var lines = input
                .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
                .ToArray();

            var elves = Parse(lines).ToHashSet();

            var result = Calculate(elves);

            return result.ToString();
        }

        private int Calculate(HashSet<(int X, int Y)> elves) 
        {
            for (int i = 1; i <= 1000000; i++)
            {
                var next = elves.Select(e => new { Current = e, Next = GetNext(e, (i-1) % 4) }).ToList();

                var freeTargets = next
                    .Where(n => n.Next != null)
                    .GroupBy(n => n.Next)
                    .Select(n => new { n.Key, Count = n.Count() })
                    .Where(n => n.Count == 1)
                    .Select(n => n.Key)
                    .ToHashSet();

                var moves = next.Where(n => freeTargets.Contains(n.Next)).ToList();

                var remove = moves.Select(m => m.Current).ToHashSet();

                if (!remove.Any())
                {
                    return i;
                }

                elves.RemoveWhere(e => remove.Contains(e));

                moves.Select(m => m.Next).ToList().ForEach(m => elves.Add(m!.Value));
            }

            var width = elves.Select(e => e.X).Max() - elves.Select(e => e.X).Min() + 1;
            var height = elves.Select(e => e.Y).Max() - elves.Select(e => e.Y).Min() + 1;
            
            return width * height - elves.Count();

            (int X, int Y)? GetNext((int X, int Y) c, int offset)
            {
                var followers = new (int X, int Y)?[] { 
                    !elves.Contains((c.X - 1, c.Y - 1)) && !elves.Contains((c.X, c.Y - 1)) && !elves.Contains((c.X + 1, c.Y - 1))
                        ? (c.X, c.Y - 1) : null,
                    !elves.Contains((c.X - 1, c.Y + 1)) && !elves.Contains((c.X, c.Y + 1)) && !elves.Contains((c.X + 1, c.Y + 1))
                        ? (c.X, c.Y + 1) : null,
                    !elves.Contains((c.X - 1, c.Y - 1)) && !elves.Contains((c.X - 1, c.Y)) && !elves.Contains((c.X - 1, c.Y + 1))
                        ? (c.X - 1, c.Y) : null,
                    !elves.Contains((c.X + 1, c.Y - 1)) && !elves.Contains((c.X + 1, c.Y)) && !elves.Contains((c.X + 1, c.Y + 1))
                        ? (c.X + 1, c.Y) : null };

                var first = followers.Skip(offset);
                var second = followers.Take(offset);
                followers = first.Concat(second).ToArray();

                if (followers.All(f => f != null))
                {
                    return null;
                }

                return followers.FirstOrDefault(f => f != null);
            }
        }

        private IEnumerable<(int X, int Y)> Parse(string[] lines)
        {
            for (int y = 0; y < lines.Length; y++)
            {
                for (int x = 0; x < lines[y].Length; x++)
                {
                    if (lines[y][x] == '#')
                    {
                        yield return (x, y);
                    }
                }
            }
        }
    }
}
