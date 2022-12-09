namespace AdventOfCode.Day09
{
    internal class Puzzle : PuzzleBase
    {
        // -- CALCULATION PARAMETERS AND RESULT ------------------------------------

        protected override string SampleResult => "1"; // PART 1: "13";

        // -------------------------------------------------------------------------

        protected override Task<string> GetSampleInputAsync()
        {
            return Task.FromResult(@"R 4
U 4
L 3
D 1
R 4
D 1
L 5
R 2");
        }

        protected override string DoCalculation(string input)
        {
            var moves = input
                .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
                .Select(i => i.Split(' '))
                .Select(i => (i[0][0], int.Parse(i[1])))
                .ToList();

            // PART 1: var result = Play(moves, 2);
            var result = Play(moves, 10);

            return result.ToString();
        }

        private int Play(List<(char Direction, int Count)> moves, int count)
        {
            var visited = new HashSet<Point>();

            var knots = Enumerable.Repeat(new Point(0,0), count).ToArray();
            var head = 0;
            var tail = count-1;

            visited.Add(knots[tail]);

            foreach (var move in moves)
            {
                for (int i = 0; i < move.Count; i++)
                {
                    knots[head] = knots[head] + GetMove(move.Direction);
                   
                    for (int k = 1; k <= tail; k++)
                    {
                        if (!knots[k - 1].IsAdjacent(knots[k]))
                        {
                            knots[k] = MoveFollower(knots[k-1], knots[k]);
                        }
                    }

                    visited.Add(knots[tail]);
                }
            }

            return visited.Count();           

            Point GetMove(char direction) => direction switch
            {
                'U' => Point.Up,
                'D' => Point.Down,
                'L' => Point.Left,
                'R' => Point.Right,
                _ => throw new InvalidOperationException()
            };

            Point MoveFollower(Point leader, Point follower)
            {
                var diff = leader - follower;
                
                return new Point(follower.X + Math.Sign(diff.X), follower.Y + Math.Sign(diff.Y));
            }
        }
    }
}

file record Point(int X, int Y)
{
    public static Point operator +(Point p1, Point p2) => new Point(p1.X + p2.X, p1.Y + p2.Y);
    public static Point operator -(Point p1, Point p2) => new Point(p1.X - p2.X, p1.Y - p2.Y);

    public static Point Up = new Point(0, -1);
    public static Point Down = new Point(0, 1);
    public static Point Left = new Point(-1, 0);
    public static Point Right = new Point(1, 0);

    public bool IsAdjacent(Point p)
    {
        return X - p.X <= 1 && X - p.X >= -1 && Y - p.Y <= 1 && Y - p.Y >= -1;
    }
}