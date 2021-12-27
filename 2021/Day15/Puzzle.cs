namespace AdventOfCode.Day15
{
    internal class Puzzle : PuzzleBase
    {
        protected override string SampleResult => "315"; // "40";

        protected override Task<string> GetSampleInputAsync()
        {
            return Task.FromResult(@"1163751742
1381373672
2136511328
3694931569
7463417111
1319128137
1359912421
3125421639
1293138521
2311944581");
        }

        private class Node
        {
            public bool Visited { get; set; }
            public int? MinCostsToStart { get; set; }
            public (int X, int Y)? NearestNodeToStart { get; set; }
        }

        protected override string DoCalculation(string input)
        {
            var data = input
                .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
                .Select(c => c.Select(c => int.Parse($"{c}")).ToArray())
                .ToArray();

            data = MultiplyMatrix(5); // MultiplyMatrix(1);
            var maxX = data[0].Length;
            var maxY = data.Length;

            var start = (X: 0, Y: 0);
            var end = (X: maxX - 1, Y: maxY - 1);

            var nodes = new Node[maxX, maxY];
            nodes[start.X, start.Y] = new Node { Visited = false, MinCostsToStart = 0, NearestNodeToStart = start };

            var queue = new PriorityQueue<(int X, int Y), int>();
            queue.Enqueue(start, 0);

            // Dijkstra
            do
            {
                var current = queue.Dequeue();
                var currentNode = nodes[current.X, current.Y];

                GetNeighbors(current.X, current.Y)
                    .OrderBy(n => n.Costs)
                    .ToList()
                    .ForEach(following =>
                    {
                        var followingNode = nodes[following.X, following.Y];
                        if (followingNode == null)
                        {
                            followingNode = nodes[following.X, following.Y] = new Node();
                        }

                        if (!followingNode.Visited)
                        {
                            if (followingNode.MinCostsToStart == null || currentNode.MinCostsToStart + following.Costs < followingNode.MinCostsToStart)
                            {
                                followingNode.MinCostsToStart = currentNode.MinCostsToStart + following.Costs;
                                followingNode.NearestNodeToStart = current;

                                if (!queue.UnorderedItems.Any(q => q.Element.X == following.X && q.Element.Y == following.Y))
                                {
                                    queue.Enqueue((following.X, following.Y), followingNode.MinCostsToStart.Value);
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

            return nodes[end.X, end.Y].MinCostsToStart.ToString();
        
            IEnumerable<(int X, int Y, int Costs)> GetNeighbors(int x, int y)
            {
                if (x > 0)
                {
                    yield return (x - 1, y, data[y][x - 1]);
                } 
                if (y > 0)
                {
                    yield return (x, y - 1, data[y - 1][x]);
                }
                if (x < maxX-1)
                {
                    yield return (x + 1, y, data[y][x + 1]);
                }
                if (y < maxY-1)
                {
                    yield return (x, y + 1, data[y + 1][x]);
                }
            }

            int[][] MultiplyMatrix(int factor)
            {
                if (factor == 1)
                {
                    return data;
                }

                var maxX = data[0].Length;
                var maxY = data.Length;

                var newMatrix = new int[factor * maxY][];

                for (int y = 0; y < factor * maxY; y++)
                {
                    newMatrix[y] = new int[factor * maxX];

                    for (int x = 0; x < factor * maxX; x++)
                    {
                        var value = data[y % maxY][x % maxX] + (x / maxX) + (y / maxY);

                        if (value > 9)
                        {
                            value -= 9;
                        }

                        newMatrix[y][x] = value;
                    }
                }

                return newMatrix;
            }
        }
    }
}
