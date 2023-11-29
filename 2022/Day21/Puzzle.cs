using System.Numerics;
using MoreLinq;

namespace AdventOfCode.Day21
{
    internal class PuzzleC : PuzzleBase
    {
        // -- CALCULATION PARAMETERS AND RESULT ------------------------------------

        protected override string SampleResult => "301"; // PART 1: "152";

        // -------------------------------------------------------------------------

        protected override string DoCalculation(string input)
        {
            var monkeys = input
                .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
                .Select(i => Monkey.Parse(i))
                .ToDictionary(i => i.Name, i => i);

          //  var result = monkeys["root"].Evaluate(monkeys);

            var transformed = CreateNewRoot("root", "humn", monkeys);
            var result = transformed["humn"].Evaluate(transformed);

            return result.ToString();
        }

        private Dictionary<string, Monkey> CreateNewRoot(string oldRoot, string newRoot, Dictionary<string, Monkey> monkeys)
        {
            var parents = monkeys
                .Values.Where(v => v.Value is null)
                .Select(m => new[] { new { Name = m.Monkey1, Monkey = m }, new { Name = m.Monkey2, Monkey = m } })
                .SelectMany(m => m)
                .ToDictionary(m => m.Name, m => m);

            var newMonkeys = new Dictionary<string, Monkey>();

            var current = newRoot;
            var last = string.Empty;

            while (true)
            {
                var parent = parents[current].Monkey;

                if (parent.Name == oldRoot)
                {
                    break;
                }

                var monkey = new Monkey { 
                    Name = current,
                    Monkey1 = parent.Name,
                    Monkey2 = parent.Monkey1 == current ? parent.Monkey2 : parent.Monkey1,
                    Operator = parent.Operator switch
                    {
                        '+' => '-',
                        '-' => '+',
                        '*' => '/',
                        '/' => '*'
                    }                    
                };

                newMonkeys.Add(monkey.Name, monkey);
                parents.Remove(current);

                last = current;
                current = parent.Name;
            }

            var root = monkeys[oldRoot];
            newMonkeys[last].Monkey1 = root.Monkey1 != current ? root.Monkey1 : root.Monkey2;

            var (a, b) = (newMonkeys[last].Monkey1, newMonkeys[last].Monkey2);
            (newMonkeys[last].Monkey2, newMonkeys[last].Monkey1) = (a, b);

            monkeys.Values.Where(v => v.Name != oldRoot && !newMonkeys.ContainsKey(v.Name)).ForEach(v => newMonkeys.Add(v.Name, v));

            return newMonkeys;
        }

        private class Monkey
        {
            public string Name { get; set; }

            public BigInteger? Value { get; set; }

            public char Operator { get; set; }

            public string Monkey1 { get; set; }

            public string Monkey2 { get; set; }

            public override string ToString()
            {
                return $"{Name}: {(Value is null ? $"{Monkey1}{Operator}{Monkey2}" : $"{Value}")}";
            }

            public static Monkey Parse(string input)
            {
                var monkey = new Monkey();

                var split = input.Split(new[] { ':', ' ' }, StringSplitOptions.RemoveEmptyEntries);

                monkey.Name = split[0];

                if (split.Length == 2)
                {
                    monkey.Value = BigInteger.Parse(split[1]);
                }
                else
                {
                    monkey.Monkey1 = split[1];
                    monkey.Operator = split[2][0];
                    monkey.Monkey2 = split[3];
                }

                return monkey;
            }

            public BigInteger Evaluate(Dictionary<string, Monkey> monkeys)
            {
                if (Value.HasValue)
                {
                    return Value.Value;
                }
                else
                {
                    var m1 = monkeys[Monkey1].Evaluate(monkeys);
                    var m2 = monkeys[Monkey2].Evaluate(monkeys);

                    return Operator switch
                    {
                        '+' => m1 + m2,
                        '-' => m1 - m2,
                        '*' => m1 * m2,
                        '/' => m1 / m2
                    };
                }
            }
        }
    }
}
