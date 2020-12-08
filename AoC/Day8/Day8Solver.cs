using System;
using System.Linq;
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
            var originalInstructions = BootCodeComputer.Parse(input).Instructions.ToReadOnlyArray();

            for (var instructionIndex = 0; instructionIndex < originalInstructions.Count; instructionIndex++)
            {
                var instruction = originalInstructions[instructionIndex];

                if (instruction.Operation is "jmp" or "nop")
                {
                    var instructions = originalInstructions.ToArray();
                    var newOp = instruction.Operation == "jmp" ? "nop" : "jmp";
                    instructions[instructionIndex] = new Instruction(newOp, instruction.Argument);

                    try
                    {
                        var bootCodeComputer = new BootCodeComputer(instructions);
                        var result = bootCodeComputer.Evaluate();
                        Console.WriteLine($"Changed {instruction.Operation} to {newOp} at index {instructionIndex} and got successful result of {result}");
                        return result;
                    }
                    catch (InvalidOperationException e) when (e.Message == "Infinite loop detected")
                    {
                        // Intentionally blank, try the next instruction
                    }
                }
            }

            throw new InvalidOperationException("Failed to solve Day 8 Part 2");
        }
    }
}
