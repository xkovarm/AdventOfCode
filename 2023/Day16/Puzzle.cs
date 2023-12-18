namespace AdventOfCode.Day16;

internal class Puzzle : PuzzleBase
{
    // -- CALCULATION PARAMETERS AND RESULT ------------------------------------

    protected override string SampleResult => "51"; //"46";

    // -------------------------------------------------------------------------

    protected override string DoCalculation(string input)
    {
        var map = input
            .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
            .Select(x => x.Trim().ToArray())
            .ToArray();

        var maxX = map[0].Length - 1;
        var maxY = map.Length - 1;

        // PART 1:
        //return TraceBeam(new(-1, 0, Directions.Right)).ToString();

        return Enumerable.Range(0, maxX + 1).Select(i => TraceBeam(new(-1, i, Directions.Right)))
            .Concat(Enumerable.Range(0, maxX + 1).Select(i => TraceBeam(new(maxX + 1, i, Directions.Left))))
            .Concat(Enumerable.Range(0, maxY + 1).Select(i => TraceBeam(new(i, -1, Directions.Down))))
            .Concat(Enumerable.Range(0, maxY + 1).Select(i => TraceBeam(new(i, maxY + 1, Directions.Up))))
            .Max()
            .ToString();

        int TraceBeam(Beam start) 
        {
            var visited = new HashSet<Coord>();
            var history = new HashSet<Beam>();
            var beams = new Stack<Beam>();

            beams.Push(start);

            while (beams.Any())
            {
                var beam = CalculateNext(beams.Pop());

                if (beam.X < 0 || beam.Y < 0 || beam.X > maxX || beam.Y > maxY || history.Contains(beam))
                {
                    // the beam left the area or a cycle has been detected
                    continue;
                }

                visited.Add(new(beam.X, beam.Y));
                history.Add(beam);

                switch (map[beam.Y][beam.X])
                {
                    case '-' when beam.Direction == Directions.Up || beam.Direction == Directions.Down:     // split
                        beams.Push(beam with { Direction = Directions.Left });
                        beams.Push(beam with { Direction = Directions.Right });
                        break;
                    case '|' when beam.Direction == Directions.Left || beam.Direction == Directions.Right:  // split
                        beams.Push(beam with { Direction = Directions.Up });
                        beams.Push(beam with { Direction = Directions.Down });
                        break;
                    case '/':   // reflection
                        beams.Push(ReflectBeamSlash(beam));
                        break;
                    case '\\':  // reflection
                        beams.Push(ReflectBeamBackslash(beam));
                        break;
                    default:    // pass-through
                        beams.Push(beam);
                        break;
                }
            }

            return visited.Count;
        }

        Beam CalculateNext(Beam beam) => beam.Direction switch
        {
            Directions.Up => beam with { Y = beam.Y - 1 },
            Directions.Down => beam with { Y = beam.Y + 1 },
            Directions.Left => beam with { X = beam.X - 1 },
            Directions.Right => beam with { X = beam.X + 1 }
        };

        Beam ReflectBeamSlash(Beam beam) => beam.Direction switch
        {
            Directions.Up => beam with { Direction = Directions.Right },
            Directions.Down => beam with { Direction = Directions.Left },
            Directions.Left => beam with { Direction = Directions.Down },
            Directions.Right => beam with { Direction = Directions.Up }
        };

        Beam ReflectBeamBackslash(Beam beam) => beam.Direction switch
        {
            Directions.Up => beam with { Direction = Directions.Left },
            Directions.Down => beam with { Direction = Directions.Right },
            Directions.Left => beam with { Direction = Directions.Up },
            Directions.Right => beam with { Direction = Directions.Down }
        };
    }

    private enum Directions
    {
        Up,
        Down,
        Left,
        Right
    }

    private record Coord(int X, int Y);

    private record Beam(int X, int Y, Directions Direction);
}