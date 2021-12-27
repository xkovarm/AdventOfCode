namespace AdventOfCode.Day23
{
    internal class Puzzle : PuzzleBase
    {
        private readonly Dictionary<char, int> Energy = new Dictionary<char, int>
        {
            ['A'] = 1,
            ['B'] = 10,
            ['C'] = 100,
            ['D'] = 1000
        };

        private readonly Dictionary<char, int> RoomIndex = new Dictionary<char, int>
        {
            ['A'] = 2,
            ['B'] = 4,
            ['C'] = 6,
            ['D'] = 8
        };

        private record Amphipod(char Name, int X, int Y);

        private class Node
        {
            public bool Visited { get; set; }
            public int? MinCostsToStart { get; set; }
            public string NearestNodeToStart { get; set; }
        }

        protected override string SampleResult => "44169"; // "12521"; 

        protected override Task<string> GetSampleInputAsync()
        {
            return Task.FromResult(@"#############
#...........#
###B#C#B#D###
  #A#D#C#A#
  #########");
        }

        protected override string DoCalculation(string input)
        {
            var current = ParseInput(input);

            var roomSize = current.Max(a => a.Y);

            var nodes = new Dictionary<string, Node>();
            var key = GetKey(current);
            nodes.Add(key, new Node { Visited = false, MinCostsToStart = 0, NearestNodeToStart = key });

            var queue = new PriorityQueue<HashSet<Amphipod>, int>();
            var queuedKeys = new HashSet<string>();

            queue.Enqueue(current, 0);
            queuedKeys.Add(key);

            do
            {
                current = queue.Dequeue();
                var currentKey = GetKey(current);
                var currentNode = nodes[currentKey];
                queuedKeys.Remove(currentKey);

                foreach (var follower in GetFollowers(current).OrderBy(f => f.Energy))
                {
                    var followingKey = GetKey(follower.Following);
                    if (!nodes.ContainsKey(followingKey))
                    {
                        nodes.Add(followingKey, new Node());
                    }
                    var followingNode = nodes[followingKey];

                    if (!followingNode.Visited)
                    {
                        if (followingNode.MinCostsToStart == null || currentNode.MinCostsToStart + follower.Energy < followingNode.MinCostsToStart)
                        {
                            followingNode.MinCostsToStart = currentNode.MinCostsToStart + follower.Energy;
                            followingNode.NearestNodeToStart = currentKey;

                            if (!queuedKeys.Contains(followingKey))
                            {
                                queue.Enqueue(follower.Following, followingNode.MinCostsToStart.Value);
                                queuedKeys.Add(followingKey);
                            }
                        }
                    }
                }

                currentNode.Visited = true;

                if (CheckFinalSolution(current))
                {
                    break;
                }
            }
            while (queue.Count > 0);

            var result = nodes[GetKey(current)];
            return result.MinCostsToStart.ToString();

            IEnumerable<(HashSet<Amphipod> Following, int Energy)> GetFollowers(HashSet<Amphipod> amphipods)
            {                
                var result = new List<(HashSet<Amphipod> Following, int Energy)>();

                foreach (var amphipod in amphipods)
                {
                    if (amphipod.Y == 0)
                    {
                        var targetX = RoomIndex[amphipod.Name];

                        // Check whether an amphipod can enter the room.
                        if (!amphipods.Any(a => a.X == targetX && a.Y == 1) && !amphipods.Any(a => a.X == targetX && a.Y >= 2 && a.Name != amphipod.Name))
                        {
                            var x = amphipod.X;
                            var step = x > targetX ? -1 : 1;
                            var energy = 0;

                            // Check whether there is a clear path to the room.
                            while (x != targetX)
                            {
                                if (amphipods.Any(a => a.X == x + step && a.Y == 0))
                                {
                                    break;
                                }

                                x += step;
                                energy += Energy[amphipod.Name];
                            }

                            if (x == targetX)
                            {
                                // Go inside the room as far as possible.
                                var targetY = amphipods.Where(a => a.X == targetX).OrderBy(a => a.Y).FirstOrDefault()?.Y - 1 ?? roomSize;
                                energy += targetY * Energy[amphipod.Name];

                                result.Add((Clone(amphipods, amphipod, new Amphipod(amphipod.Name, targetX, targetY)), energy));
                            }
                        }
                    }
                    else 
                    {
                        // Check whether an amphipod has a clear way out of the room
                        if (!amphipods.Any(a => a.X == amphipod.X && a.Y > 0 && a.Y < amphipod.Y))
                        {
                            result.AddRange(CheckDirection(amphipod, -1));
                            result.AddRange(CheckDirection(amphipod, 1));
                        }

                        IEnumerable<(HashSet<Amphipod> Following, int Energy)> CheckDirection(Amphipod amphipod, int step)
                        {
                            var x = amphipod.X + step;
                            var energy = (amphipod.Y + 1) * Energy[amphipod.Name];

                            // Enumerate all free stops to the left or to the right.
                            while (x >= 0 && x <= 10)
                            {
                                if (amphipods.Any(a => a.X == x && a.Y == 0))
                                {
                                    break;
                                }

                                yield return (Clone(amphipods, amphipod, new Amphipod(amphipod.Name, x, 0)), energy);

                                if (x > 2 && x < 8)
                                {
                                    x += 2 * step;
                                    energy += 2 * Energy[amphipod.Name];
                                }
                                else
                                {
                                    x += step;
                                    energy += Energy[amphipod.Name];
                                }
                            }
                        }
                    }
                }

                return result;
            }

            bool CheckFinalSolution(HashSet<Amphipod> amphipods)
            {
                return amphipods.Count(a => a.X == 2 && a.Name == 'A') == roomSize
                    && amphipods.Count(a => a.X == 4 && a.Name == 'B') == roomSize
                    && amphipods.Count(a => a.X == 6 && a.Name == 'C') == roomSize
                    && amphipods.Count(a => a.X == 8 && a.Name == 'D') == roomSize;
            }

            HashSet<Amphipod> Clone(HashSet<Amphipod> amphipods, Amphipod remove, Amphipod replaceWith)
            {
                var copy = new HashSet<Amphipod>(amphipods);

                copy.Remove(remove);
                copy.Add(replaceWith);

                return copy;
            }

            string GetKey(IEnumerable<Amphipod> amphipods)
            {
                var buffer = Enumerable.Repeat('.', 11 + 4 * roomSize).ToArray();
                
                amphipods.Where(a => a.Y == 0).ToList().ForEach(a =>
                {
                    buffer[a.X] = a.Name;
                });

                amphipods.Where(a => a.Y > 0).ToList().ForEach(a =>
                {
                    buffer[10 + roomSize * (a.X/2 - 1) + a.Y] = a.Name;
                });

                return new string(buffer);
            }
        }

        private HashSet<Amphipod> ParseInput(string input)
        {
            var data = input
                .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
                .ToList();

            // Comment out these 2 lines for the part 1.
            data.Insert(3, "  #D#C#B#A#");
            data.Insert(4, "  #D#B#A#C#");

            var amphipods = new List<Amphipod>();

            amphipods.AddRange(ParseColumn(3));
            amphipods.AddRange(ParseColumn(5));
            amphipods.AddRange(ParseColumn(7));
            amphipods.AddRange(ParseColumn(9));

            return amphipods.ToHashSet();

            IEnumerable<Amphipod> ParseColumn(int column)
            {
                for (int i = 2; i < data.Count - 1; i++)
                {
                    yield return new Amphipod(data[i][column], column - 1, i - 1);
                }
            }
        }
    }
}
