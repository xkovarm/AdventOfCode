using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Day18
{
    internal class Puzzle : PuzzleBase
    {
        protected override string SampleResult => "3993";

        private class Node
        {
            public Node Left { get; set; }

            public Node Right { get; set; }

            public int? Number { get; set; }

            public Node Parent { get; set; }
        }

        public Puzzle()
        {
            Debug.Assert(Render(Add(
                Parse("[[[[4,3],4],4],[7,[[8,4],9]]]"),
                Parse("[1,1]")
                )) == "[[[[0,7],4],[[7,8],[6,0]]],[8,1]]");

            Debug.Assert(Render(Add(
                Parse("[[[0,[4,5]],[0,0]],[[[4,5],[2,6]],[9,5]]]"),
                Parse("[7,[[[3,7],[4,3]],[[6,3],[8,8]]]]")
                )) == "[[[[4,0],[5,4]],[[7,7],[6,0]]],[[8,[7,7]],[[7,9],[5,0]]]]");

            Debug.Assert(DoCalculation1(@"[[[0,[4,5]],[0,0]],[[[4,5],[2,6]],[9,5]]]
[7,[[[3,7],[4,3]],[[6,3],[8,8]]]]
[[2,[[0,8],[3,4]]],[[[6,7],1],[7,[1,6]]]]
[[[[2,4],7],[6,[0,5]]],[[[6,8],[2,8]],[[2,1],[4,5]]]]
[7,[5,[[3,8],[1,4]]]]
[[2,[2,2]],[8,[8,1]]]
[2,9]
[1,[[[9,3],9],[[9,0],[0,7]]]]
[[[5,[7,4]],7],1]
[[[[4,2],2],6],[8,7]]") == "3488");
        }

        protected override Task<string> GetSampleInputAsync()
        {
            return Task.FromResult(@"[[[0,[5,8]],[[1,7],[9,6]]],[[4,[1,2]],[[1,4],2]]]
[[[5,[2,8]],4],[5,[[9,9],0]]]
[6,[[[6,2],[5,6]],[[7,6],[4,7]]]]
[[[6,[0,7]],[0,9]],[4,[9,[9,0]]]]
[[[7,[6,4]],[3,[1,3]]],[[[5,5],1],9]]
[[6,[[7,3],[3,2]]],[[[3,8],[5,7]],4]]
[[[[5,4],[7,7]],8],[[8,3],8]]
[[9,3],[[9,9],[6,[4,9]]]]
[[2,[[7,7],7]],[[5,8],[[9,3],[0,2]]]]
[[[[5,2],5],[8,[3,7]]],[[5,[7,5]],[4,4]]]");
        }

        protected override string DoCalculation(string input)
        {
            var values = input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).ToArray();
          
            return CalculateMagnitudes().Max().ToString();

            IEnumerable<int> CalculateMagnitudes()
            {
                for (int i = 0; i < values.Length; i++)
                {
                    for (int j = 0; j < values.Length; j++)
                    {
                        if (i != j)
                        {
                            yield return Magnitude(Add(Parse(values[i]), Parse(values[j])));
                        }
                    }
                }
            }
        }

        protected string DoCalculation1(string input)
        {
            var values = input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

            var result = Parse(values[0]);
            values[1..].ToList().ForEach(value =>
            {
                var next = Parse(value);
                result = Add(result, next);
            });

            var magnitude = Magnitude(result);

            return magnitude.ToString();
        }

        private int Magnitude(Node node)
        {
            if (node.Number.HasValue)
            {
                return node.Number.Value;
            } 
            else 
            {
                return 3 * Magnitude(node.Left) + 2 * Magnitude(node.Right);
            }
        }

        private bool ScanExplosions(Node expression)
        {
            var list = new List<(Node Node, int Depth)>();

            Node toExplode = null;

            Node lastLeftNumber = null;
            Node firstRightNumber = null;

            list.Clear();
            list.Insert(0, (expression, 0));

            while (list.Any())
            {
                var item = list[0];
                list.RemoveAt(0);

                if (item.Node.Number.HasValue)
                {
                    if (toExplode == null)
                    {
                        lastLeftNumber = item.Node;
                    }
                    else
                    {
                        firstRightNumber = item.Node;
                        break;
                    }
                }
                else
                {
                    if (item.Depth == 4 && toExplode == null)
                    {
                        toExplode = item.Node;
                    }
                    else
                    {
                        list.Insert(0, (item.Node.Right, item.Depth + 1));
                        list.Insert(0, (item.Node.Left, item.Depth + 1));
                    }
                }
            }

            if (toExplode != null)
            {
                if (lastLeftNumber != null)
                {
                    lastLeftNumber.Number += toExplode.Left.Number.Value;
                }

                if (firstRightNumber != null)
                {
                    firstRightNumber.Number += toExplode.Right.Number.Value;
                }

                if (toExplode.Parent.Left == toExplode)
                {
                    toExplode.Parent.Left = new Node { Parent = toExplode.Parent, Number = 0 };
                }
                else
                {
                    toExplode.Parent.Right = new Node { Parent = toExplode.Parent, Number = 0 };
                }
            }

            return toExplode != null;
        }

        private bool ScanSplits(Node expression)
        {
            var list = new List<Node>();

            Node toSplit = null;

            list.Clear();
            list.Insert(0, expression);

            while (list.Any())
            {
                var item = list[0];
                list.RemoveAt(0);

                if (item.Number.HasValue)
                {
                    if (item.Number.Value >= 10)
                    {
                        toSplit = item;
                        break;
                    }
                } 
                else
                {
                   list.Insert(0, item.Right);
                   list.Insert(0, item.Left);
                }
            }

            if (toSplit != null)
            {
                var node = new Node { Parent = toSplit.Parent };
                var left = new Node { Number = toSplit.Number.Value / 2, Parent = node };
                var right = new Node { Number = toSplit.Number.Value - (toSplit.Number.Value / 2), Parent = node };
                node.Left = left;
                node.Right = right;

                if (toSplit.Parent.Left == toSplit)
                {
                    toSplit.Parent.Left = node;
                }
                else
                {
                    toSplit.Parent.Right = node;
                }
            }

            return toSplit != null;
        }

        private Node Add(Node n1, Node n2)
        {
            var pair = new Node { Left = n1, Right = n2 };

            n1.Parent = pair;
            n2.Parent = pair;

            Reduce(pair);

            return pair;
        }

        private void Reduce(Node expression)
        {
            while (true)
            {
                var explosions = ScanExplosions(expression);

                if (!explosions)
                {
                    var splits = ScanSplits(expression);
                    
                    if (!explosions && !splits)
                    {
                        break;
                    }
                }
            }
        }

        private Node Parse(string input)
        {
            return ParseStep(new Queue<char>(input));

            Node ParseStep(Queue<char> queue)
            {
                if (queue.Peek() == '[')
                {
                    queue.Dequeue();    // '['

                    var first = ParseStep(queue);

                    queue.Dequeue();    // ','                

                    var second = ParseStep(queue);

                    queue.Dequeue();   // ']'

                    var pair = new Node { Left = first, Right = second };

                    first.Parent = pair;
                    second.Parent = pair;

                    return pair;
                }
                else if (queue.Peek() >= '0' && queue.Peek() <= '9')
                {
                    return new Node { Number = queue.Dequeue() - '0' };
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
        }


        private string Render(Node node)
        {
            var builder = new StringBuilder();

            RenderStep(node);

            return builder.ToString();

            void RenderStep(Node node)
            {
                if (node.Number.HasValue)
                {
                    builder.Append($"{node.Number.Value}");
                }
                else
                {
                    builder.Append("[");
                    RenderStep(node.Left);
                    builder.Append(",");
                    RenderStep(node.Right);
                    builder.Append("]");
                }
            }
        }
    }
}
