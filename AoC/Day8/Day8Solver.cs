using System;
using AoC.BootCode;

namespace AoC.Day8
{
    public class Day8Solver : SolverBase
    {
        protected override long? SolvePart1Impl(string input)
        {
            var bootCodeComputer = BootCodeComputer.Parse(input);
            try
            {
                bootCodeComputer.Evaluate();
            }
            catch (InvalidOperationException e) when (e.Message == "Infinite loop detected")
            {
                return bootCodeComputer.Accumulator;
            }

            throw new InvalidOperationException("Expected Day 8 Part 1's puzzle input to produce an Infinite loop error, but got none.");
        }

        protected override long? SolvePart2Impl(string input)
        {
            return base.SolvePart2Impl(input);
        }
    }
}
