using System;

namespace AoC.BootCode
{
    public record Instruction(
        string Operation,
        long Argument)
    {

        public static Instruction ParseLine(string line)
        {
            var parts = line.Split(' ');
            if (parts.Length != 2)
            {
                throw new InvalidOperationException("Invalid line: " + line);
            }

            return new Instruction(
                parts[0],
                long.TryParse(parts[1], out var argument) ? argument : throw new InvalidOperationException("Invalid argument: " + parts[1]));
        }
    }
}
