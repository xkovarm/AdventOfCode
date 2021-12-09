using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Day09
{
    internal class Puzzle : PuzzleBase
    {
        protected override string SampleResult => "1134"; // "15";

        protected override Task<string> GetCheckInputAsync()
        {
            return File.ReadAllTextAsync(@"Day09\input.txt");
        }

        protected override Task<string> GetSampleInputAsync()
        {
            return Task.FromResult(@"2199943210
3987894921
9856789892
8767896789
9899965678");
        }

        protected override string DoCalculation(string input)
        {
            var data = input
                .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.ToArray())
                .ToArray();

            //return DoCalculation1(data);
            return DoCalculation2(data);
        }

        private string DoCalculation2(char[][] data)
        {
            var lowestPoints = GetLowestPoints(data);
            var basins = SearchBasins(lowestPoints);
            var result = basins.Select(b => b.Count()).OrderByDescending(b => b).Take(3).Aggregate((a, b) => a * b);

            return result.ToString();

            IEnumerable<IEnumerable<(int X, int Y)>> SearchBasins(IEnumerable<(int X, int Y)> lowestPoints)
            {
                var sizeX = data[0].Length;
                var sizeY = data.Length;
                var queue = new Queue<(int X, int Y)>();

                foreach (var minimum in lowestPoints)
                {
                    var basin = new List<(int X, int Y)>();

                    queue.Enqueue(minimum);
                    data[minimum.Y][minimum.X] = '#';

                    while (queue.Count > 0)
                    {
                        var current = queue.Dequeue();
                        basin.Add(current);

                        GetNeighbors(current.X, current.Y, sizeX, sizeY)
                            .Where(n => data[n.Y][n.X] < '9' && data[n.Y][n.X] != '#')
                            .ForEach(n =>
                            {
                                queue.Enqueue(n);
                                data[n.Y][n.X] = '#';
                            });
                    }

                    yield return basin;
                }
            }
        }

        private string DoCalculation1(char[][] data)
        {
            var result = GetLowestPoints(data).Select(p => data[p.Y][p.X]-'0' + 1).Sum();

            return result.ToString();
        }

        private IEnumerable<(int X, int Y)> GetLowestPoints(char[][] data)
        {
            var sizeX = data[0].Length;
            var sizeY = data.Length;

            for (int y = 0; y < sizeY; y++)
            {
                for (int x = 0; x < sizeX; x++)
                {
                    var neighbors = GetNeighbors(x, y, sizeX, sizeY);

                    if (!neighbors.Any(n => data[n.Y][n.X] <= data[y][x]))
                    {
                        yield return (x, y);
                    }
                }
            }
        }

        private IEnumerable<(int X, int Y)> GetNeighbors(int x, int y, int sizeX, int sizeY)
        {
            if (x > 0)
            {
                yield return (x - 1, y);
            }

            if (y > 0)
            {
                yield return (x, y - 1);
            }

            if (x < sizeX - 1)
            {
                yield return (x + 1, y);
            }

            if (y < sizeY - 1)
            {
                yield return (x, y + 1);
            }
        }
    }
}
