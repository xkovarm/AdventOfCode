namespace AdventOfCode.Day13
{
    internal class Puzzle : PuzzleBase
    {
        // -- CALCULATION PARAMETERS AND RESULT ------------------------------------

        protected override string SampleResult => "140";    // PART 1: "13"

        // -------------------------------------------------------------------------

        protected override string DoCalculation(string input)
        {
            var pairs = input
                .Split($"{Environment.NewLine}{Environment.NewLine}", StringSplitOptions.RemoveEmptyEntries)
                .Select(i => i.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).ToArray())
                .ToList();

            // PART 1: var result = Check(pairs);
            var result = Calculate(pairs);

            return result.ToString();
        }
 
        private int Calculate(List<string[]> pairs)
        {
            pairs.Add(new[] { "[[2]]", "[[6]]" });
            
            var packets = pairs
                .SelectMany(p => p)
                .Select(p => new { Packet = p, Parsed = Parse(new Queue<char>(p)) })
                .ToList();

            var ordered = packets.OrderBy(p => p.Parsed, new Comparer()).ToList();
            
            var packet2 = ordered.Single(p => p.Packet == "[[2]]");
            var packet6 = ordered.Single(p => p.Packet == "[[6]]");

            return (ordered.IndexOf(packet2) + 1) * (ordered.IndexOf(packet6) + 1);

        }

        private int Check(List<string[]> pairs)
        {
            return Enumerable.Range(0, pairs.Count)
                .Select(r => CompareNodes(
                    Parse(new Queue<char>(pairs[r][0])), 
                    Parse(new Queue<char>(pairs[r][1]))) ?? true ? r + 1 : 0)
                .Sum();
        }

        private static bool? CompareNodes(Node left, Node right)
        {
            for (int i = 0; i < left.Children.Count; i++)
            {                
                if (i >= right.Children.Count)
                {
                    return false;
                }

                var leftNode = left.Children[i];
                var rightNode = right.Children[i];

                if (leftNode.Value.HasValue && rightNode.Value.HasValue)
                {
                    if (leftNode.Value < rightNode.Value)
                    {
                        return true;
                    }
                    else if (leftNode.Value > rightNode.Value)
                    {
                        return false;
                    }
                }
                else
                {
                    var result = CompareNodes(
                        leftNode.Value.HasValue ? new Node(leftNode) : leftNode, 
                        rightNode.Value.HasValue ? new Node(rightNode) : rightNode
                        );
               
                    if (result.HasValue)
                    {
                        return result;
                    }
                }
            }
            
            if (left.Children.Count < right.Children.Count)
            {                
                return true;
            }

            return null;
        }

        private Node Parse(Queue<char> input)
        {
            if (input.Dequeue() != '[')
            {
                throw new InvalidOperationException("Syntax error");
            }

            var node = new Node();

            while (input.Any())
            {
                switch (input.Peek())
                {
                    case '[':
                        node.Children.Add(Parse(input));
                        break;
                    case ']':
                        input.Dequeue();
                        return node;
                    case ',':
                        input.Dequeue();
                        break;
                    case char ch when IsNumber(ch):
                        node.Children.Add(new Node { Value = GetInt(input) });
                        break;
                    default:
                        throw new InvalidOperationException("Syntax error");
                }
            }

            return node;
        }

        private bool IsNumber(char c) => c >= '0' && c <= '9';

        // simplified to 2 digits max.
        private int GetInt(Queue<char> input) => int.Parse($"{input.Dequeue()}{(IsNumber(input.Peek()) ? input.Dequeue() : string.Empty)}");

        private class Node
        {
            public int? Value { get; set; }

            public List<Node> Children { get; } = new();

            public Node()
            {
            }

            public Node(Node child)
            {
                Children.Add(child);
            }
        }

        private class Comparer : IComparer<Node>
        {
            public int Compare(Node? x, Node? y)
            {
                if (x is null || y is null)
                {
                    throw new InvalidOperationException();  // not handled
                }

                if (x == y)
                {
                    return 0;
                }

                return CompareNodes(x, y).GetValueOrDefault(true) ? -1 : 1;
            }
        }
    }
}
