namespace AdventOfCode.Day12
{
    internal class Puzzle : PuzzleBase
    {
        public Puzzle()
        {
            Debug.Assert(DoCalculation(@"start-A
start-b
A-c
A-b
b-d
A-end
b-end") == "36" /*10*/);

            Debug.Assert(DoCalculation(@"dc-end
HN-start
start-kj
dc-start
dc-HN
LN-dc
HN-end
kj-sa
kj-HN
kj-dc") == "103" /*19*/);
        }

        protected override string SampleResult => "3509"; // "226";

        protected override Task<string> GetSampleInputAsync()
        {
            return Task.FromResult(@"fs-end
he-DX
fs-he
start-DX
pj-DX
end-zg
zg-sl
zg-pj
pj-he
RW-he
fs-DX
pj-RW
zg-RW
start-pj
he-WI
zg-he
pj-fs
start-RW");
        }

        protected override string DoCalculation(string input)
        {
            var edges = input
                .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Split('-'))
                .Select(s => (V1: s[0], V2: s[1]))
                .ToArray();

            var paths = new HashSet<string>();
            
            var stack = new Stack<string>();
            stack.Push("start");

            while (stack.Any())
            {
                var item = stack.Pop();
                var current = !item.Contains(',') ? item : item.Substring(item.LastIndexOf(',') + 1);

                var next = edges
                    .Where(e => e.V1 == current || e.V2 == current)
                    .Select(e => e.V1 == current ? e.V2 : e.V1)
                    .Select(e => $"{item},{e}")
                    .ToArray();

                next.Where(n => CheckPath(n)).ForEach(n =>
                {
                    if (n.EndsWith("end"))
                    {
                        if (!paths.Contains(n))
                        {
                            paths.Add(n);
                        }
                    } 
                    else
                    {
                        stack.Push(n);
                    }
                });
            }      

            return paths.Count().ToString();

            bool CheckPath(string path)
            {
                var caves = path.Split(',').ToArray();

                var counts = caves.GroupBy(c => c).Select(c => new { c.Key, Amount = c.ToArray().Count() });

                var small = counts.Where(s => s.Key.ToUpper() != s.Key).ToArray();

                //return small.All(s => s.Amount <= 1);
                return 
                    small.All(s => s.Amount <= 2) 
                    && small.Where(s => s.Amount == 2).Count() <= 1 
                    && small.Where(s => s.Key == "start" || s.Key == "end").All(s => s.Amount <= 1); 
            }
        }
    }
}
