namespace AdventOfCode.Day11
{
    internal class Puzzle : PuzzleBase
    {
        // -- CALCULATION PARAMETERS AND RESULT ------------------------------------

        protected override string SampleResult => "2713310158"; //"10605";

        // -------------------------------------------------------------------------

        protected override string DoCalculation(string input)
        {
            var monkeys = ParseInput(input).ToList();

            var result = Run(monkeys);

            return result.ToString();
        }

        private long Run(List<Monkey> monkeys)
        {
            var div = monkeys.Select(s => s.Test).Aggregate((a,b) => a * b);

            // PART 1: var count = 20;
            var count = 10000;

            for (int i = 0; i < count; i++)
            {
                foreach (var monkey in monkeys)
                {
                    while (monkey.Items.Any())
                    {
                        var item = monkey.Items.Dequeue();

                        var worry = monkey.Evaluate(item);
                        //PART 1: worry /= 3;
                        worry %= div;

                        var next = worry % monkey.Test == 0 ? monkey.TrueId : monkey.FalseId;

                        monkeys[next].Items.Enqueue(worry);

                        monkey.Inspected++;
                    }
                }
            }

            var ordered = monkeys.OrderByDescending(m => m.Inspected).ToArray();

            return (long)ordered[0].Inspected * (long)ordered[1].Inspected;
        }

        private IEnumerable<Monkey> ParseInput(string input)
        {
            var blocks = input
                .Split($"{Environment.NewLine}{Environment.NewLine}", StringSplitOptions.RemoveEmptyEntries)
                .Select(i => i.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries))
                .Select(i => i.Select(a => a).ToList())
                .ToList();

            // (╯°□°）╯︵ ┻━┻

            foreach (var block in blocks)
            {
                var id = int.Parse(block[0].Split(new [] { "Monkey", ":" }, StringSplitOptions.RemoveEmptyEntries)[^1]);
                var things = new Queue<long>(block[1].Split(':')[^1].Split(',', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToList());
                var op = block[2].Split(':')[^1].Split(' ', StringSplitOptions.RemoveEmptyEntries);
                var test = int.Parse(block[3].Split(' ', StringSplitOptions.RemoveEmptyEntries)[^1]);
                var trueId = int.Parse(block[4].Split(' ', StringSplitOptions.RemoveEmptyEntries)[^1]);
                var falseId = int.Parse(block[5].Split(' ', StringSplitOptions.RemoveEmptyEntries)[^1]);

                if (op is [_, _, _, "*", "old"])
                {
                    yield return new Monkey(id, things, "^", null, test, trueId, falseId);
                } 
                else if (op is [_, _, _, "*", var a])
                {
                    yield return new Monkey(id, things, "*", int.Parse(a), test, trueId, falseId);
                } 
                else if (op is [_, _, _, "+", var b])
                {
                    yield return new Monkey(id, things, "+", int.Parse(b), test, trueId, falseId);
                } 
                else
                {
                    throw new InvalidOperationException();
                }
            }
        }
    }

    record Monkey(int Id, Queue<long> Items, string Op, int? Num, int Test, int TrueId, int FalseId)
    {
        public int Inspected { get; set; } = 0;

        public long Evaluate(long item) => Op switch
        {
            "+" => item + Num!.Value,
            "*" => item * Num!.Value,
            "^" => item * item,
            _ => throw new InvalidOperationException()
        };      
    }
}
