using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Puzzles
{
    public record OperationBlock
    {
        private readonly List<Operation> _operations = new();

        public IReadOnlyList<Operation> Operations => _operations.AsReadOnly();
        public string Bitmask { get; }

        public OperationBlock(string mask)
        {
            Bitmask = mask;
        }

        public void AddOperation(Operation op)
        {
            _operations.Add(op);
        }
    }

    public record Operation
    {
        public ulong Address { get; }
        public ulong Value { get; }
        public BitArray AddressBits => GetBits(Address);
        public BitArray ValueBits => GetBits(Value);

        public Operation(ulong address, ulong value)
        {
            Address = address;
            Value = value;
        }

        private BitArray GetBits(ulong val)
        {
            var bytes = BitConverter.GetBytes(val);
            return new BitArray(bytes);
        }
    }

    public class DockingData
    {
        private const int BitSize = 36;
        private readonly Dictionary<ulong, BitArray> _memory = new();

        public ulong Solve1(string[] program)
        {
            var blocks = new List<OperationBlock>();

            for (var i = 0; i < program.Length;)
            {
                var bitmask = ParseBitmask(program[i++]);
                var block = new OperationBlock(bitmask);
                blocks.Add(block);

                while (i < program.Length && !program[i].StartsWith("mask"))
                {
                    var op = ParseOperation(program[i++]);
                    block.AddOperation(op);
                }
            }

            foreach (var block in blocks)
            {
                var bitmask = block.Bitmask.Reverse().ToArray();

                foreach (var operation in block.Operations)
                {
                    if (!_memory.ContainsKey(operation.Address))
                        _memory.Add(operation.Address, new BitArray(BitConverter.GetBytes(0ul)));

                    var memory = _memory[operation.Address];
                    var bits = operation.ValueBits;

                    for (var i = 0; i < BitSize; i++)
                    {
                        memory[i] = bitmask[i] switch
                        {
                            'X' => bits[i],
                            '1' => true,
                            '0' => false,
                            _ => throw new ArgumentOutOfRangeException()
                        };
                    }
                }
            }

            var sumInMemory = 0ul;

            foreach (var address in _memory.Values)
            {
                var bytes = BitConverter.GetBytes(0ul);
                address.CopyTo(bytes, 0);
                sumInMemory += BitConverter.ToUInt64(bytes);
            }

            return sumInMemory;
        }

        private Operation ParseOperation(string operation)
        {
            var regex = new Regex("^mem[[](?<address>[0-9]+)[]] = (?<value>[0-9]+)$");

            var match = regex.Match(operation);

            if (!match.Success) throw new ArgumentOutOfRangeException(nameof(operation));

            var address = ulong.Parse(match.Groups["address"].Value);
            var value = ulong.Parse(match.Groups["value"].Value);

            return new Operation(address, value);
        }

        public string ParseBitmask(string mask)
        {
            var regex = new Regex("^mask = (?<mask>[X|1|0]{36})$");
            var match = regex.Match(mask);

            if (!match.Success) throw new ArgumentOutOfRangeException(nameof(mask));

            return match.Groups["mask"].Value;
        }

        public ulong Solve2(string[] program)
        {
            var blocks = ParseProgram(program);

            foreach (var block in blocks)
            {
                foreach (var operation in block.Operations)
                {
                    // reverse bitmask for LSB to be at index 0
                    var bitmask = block.Bitmask.Reverse().ToArray();

                    var addressMask = createAddressMask(bitmask, operation.AddressBits);
                    var addresses = createAddresses(addressMask);

                    foreach (var address in addresses)
                    {
                        if (!_memory.ContainsKey(address))
                            _memory.Add(address, new BitArray(BitConverter.GetBytes(0ul)));

                        var memoryBits = _memory[address];
                        var valueBits = operation.ValueBits;

                        for (var i = 0; i < BitSize; i++)
                            memoryBits[i] = valueBits[i];
                    }
                }
            }

            return sumInMemory();
        }

        private ulong sumInMemory()
        {
            var sumInMemory = 0ul;

            foreach (var address in _memory.Values)
            {
                var bytes = BitConverter.GetBytes(0ul);
                address.CopyTo(bytes, 0);
                sumInMemory += BitConverter.ToUInt64(bytes);
            }

            return sumInMemory;
        }

        private static ulong[] createAddresses(char[] maskedAddress)
        {
            var wildcards = maskedAddress.Count(x => x == 'X');
            var combinations = (int) Math.Pow(2, wildcards);

            var wildcardIndices = maskedAddress.Select((c, idx) => (Char: c, Index: idx))
                .Where(tuple => tuple.Char == 'X')
                .Select(tuple => tuple.Index)
                .ToArray();

            var addresses = new BitArray[combinations];
            for (var i = 0; i < combinations; i++)
            {
                addresses[i] = new BitArray(BitConverter.GetBytes(0ul));
                for (var k = 0; k < maskedAddress.Length; k++)
                    addresses[i].Set(k, maskedAddress[k] switch
                    {
                        '1' => true,
                        _ => false
                    });

                // create binary representation of the combination bits
                var ba = new BitArray(new[] {i});

                for (var j = 0; j < wildcards; j++)
                {
                    // for each of the combinations set the combinations bits for the Xs
                    addresses[i].Set(wildcardIndices[j], ba[j]);
                }
            }

            var decimalAddresses = addresses.Select(ba =>
            {
                var bytes = BitConverter.GetBytes(0ul);
                ba.CopyTo(bytes, 0);
                return BitConverter.ToUInt64(bytes);
            }).ToArray();
            return decimalAddresses;
        }

        private static char[] createAddressMask(char[] bitmask, BitArray addressBits)
        {
            var maskedAddress = new char[BitSize];

            for (var i = 0; i < BitSize; i++)
            {
                maskedAddress[i] = bitmask[i] switch
                {
                    '0' => addressBits[i] ? '1' : '0',
                    '1' => '1',
                    'X' => 'X',
                    _ => throw new ArgumentOutOfRangeException()
                };
            }

            return maskedAddress;
        }

        private List<OperationBlock> ParseProgram(string[] program)
        {
            var blocks = new List<OperationBlock>();

            for (var i = 0; i < program.Length;)
            {
                var bitmask = ParseBitmask(program[i++]);
                var block = new OperationBlock(bitmask);
                blocks.Add(block);

                while (i < program.Length && !program[i].StartsWith("mask"))
                {
                    var op = ParseOperation(program[i++]);
                    block.AddOperation(op);
                }
            }

            return blocks;
        }
    }
}