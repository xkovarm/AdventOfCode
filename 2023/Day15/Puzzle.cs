namespace AdventOfCode.Day15;

internal class Puzzle : PuzzleBase
{
    // -- CALCULATION PARAMETERS AND RESULT ------------------------------------

    protected override string SampleResult => "145"; //"1320";

    // -------------------------------------------------------------------------

    public Puzzle()
    {
        Debug.Assert(CalculateHash("HASH") == 52);
    }

    protected override string DoCalculation(string input)
    {
        //var result = CalculatePart1(input);
        var result = CalculatePart2(input);

        return result.ToString();
    }

    int CalculatePart2(string input)
    {
        var steps = input.Trim().Split(',').ToList();

        var boxes = new List<Lens>[256];
        for (int i = 0; i < boxes.Length; i++)
        {
            boxes[i] = new();
        }

        foreach (var step in steps)
        {
            var parts = step.Split(['=', '-']);
            var label = parts[0];

            var hash = CalculateHash(label);
            var box = boxes[hash];

            if (step.EndsWith('-'))
            {
                RemoveIfExists(box, label);
            } 
            else
            {
                AddOrReplace(box, new Lens(label, int.Parse(parts[1])));
            }
        }

        var result = 0;

        for (int b = 0; b < boxes.Length; b++)
        {
            for (int l = 0; l < boxes[b].Count; l++)
            {
                result += (b + 1) * (l + 1) * boxes[b][l].Length;
            }
        }

        return result;

        void AddOrReplace(List<Lens> box, Lens lens)
        {
            var existingLens = box.FirstOrDefault(b => b.Label == lens.Label);

            if (existingLens != default)
            {
                box[box.IndexOf(existingLens)] = lens;
            }
            else
            {
                box.Add(lens);
            }

        }

        void RemoveIfExists(List<Lens> box, string label)
        {
            var lens = box.FirstOrDefault(b => b.Label == label);

            if (lens != null)
            {
                box.Remove(lens);
            }
        }
    }

    int CalculatePart1(string input)
    {
        return input
            .Trim()
            .Split(',')
            .Select(CalculateHash)
            .Sum();
    }

    int CalculateHash(string s)
    {
        var hash = 0;

        foreach (var ch in s)
        {
            hash = (hash + ch) * 17 % 256;
        }

        return hash;
    }

    private record Lens(string Label, int Length);
}
