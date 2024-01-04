namespace AdventOfCode.Day19;

internal class Puzzle : PuzzleBase
{
    // -- CALCULATION PARAMETERS AND RESULT ------------------------------------

    protected override string SampleResult => "167409079868000"; //"19114";

    // -------------------------------------------------------------------------

    protected override string DoCalculation(string input)
    {
        var (workflows, parts) = Parse(input);

        var working = new Stack<Result>();
        working.Push(new Result { Conditions = new List<Condition>(), Next = "in" });

        var accepted = new List<Result>();

        while (working.TryPop(out var record))
        {
            var rules = workflows[record.Next].Rules;

            var negations = new List<Condition>();

            foreach (var rule in rules)
            {
                var cloned = record.CloneAndExtend(rule.Result, (rule.Condition == null ? negations : negations.Concat([rule.Condition])).ToArray());

                if (cloned.Next == "A")
                {
                    accepted.Add(cloned);
                }
                else if (cloned.Next != "R")
                {
                    working.Push(cloned);
                }

                if (rule.Condition != null)
                {
                    negations.Add(rule.Condition.Negate());
                }
            }
        }

        var result = accepted.Select(a => a.Evaluate()).Sum();

        return result.ToString();
    }

    //protected string DoCalculation_Part1(string input)
    //{
    //    var (workflows, parts) = Parse(input);

    //    var result = 0;

    //    foreach (var part in parts)
    //    {
    //        var wf = "in";

    //        while (true)
    //        {
    //            var workflow = workflows[wf];

    //            wf = EvaluateRules(workflow.Rules, part);

    //            if (wf == "A")
    //            {
    //                result += part.Values.Sum();
    //                break;
    //            }
    //            else if (wf == "R")
    //            {
    //                break;
    //            }
    //        }
    //    }

    //    string EvaluateRules(List<Rule> rules, Part part)
    //    {
    //        foreach (var rule in rules)
    //        {
    //            if (rule.Condition is null)
    //            {
    //                return rule.Result;
    //            }
    //            else
    //            {
    //                if ((rule.Condition.Op == '>' && part[rule.Condition.Key] > rule.Condition.Value)
    //                    || (rule.Condition.Op == '<' && part[rule.Condition.Key] < rule.Condition.Value))
    //                {
    //                    return rule.Result;
    //                }
    //            }
    //        }

    //        throw new InvalidOperationException();
    //    }

    //    return result.ToString();
    //}

    // Parse: it could definitely be done better...
    (Dictionary<string, Workflow> Workflow, List<Part> Parts) Parse(string input)
    {
        var split = input.Split($"{Environment.NewLine}{Environment.NewLine}");

        var workflows = split[0]
            .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
            .Select(s => s
                .Split(new[] { '{', ',', '}' }, StringSplitOptions.RemoveEmptyEntries)
            )
            .Select(ss => new Workflow(ss[0], ParseRules(ss[1..]).ToList()))
            .ToDictionary(s => s.Name, s => s);

        var parts = split[1]
            .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
            .Select(s => s
                .Split(new[] { '{', ',', '}' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(ss => ss.Split('='))
                .ToArray()
                )
            .Select(s => new Part { [s[0][0]] = int.Parse(s[0][1]), [s[1][0]] = int.Parse(s[1][1]), [s[2][0]] = int.Parse(s[2][1]), [s[3][0]] = int.Parse(s[3][1]) })
            .ToList();

        return (workflows, parts);

        IEnumerable<Rule> ParseRules(IEnumerable<string> ss)
        {
            foreach (var s in ss)
            {
                if (s.Contains(':'))
                {
                    var prts = s.Split([':', '<', '>']);
                    yield return new Rule(s.Contains('>') ? new Gt(prts[0], int.Parse(prts[1])) : new Lt(prts[0], int.Parse(prts[1])), prts[2]);
                }
                else
                {
                    yield return new Rule(null, s);
                }
            }
        }
    }

    private record Workflow(string Name, List<Rule> Rules);

    private record Rule(Condition? Condition, string Result);

    private abstract class Condition
    {
        public string Key { get; private set;}

        public int Value { get; private set;}

        public Condition(string key, int value)
        {
            Key = key;
            Value = value;
        }

        public abstract bool Check(int value);

        public abstract Condition Negate();
    }

    private class Lt : Condition
    {
        public Lt(string key, int value) : base(key, value) {}

        public override bool Check(int value) => value < Value;

        public override Condition Negate() => new Gt(Key, Value - 1);
    }

    private class Gt : Condition
    {
        public Gt(string key, int value) : base(key, value) {}

        public override bool Check(int value) => value > Value;

        public override Condition Negate() => new Lt(Key, Value + 1);
    }

    private class Result
    {
        public List<Condition> Conditions { get; set; }

        public string Next { get; set; }

        public Result CloneAndExtend(string next, params Condition[] conditions)
        {
            var c = Conditions.ToList();
            c.AddRange(conditions);

            return new Result { Conditions = c, Next = next };
        }

        // Lazy version, brute force, (╯°□°）╯︵ ┻━┻
        public long Evaluate()
        {
            // Start with full lists...
            var x = Enumerable.Range(1, 4000).ToList();
            var m = Enumerable.Range(1, 4000).ToList();
            var a = Enumerable.Range(1, 4000).ToList();
            var s = Enumerable.Range(1, 4000).ToList();

            // ...and apply filtering by conditions
            Filter(Conditions.Where(c => c.Key == "x"), x);
            Filter(Conditions.Where(c => c.Key == "m"), m);
            Filter(Conditions.Where(c => c.Key == "a"), a);
            Filter(Conditions.Where(c => c.Key == "s"), s);

            var result = (long)x.Count * m.Count * a.Count * s.Count;

            return result;

            void Filter(IEnumerable<Condition> conditions, List<int> values)
            {
                foreach (var condition in conditions)
                {
                    values.RemoveAll(v => !condition.Check(v));
                }
            }
        }
    }

    private class Part : Dictionary<string, int> { }
} 