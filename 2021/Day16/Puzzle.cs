namespace AdventOfCode.Day16
{
    internal class Puzzle : PuzzleBase
    {
        protected override string SampleResult => "1"; // "16";

        private class Packet
        {
            public long Version { get; set; }

            public long Type { get; set; }

            public long Value { get; set; }

            public Operations Operation { get; set; }

            public List<Packet> Children { get; set; }
        }

        private enum Operations
        {
            Nop = 4,
            Greater = 5,
            Lower = 6,
            Equal = 7,
            Sum = 0,
            Min = 2,
            Max = 3,
            Multiply = 1
        }

        public Puzzle()
        {
            // part 1
            //Debug.Assert(DoCalculation("8A004A801A8002F478") == "16");
            //Debug.Assert(DoCalculation("620080001611562C8802118E34") == "12");
            //Debug.Assert(DoCalculation("C0015000016115A2E0802F182340") == "23");
            //Debug.Assert(DoCalculation("A0016C880162017C3686B18A3D4780") == "31");

            // part 2
            Debug.Assert(DoCalculation("C200B40A82") == "3");
            Debug.Assert(DoCalculation("04005AC33890") == "54");
            Debug.Assert(DoCalculation("880086C3E88112") == "7");
            Debug.Assert(DoCalculation("CE00C43D881120") == "9");
            Debug.Assert(DoCalculation("D8005AC2A8F0") == "1");
            Debug.Assert(DoCalculation("F600BC2D8F") == "0");
            Debug.Assert(DoCalculation("9C005AC2F8F0") == "0");
        }

        protected override Task<string> GetSampleInputAsync()
        {
            return Task.FromResult("9C0141080250320F1802104A08" /*"8A004A801A8002F478"*/);
        }

        protected override string DoCalculation(string input)
        {
            var data = string.Join(string.Empty, input.Trim().Select(c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')));

            int index = 0;
            var packet = ParsePacket(data, ref index);

            //var value = Evaluate1(packet);
            var value = Evaluate2(packet);

            return value.ToString();
        }

        private long Evaluate1(Packet packet)
        {
            return packet.Version + packet.Children?.Select(Evaluate1).Sum() ?? 0;
        }

        private long Evaluate2(Packet packet)
        {
            return packet.Operation switch
            {
                Operations.Greater => Evaluate2(packet.Children[0]) > Evaluate2(packet.Children[1]) ? 1 : 0,
                Operations.Lower => Evaluate2(packet.Children[0]) < Evaluate2(packet.Children[1]) ? 1 : 0,
                Operations.Equal => Evaluate2(packet.Children[0]) == Evaluate2(packet.Children[1]) ? 1 : 0,
                Operations.Min => packet.Children.Select(Evaluate2).Min(),
                Operations.Sum => packet.Children.Select(Evaluate2).Sum(),
                Operations.Max => packet.Children.Select(Evaluate2).Max(),
                Operations.Multiply => packet.Children.Select(Evaluate2).Aggregate((a, b) => a * b),
                Operations.Nop => packet.Value
            };
        }

        private Packet ParsePacket(string data, ref int index)
        {
            var version = GetValue(data, 3, ref index);
            var type = GetValue(data, 3, ref index);

            // type: value
            if (type == 4)
            {
                var value = 0L;

                while (true)
                {
                    var last = GetValue(data, 1, ref index);

                    value <<= 4;
                    value |= GetValue(data, 4, ref index);

                    if (last == 0)
                    {
                        break;
                    }
                }

                return new Packet { Type = type, Version = version, Operation = Operations.Nop, Value = value  };
            }
            // type: operator
            else
            {
                var lengthType = GetValue(data, 1, ref index);
                var children = new List<Packet>();

                if (lengthType == 0)
                {
                    var length = GetValue(data, 15, ref index);
                    var lastIndex = index + length;

                    while (index < lastIndex)
                    {
                        children.Add(ParsePacket(data, ref index));
                    }
                }
                else
                {
                    var count = GetValue(data, 11, ref index);

                    for (int i = 0; i < count; i++)
                    {
                        children.Add(ParsePacket(data, ref index));
                    }
                }

                return new Packet { Type = type, Version = version, Operation = (Operations)type, Children = children };
            }
        }

        long GetValue(string data, int bits, ref int index)
        {
            var value = data.Substring(index, bits);
            index += bits;

            return Convert.ToInt32(value, 2);
        }
    }
}
