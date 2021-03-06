using System.Collections.Generic;

namespace AoC.Day8
{
    public class ProgramState
    {
        public HashSet<long> VisitedInstructions { get; } = new();

        public long NextInstructionPointer { get; set; } = 0;

        public long Accumulator { get; set; } = 0;
    }
}
