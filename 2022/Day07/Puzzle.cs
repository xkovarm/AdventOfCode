using System.Data;

namespace AdventOfCode.Day07
{
    internal class Puzzle : PuzzleBase
    {
        // -- CALCULATION PARAMETERS AND RESULT ------------------------------------

        protected override string SampleResult => "24933642"; // PART 1: "95437";

        // -------------------------------------------------------------------------

        private abstract class Node
        {
            public string Name { get; set; }
        }

        private class File : Node
        {
            public int Size { get; set; }
        }

        private class Dir : Node
        {
            public Dir Parent { get; set; }

            public List<Node> Children { get; } = new();
        }


        protected override Task<string> GetSampleInputAsync()
        {
            return Task.FromResult(@"$ cd /
$ ls
dir a
14848514 b.txt
8504156 c.dat
dir d
$ cd a
$ ls
dir e
29116 f
2557 g
62596 h.lst
$ cd e
$ ls
584 i
$ cd ..
$ cd ..
$ cd d
$ ls
4060174 j
8033020 d.log
5626152 d.ext
7214296 k");
        }

        protected override string DoCalculation(string input)
        {
            var lines = input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).ToList();

            var root = Parse(lines);
            
            // PART 1: var size = Calculate(root);
            var size = Calculate2(root);

            return size.ToString();
        }

        private long Calculate(Dir root)
        {
            var dirs = ScanDirectories(root);

            return dirs.Where(d => d.Size <= 100000).Sum(d => d.Size);
        }

        private long Calculate2(Dir root)
        {
            var dirs = ScanDirectories(root);

            var currentlyUsed = dirs.Single(d => d.Dir.Name == "/").Size;
            var toBeDeleted = 30000000 - (70000000 - currentlyUsed);

            var result = dirs.Where(d => d.Size >= toBeDeleted).OrderBy(d => d.Size).First().Size;

            return result;
        }

        private List<(Dir Dir, long Size)> ScanDirectories(Dir root)
        {
            var dirs = new List<(Dir Dir, long Size)>();

            CalculateStep(root);

            return dirs;

            long CalculateStep(Dir dir)
            {
                var size = dir.Children.OfType<Dir>().Sum(CalculateStep) + dir.Children.OfType<File>().Sum(f => f.Size);
                dirs.Add((dir, size));
                return size;
            }
        }

        private Dir Parse(IList<string> lines)
        {
            var pos = 0;

            Dir root = new Dir { Name = "/" };
            Dir current = root;

            var line = GetLine();

            while (line is not null)
            {
                if (line[0] == "$")
                {
                    if (line[1] == "cd")
                    {
                        Cd(line[2]);
                        line = GetLine();
                        continue;
                    }
                    else if (line[1] == "ls")
                    {
                        Ls();
                        continue;
                    }
                }
                throw new InvalidOperationException("Ooops, what are we doing here?");
            }

            return root;

            void Ls()
            {
                line = GetLine();

                while (line is not null && line[0] != "$")
                {
                    if (line[0] == "dir")
                    {
                        current.Children.Add(new Dir { Name = line[1], Parent = current });
                    }
                    else
                    {
                        current.Children.Add(new File { Name = line[1], Size = int.Parse(line[0]) });
                    }

                    line = GetLine();
                }
            }

            void Cd(string dir)
            {
                current = dir switch
                {
                    "/" => root,
                    ".." => current.Parent,
                    _ => current.Children.OfType<Dir>().Single(n => n.Name == dir)    // let's belive that the folder exists
                };
            }

            string[] GetLine()
            {
                return pos < lines.Count ? lines[pos++].Split(' ', StringSplitOptions.RemoveEmptyEntries) : null;
            }
        }
    }
}