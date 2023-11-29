using MoreLinq;
using MoreLinq.Extensions;

namespace AdventOfCode.Day16
{
    internal class Puzzle : PuzzleBase
    {
        // -- CALCULATION PARAMETERS AND RESULT ------------------------------------

        protected override string SampleResult => "1707"; //"1651";

        // -------------------------------------------------------------------------

        protected override string DoCalculation(string input)
        {
            var valves = input
                .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Split(new[] { "Valve", "has flow rate=", "; tunnels lead to valves", "; tunnel leads to valve" }, StringSplitOptions.RemoveEmptyEntries).ToList())
                .Select(x => new Valve(x[0].Trim(), int.Parse(x[1]), x[2].Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries).ToList()))
                .ToDictionary(x => x.Name, x => x);

            var result = Calculate(valves);

            return result.ToString();
        }

        private int Calculate(IDictionary<string, Valve> allValves)
        {
            // reduce the nodes
            var valves = allValves
                .Values
                .Where(v => v.Rate > 0 || v.Name == "AA")
                .ToDictionary(v => v.Name, v => v);


            // calculate distances between reduced nodes
            var distances = new List<Distance>();
            var valveNames = valves.Keys.ToList();

            for (int i = 0; i < valveNames.Count - 1; i++)
            {
                for (int j = i + 1; j < valveNames.Count; j++)
                {
                    var distance = GetDistance(allValves, valveNames[i], valveNames[j]);
                    distances.Add(new Distance(valveNames[i], valveNames[j], distance));
                }
            }

            var queue = new Queue<State>();
            var valvesCount = valves.Keys.Count();

            var start = new State("AA", "AA", valvesCount, 26, 26);
            var index = valveNames.IndexOf("AA");
            start.Open[index] = true;
            
            queue.Enqueue(start);

            int max = 0;

            while (queue.TryDequeue(out var current))
            {
                var followers1 = GetFollowers(current.Valve1);
                var followers2 = GetFollowers(current.Valve2);

                foreach (var follower1 in followers1)
                {
                    foreach (var follower2 in followers2)
                    {
                        if (follower1.Node == follower2.Node)
                        {
                            continue;
                        }

                        var index1 = valveNames.IndexOf(follower1.Node);
                        var index2 = valveNames.IndexOf(follower2.Node);

                        if (!current.Open[index1] && !current.Open[index2])
                        {
                            var next = current.Clone();
                            next.Valve1 = follower1.Node;
                            next.Valve2 = follower2.Node;
                            next.Open[index1] = true;
                            next.Open[index2] = true;

                            var remainingTime1 = current.Time1 - follower1.Distance - 1;

                            if (remainingTime1 >= 0)
                            {
                                next.Time1 = remainingTime1;
                                next.Rate += valves[next.Valve1].Rate * remainingTime1;
                            }

                            var remainingTime2 = current.Time2 - follower2.Distance - 1;

                            if (remainingTime2 >= 0)
                            {
                                next.Time2 = remainingTime2;
                                next.Rate += valves[next.Valve2].Rate * remainingTime2;
                            }

                            if (remainingTime1 <= 0 || remainingTime2 <= 0 || next.Open.All(o => o))
                            {
                                // out of time -> return the added rate
                                max = int.Max(max, next.Rate);
                                continue;
                            }

                            queue.Enqueue(next);
                        }
                    }
                }
            }


            //while (queue.TryDequeue(out var current))
            //{
            //    if (current.Open.All(o => o))
            //    {
            //        max = int.Max(max, current.Rate);
            //    }

            //    var followers = GetFollowers(current.Valve1);
            //    //var followers2 = GetFollowers(current.Valve2);

            //    foreach (var follower in followers)
            //    {
            //        index = valveNames.IndexOf(follower.Node);

            //        if (!current.Open[index])
            //        {
            //            var next = current.Clone();
            //            next.Valve1 = follower.Node;
            //            next.Open[index] = true;

            //            var remainingTime = current.Time - follower.Distance - 1;

            //            if (remainingTime < 0)
            //            {
            //                max = int.Max(max, current.Rate);
            //                continue;
            //            }

            //            next.Time = remainingTime;                        
            //            next.Rate += valves[next.Valve1].Rate * remainingTime;

            //            queue.Enqueue(next);
            //        }
            //    }
            //}

            return max;

            List<Follower> GetFollowers(string current)
            {
                return distances!
                    .Where(e => e.Node1 == current || e.Node2 == current)
                    .Select(e => new Follower(current != e.Node2 ? e.Node2 : e.Node1, e.Value))
                    .ToList();
            }
        }

        private int GetDistance(IDictionary<string, Valve> valves, string start, string end)
        {
            var queue = new Queue<string>();
            var dist = new Dictionary<string, int>();

            queue.Enqueue(start);
            dist.Add(start, 0);

            while (queue.TryDequeue(out var node))
            {
                foreach (var next in valves[node].ConnectedTo)
                {
                    if (!dist.ContainsKey(next))
                    {
                        dist.Add(next, dist[node] + 1);
                        queue.Enqueue(next);

                        if (next == end)
                        {
                            return dist[end];
                        }
                    }
                }
            }

            throw new InvalidOperationException("The nodes are not connected");
        }

        private record Valve(string Name, int Rate, List<string> ConnectedTo);

        private record Distance(string Node1, string Node2, int Value);

        private class State
        {
            public string Valve1 { get; set; }

            public string Valve2 { get; set; }

            public bool[] Open { get; set; }

            public int Rate {get; set;}

            public int Time1 { get; set; }

            public int Time2 { get; set; }

            public State(string valve1, string valve2, int valvesCount, int time1, int time2)
            {
                Valve1 = valve1;
                Valve2 = valve2;
                Open = new bool[valvesCount];
                Time1 = time1;
                Time2 = time2;
            }

            public State Clone()
            {
                var state = new State(Valve1, Valve2, Open.Length, Time1, Time2);

                state.Rate = Rate;

                for (int i = 0; i< Open.Length; i++)
                {
                    state.Open[i] = Open[i];
                }

                return state;
            }

            public override string ToString()
            {
                return $"valve1={Valve1}, valve2={Valve2}, rate={Rate}, time1={Time1}, time={Time2}, open={string.Join("", Open.Select(o => o ? 'O' : '-').ToArray())}";
            }
        }

        private record Follower(string Node, int Distance);
    }
}
