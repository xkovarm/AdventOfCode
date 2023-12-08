namespace AdventOfCode.Day08;

internal class Puzzle : PuzzleBase
{
    // -- CALCULATION PARAMETERS AND RESULT ------------------------------------

    protected override string SampleResult => "2";

    // -------------------------------------------------------------------------

    protected override string DoCalculation(string input)
    {
        var (path, map) = Parse(input);

        // PART 1
        //var startNode = "AAA";
        //var result = CalculateSteps(startNode, n => n == "ZZZ");

        // PART 2
        var startNodes = map.Keys.Where(m => m.EndsWith('A')).ToList();
        var result = startNodes.Select(n => CalculateSteps(n, n => n.EndsWith('Z'))).LeastCommonMultiple();

        return result.ToString();

        long CalculateSteps(string node, Func<string, bool> finalNode) 
        {
            var i = 0;
            var steps = 0;

            while (!finalNode(node))
            {
                node  = path[i] == 'L' ? map[node].Left : map[node].Right;

                i = i < path.Length - 1 ? i + 1 : 0;
                steps++;
            }

            return steps;
        }
    }

    private (string Path, Dictionary<string, Node> Map) Parse(string input)
    {
        var lines = input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

        var map = lines[1..]
            .Select(l => l.Split(new[] { ' ', '=', '(', ',', ')' }, StringSplitOptions.RemoveEmptyEntries))
            .Select(l => new Node(l[0], l[1], l[2]))
            .ToDictionary(n => n.Name, n => n);

        return (lines[0], map);
    }

    private record Node(string Name, string Left, string Right);
}
