using System.Data;
using System.Text.RegularExpressions;

namespace AdventOfCode.Day19
{
    internal class Puzzle : PuzzleBase
    {
        // -- CALCULATION PARAMETERS AND RESULT ------------------------------------

        protected override string SampleResult => "3472"; // PART 1: "33";

        // -------------------------------------------------------------------------

        protected override string DoCalculation(string input)
        {
            var blueprints = input
                .Replace(Environment.NewLine, string.Empty)
                .Split("Blueprint", StringSplitOptions.RemoveEmptyEntries)
                .Select(i => Regex.Matches(i, "[0-9]+").Select(m => int.Parse(m.Value)).ToArray())
                .Select(i => new Blueprint(i[0], i[1], i[2], i[3], i[4], i[5], i[6]))
                .ToList();

            // PART 1: var result = blueprints.Select(b => b.N * CheckBlueprint(b, 24)).Sum();
            var result = blueprints.Take(3).Select(b => CheckBlueprint(b, 32)).Aggregate((a, b) => a * b);

            return result.ToString();
        }

        private int CheckBlueprint(Blueprint blueprint, int time)
        {
            var visited = new HashSet<State>();
            var queue = new Queue<State>();

            queue.Enqueue(new State(0, 0, 0, 0, 1, 0, 0, 0, time));

            int found = 0;

            while (queue.TryDequeue(out var state)) 
            {
                found = int.Max(found, state.Geode);

                if (state.Time == 0)
                {
                    continue;
                }

                var oreRobots = int.Min(state.OreRobots, blueprint.MaxOreCosts);
                var clayRobots = int.Min(state.ClayRobots, blueprint.ObsidianCostsInClay);
                var obsidianRobots = int.Min(state.ObsidianRobots, blueprint.GeodeCostsInObsidian);

                var ore = int.Min(state.Ore, state.Time * blueprint.MaxOreCosts - oreRobots * (state.Time - 1));
                var clay = int.Min(state.Clay, state.Time * blueprint.ObsidianCostsInClay - clayRobots * (state.Time - 1));
                var obsidian = int.Min(state.Obsidian, state.Time * blueprint.GeodeCostsInObsidian - obsidianRobots * (state.Time - 1));

                var newState = new State(ore, clay, obsidian, state.Geode, oreRobots, clayRobots, obsidianRobots, state.GeodeRobots, state.Time);

                if (visited.Contains(newState))
                {
                    continue;
                }

                visited.Add(newState);

                queue.Enqueue(new State(
                    ore + oreRobots, clay + clayRobots, obsidian + obsidianRobots, state.Geode + state.GeodeRobots,
                    oreRobots, clayRobots, obsidianRobots, state.GeodeRobots, state.Time - 1));

                if (ore >= blueprint.OreCostsInOre)
                {
                    queue.Enqueue(new State(ore + oreRobots - blueprint.OreCostsInOre, clay + clayRobots, obsidian + obsidianRobots, 
                        state.Geode + state.GeodeRobots, oreRobots + 1, clayRobots, obsidianRobots, state.GeodeRobots, state.Time - 1));
                }

                if (ore >= blueprint.ClayCostsInOre)
                {
                    queue.Enqueue(new State(ore + oreRobots - blueprint.ClayCostsInOre, clay + clayRobots, obsidian + obsidianRobots,
                        state.Geode + state.GeodeRobots, oreRobots, clayRobots + 1, obsidianRobots, state.GeodeRobots, state.Time - 1));
                }

                if (ore >= blueprint.ObsidianCostsInOre && clay >= blueprint.ObsidianCostsInClay)
                {
                    queue.Enqueue(new State(ore + oreRobots - blueprint.ObsidianCostsInOre, clay + clayRobots - blueprint.ObsidianCostsInClay, obsidian + obsidianRobots,
                        state.Geode + state.GeodeRobots, oreRobots, clayRobots, obsidianRobots + 1, state.GeodeRobots, state.Time - 1));
                }

                if (ore >= blueprint.GeodeCostsInOre && obsidian >= blueprint.GeodeCostsInObsidian)
                {
                    queue.Enqueue(new State(ore + oreRobots - blueprint.GeodeCostsInOre, clay + clayRobots, obsidian + obsidianRobots - blueprint.GeodeCostsInObsidian,
                        state.Geode + state.GeodeRobots, oreRobots, clay, obsidianRobots, state.GeodeRobots + 1, state.Time - 1));
                }
            }

            return found;
        }

        private record Blueprint(int N, int OreCostsInOre, int ClayCostsInOre, int ObsidianCostsInOre, int ObsidianCostsInClay, int GeodeCostsInOre, int GeodeCostsInObsidian)
        {
            public int MaxOreCosts => new[] { OreCostsInOre, ClayCostsInOre, ObsidianCostsInOre, GeodeCostsInOre }.Max();
        }

        private record State(int Ore, int Clay, int Obsidian, int Geode, int OreRobots, int ClayRobots, int ObsidianRobots, int GeodeRobots, int Time);
    }
}
