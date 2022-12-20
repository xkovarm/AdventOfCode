namespace AdventOfCode.Day20
{
    internal class Puzzle : PuzzleBase
    {
        // -- CALCULATION PARAMETERS AND RESULT ------------------------------------

        protected override string SampleResult => "1623178306"; // PART 1: "3";

        // -------------------------------------------------------------------------

        protected override string DoCalculation(string input)
        {
            var file = input
                .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries) 
                .Select(i => long.Parse(i))
                .ToArray();

            // PART 1: var decrypted = Decrypt(file, 1);
            var decrypted = DecryptFile(file, 10, 811589153);

            var result = GetGroveCoordinates(decrypted).Sum();

            return result.ToString();
        }

        private IEnumerable<long> GetGroveCoordinates(long[] file)
        {
            var zero = Array.IndexOf(file, 0);

            yield return file[(zero + 1000) % file.Length];
            yield return file[(zero + 2000) % file.Length];
            yield return file[(zero + 3000) % file.Length];
        }

        private long[] DecryptFile(long[] file, int iterations, long? decryptionKey = null)
        {
            if (decryptionKey is not null)
            {
                file = file.Select(f => f * decryptionKey.Value).ToArray();
            }

            var list = new LinkedList<long>(file);
            var nodesInOriginalOrder = new List<LinkedListNode<long>>();

            for (var node = list.First; node != null; node = node.Next)
            {
                nodesInOriginalOrder.Add(node);
            }

            for (var r = 0; r < iterations; r++)
            {
                foreach (var node in nodesInOriginalOrder)
                {
                    if (node.Value == 0)
                    {
                        continue;
                    }

                    var target = node.Value > 0 ? node.Next ?? list.First : node.Previous ?? list.Last;
                    list.Remove(node);

                    long reducedValue = node.Value % list.Count;

                    if (reducedValue > 0)
                    {
                        for (long i = 0; i < reducedValue - 1; i++)
                        {
                            target = target!.Next ?? list.First;
                        }
                    }
                    else
                    {
                        for (long i = 0; i < -reducedValue; i++)
                        {
                            target = target!.Previous ?? list.Last;
                        }
                    }

                    list.AddAfter(target!, node);
                }
            }

            return list.ToArray();
        }
    }
}
