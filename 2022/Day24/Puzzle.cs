namespace AdventOfCode.Day24
{
    internal class Puzzle : PuzzleBase
    {
        // -- CALCULATION PARAMETERS AND RESULT ------------------------------------

        protected override string SampleResult => "54"; //"18"; 

        // -------------------------------------------------------------------------

        protected override Task<string> GetSampleInputAsync()
        {
            return Task.FromResult(@"#.######
#>>.<^<#
#.<..<<#
#>v.><>#
#<^v^^>#
######.#");
        }

        protected override string DoCalculation(string input)
        {
            var lines = input
                .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries) 
                .ToArray();

            var result = Calculate(lines);

            return result.ToString();
        }

        private int Calculate(string[] lines) 
        {
            var blizzards = Parse(lines).ToList();
            var max = (X: lines[0].Length - 2, Y: lines.Length - 2);
            var start = (lines[0].IndexOf('.'), 0);
            var end = (lines[lines.Length - 1].IndexOf("."), max.Y+1);

            return 
                Calculate(start, end)
                + Calculate(end, start)
                + Calculate(start, end);

            int Calculate((int X, int Y) start, (int X, int Y) end)
            {
                
                var pos = new HashSet<(int X, int Y)>();

                int time = 0;
                bool started = false;

                pos.Add(start);

                Dump();

                while (true)
                {
                    if (pos.Any(p => p == end))
                    {
                        return time;
                    }

                    Shuffle(blizzards);
                    time++;

                    var newPos = new List<(int X, int Y)>();

                    foreach (var p in pos)
                    {
                        newPos.AddRange(GetFollowers(p));
                    }

                    pos.Clear();
                    newPos.ForEach(a => pos.Add(a));

                    Dump();

                    if (pos.Count == 0)
                    {
                        int j = 1;
                    }
                }


                void Dump()
                {
                    return;
                    
                    Console.Clear();
                    Console.WriteLine(time);

                    for (int y = 0; y <= max.Y + 1; y++) {
                        for (int x = 0; x <= max.X + 1; x++)
                        {
                            if (pos.Any(p => p == (x, y)))
                            {
                                Console.Write('X');
                            } else if (blizzards.Any(b => b.Position == (x, y)))
                            {
                                Console.Write('o');
                            }
                            else if (x == 0 || y == 0 || x == max.X + 1 || y == max.Y + 1)
                            {
                                Console.Write('#');
                                continue;
                            }
                            else
                            {
                                Console.Write('.');
                            }
                        }
                        Console.WriteLine();
                    }

                    Console.WriteLine(":");
                    Console.ReadLine();

                }
            }

            IEnumerable<(int X, int Y)> GetFollowers((int X, int Y) pos)
            {
                if (pos.X == max.X && pos.Y == max.Y)
                {
                    yield return (max.X, pos.Y + 1);
                }

                if (pos.X == 1 && pos.Y == 1)
                {
                    yield return (1, 0);
                }

                if (pos.X < max.X && pos.Y >= 1 && pos.Y <= max.Y)
                {
                    var next = pos with { X = pos.X + 1 };
                    if (!blizzards.Any(b => b.Position == next))
                    {
                        yield return next;
                    }
                }

                if (pos.X > 1 && pos.Y >= 1 && pos.Y <= max.Y)
                {
                    var next = pos with { X = pos.X - 1 };
                    if (!blizzards.Any(b => b.Position == next))
                    {
                        yield return next;
                    }
                }

                if (pos.Y < max.Y)
                {
                    var next = pos with { Y = pos.Y + 1 };
                    if (!blizzards.Any(b => b.Position == next))
                    {
                        yield return next;
                    }
                }

                if (pos.Y > 1)
                {
                    var next = pos with { Y = pos.Y - 1 };
                    if (!blizzards.Any(b => b.Position == next))
                    {
                        yield return next;
                    }
                }

                if (!blizzards.Any(b => b.Position == pos))
                {
                    yield return pos;
                }
            }

            void Shuffle(List<Blizzard> blizzards)
            {
                foreach (var b in blizzards)
                {
                    switch (b.Dir)
                    {
                        case '<':
                            b.Position = b.Position with { X = b.Position.X > 1 ? b.Position.X - 1 : max.X };
                            break;
                        case '>':
                            b.Position = b.Position with { X = b.Position.X < max.X ? b.Position.X + 1 : 1 };
                            break;
                        case '^':
                            b.Position = b.Position with { Y = b.Position.Y > 1 ? b.Position.Y - 1 : max.Y };
                            break;
                        case 'v':
                            b.Position = b.Position with { Y = b.Position.Y < max.Y ? b.Position.Y + 1 : 1 };
                            break;
                    }
                };
            }
        }

        private IEnumerable<Blizzard> Parse(string[] lines)
        {
            for (int y = 1; y < lines.Count() - 1; y++)
            {
                for (int x = 1; x < lines[y].Count() - 1; x++)
                {
                    if (lines[y][x] == '.')
                    {
                        continue;
                    }

                    yield return new Blizzard { Position = (x, y), Dir = lines[y][x] };
                }
            }
        }

        private class Blizzard
        {
            public (int X, int Y) Position { get; set; }
            public char Dir { get; set; }
        }
    }
}
