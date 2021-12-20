using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode
{
    /// <summary>
    /// Solver for Day 16
    /// </summary>
    public class Day16
    {
        public uint Part1(string[] input)
        {
            string line = input.First().Trim();

            string binary = string.Join(string.Empty, line.Select(c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')));

            TryParseOperator(binary, out OperatorPacket packet, out _);

            return AddVersions(packet);
        }

        public ulong Part2(string[] input)
        {
            string line = input.First().Trim();

            string binary = string.Join(string.Empty, line.Select(c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')));

            TryParseOperator(binary, out OperatorPacket packet, out _);

            return packet.Evaluate();
        }

        private static bool TryParseLiteral(string binary, out LiteralPacket packet, out int taken)
        {
            uint version = Convert.ToUInt32(binary[..3], 2);
            uint type = Convert.ToUInt32(binary[3..6], 2);

            if (type != 4)
            {
                packet = null;
                taken = 0;
                return false;
            }

            int i = 6;
            string value = "";
            string chunk = "";

            while (true)
            {
                chunk = binary[i..(i + 5)];
                value += chunk[1..];
                i += 5;

                if (chunk.StartsWith("0"))
                {
                    break;
                }
            }

            ulong parsed = Convert.ToUInt64(value, 2);

            packet = new LiteralPacket { Version = version, Type = type, Value = parsed };
            taken = i;
            return true;
        }

        private static bool TryParseOperator(string binary, out OperatorPacket packet, out int taken)
        {
            uint version = Convert.ToUInt32(binary[..3], 2);
            uint type = Convert.ToUInt32(binary[3..6], 2);
            char lengthType = binary[6];

            int offset = 7;
            offset += lengthType == '1' ? 11 : 15;

            string lengthBits = binary[7..offset];
            int length = (int)Convert.ToUInt32(lengthBits, 2);

            packet = new OperatorPacket
            {
                Type = type,
                Version = version,
                Inner = new List<Packet>()
            };

            int parsedPackets = 0;
            int end = offset + length;

            while (true)
            {
                if (TryParseLiteral(binary[offset..], out LiteralPacket literal, out taken))
                {
                    packet.Inner.Add(literal);
                    offset += taken;
                }
                // recurse into sub-operator if literal wasn't found
                else if (TryParseOperator(binary[offset..], out OperatorPacket subOperator, out taken))
                {
                    packet.Inner.Add(subOperator);
                    offset += taken;
                }
                else
                {
                    throw new InvalidOperationException("Unable to find either literal or operator packet");
                }

                parsedPackets++;

                if (lengthType == '0' && offset >= end)
                {
                    break;
                }

                if (lengthType == '1' && parsedPackets == length)
                {
                    break;
                }
            }

            taken = offset;
            return true;
        }

        private static uint AddVersions(Packet packet)
        {
            uint result = packet.Version;

            if (packet is OperatorPacket o)
            {
                foreach (Packet inner in o.Inner)
                {
                    result += AddVersions(inner);
                }
            }

            return result;
        }
    }

    public abstract class Packet
    {
        public uint Version { get; set; }

        public uint Type { get; set; }

        public abstract ulong Evaluate();
    }

    public class LiteralPacket : Packet
    {
        public ulong Value { get; set; }

        public override ulong Evaluate()
        {
            return this.Value;
        }
    }

    public class OperatorPacket : Packet
    {
        public ICollection<Packet> Inner { get; set; } = new List<Packet>();

        public override ulong Evaluate()
        {
            ulong result;
            Packet first;
            Packet second;

            switch (this.Type)
            {
                case 0:
                    result = 0;

                    foreach (Packet inner in this.Inner)
                    {
                        result += inner.Evaluate();
                    }

                    return result;

                case 1:
                    result = 1;

                    foreach (Packet inner in this.Inner)
                    {
                        result *= inner.Evaluate();
                    }

                    return result;

                case 2:
                    result = ulong.MaxValue;

                    foreach (Packet inner in this.Inner)
                    {
                        result = Math.Min(result, inner.Evaluate());
                    }

                    return result;

                case 3:
                    result = ulong.MinValue;

                    foreach (Packet inner in this.Inner)
                    {
                        result = Math.Max(result, inner.Evaluate());
                    }

                    return result;

                case 5:
                    first = this.Inner.ElementAt(0);
                    second = this.Inner.ElementAt(1);

                    return first.Evaluate() > second.Evaluate() ? (ulong)1 : 0;

                case 6:
                    first = this.Inner.ElementAt(0);
                    second = this.Inner.ElementAt(1);

                    return first.Evaluate() < second.Evaluate() ? (ulong)1 : 0;

                case 7:
                    first = this.Inner.ElementAt(0);
                    second = this.Inner.ElementAt(1);

                    return first.Evaluate() == second.Evaluate() ? (ulong)1 : 0;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
