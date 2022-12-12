using MoreLinq;

namespace AdventOfCode.Day12
{
    internal class Puzzle : PuzzleBase
    {
        // -- CALCULATION PARAMETERS AND RESULT ------------------------------------

        protected override string SampleResult => "29"; // PART 1: "31"

        // -------------------------------------------------------------------------

        protected override string DoCalculation(string input)
        {
            var area = input
                .Split($"{Environment.NewLine}", StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.ToArray())
                .ToArray();

            var maxX = area[0].Length;
            var maxY = area.Length;

            var important = Enumerable.Range(0, maxY)
                .SelectMany(row => Enumerable.Range(0, maxX).Select(col => (X: col, Y: row)))
                .Where(a => area[a.Y][a.X] is 'S' or 'E' or 'a')
                .Select(n => new { Type = area[n.Y][n.X] switch { 'E' => 0, 'S' => 1, 'a' => 2 }, Coord = (n.X, n.Y) })
                .OrderBy(n => n.Type)
                .ToArray();

            // PART 1: var starts = new[] { important[1] };
            var starts = important[1..];

            var result = starts.Select(s => CalculateRoute(s.Coord, important[0].Coord)).Where(a => a.HasValue).Min(a => a.Value);

            return result.ToString();

            int? CalculateRoute((int X, int Y) start, (int X, int Y) end)
            {
                var nodes = new Node[maxX, maxY];
                nodes[start.X, start.Y] = new Node { Visited = false, MinStepsToStart = 0 };

                var queue = new PriorityQueue<(int X, int Y), int>();
                queue.Enqueue(start, 0);

                do
                {
                    var current = queue.Dequeue();
                    var currentNode = nodes[current.X, current.Y];

                    GetNeighbors(current.X, current.Y)
                        .OrderBy(n => n.Energy)
                        .ForEach(following =>
                        {
                            var followingNode = nodes[following.X, following.Y];
                            if (followingNode == null)
                            {
                                followingNode = nodes[following.X, following.Y] = new Node();
                            }

                            if (!followingNode.Visited)
                            {
                                if (followingNode.MinStepsToStart == null || currentNode.MinStepsToStart + 1 < followingNode.MinStepsToStart)
                                {
                                    followingNode.MinStepsToStart = currentNode.MinStepsToStart + 1;

                                    if (!queue.UnorderedItems.Any(q => q.Element.X == following.X && q.Element.Y == following.Y))
                                    {
                                        queue.Enqueue((following.X, following.Y), followingNode.MinStepsToStart!.Value);
                                    }
                                }
                            }

                        });

                    currentNode.Visited = true;

                    if (current == end)
                    {
                        break;
                    }


                } while (queue.Count > 0);

                return nodes[end.X, end.Y]?.MinStepsToStart;
            }

            IEnumerable<(int X, int Y, char Energy)> GetNeighbors(int x, int y)
            {
                var energy = GetEnergy(x, y);

                if (x > 0 && GetEnergy(x - 1, y) <= energy + 1)
                {
                    yield return (x - 1, y, GetEnergy(x - 1, y));
                }
                if (y > 0 && GetEnergy(x, y - 1) <= energy + 1)
                {
                    yield return (x, y - 1, GetEnergy(x, y - 1));
                }
                if (x < maxX - 1 && GetEnergy(x + 1, y) <= energy + 1)
                {
                    yield return (x + 1, y, GetEnergy(x + 1, y));
                }
                if (y < maxY - 1 && GetEnergy(x, y + 1) <= energy + 1)
                {
                    yield return (x, y + 1, GetEnergy(x, y + 1));
                }
            }

            char GetEnergy(int x, int y) => area[y][x] switch
            {
                'S' => 'a',
                'E' => 'z',
                var ch => ch
            };
        }

        private class Node
        {
            public bool Visited { get; set; }
            public int? MinStepsToStart { get; set; }
        }
    }
}
