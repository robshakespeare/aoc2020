using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.BootCode
{
    public class BootCodeComputer
    {
        private readonly Instruction[] _instructions;
        private readonly ProgramState _state;

        public BootCodeComputer(IEnumerable<Instruction> instructions)
        {
            _instructions = instructions.ToArray();
            _state = new ProgramState();
        }

        /// <summary>
        /// Parses the specified Boot Code program, and creates a computer for it in its initial state.
        /// </summary>
        public static BootCodeComputer Parse(string inputProgram) => new(inputProgram.ReadLines().Select(Instruction.ParseLine));

        /// <summary>
        /// Current value of the `Accumulator` register.
        /// </summary>
        public long Accumulator => _state.Accumulator;

        /// <summary>
        /// Returns a read only version of this computer's instructions.
        /// </summary>
        public IReadOnlyList<Instruction> GetInstructions() => _instructions.ToReadOnlyArray();

        /// <summary>
        /// Evaluates the computer until it halts, and then returns the value of the Accumulator.
        /// </summary>
        public long Evaluate()
        {
            while (EvaluateNextInstruction())
            {
            }
            return Accumulator;
        }

        /// <summary>
        /// Evaluates the next instruction in the computer.
        /// Returns true if the computer should continue to be evaluated, otherwise if the computer has now halted returns false.
        /// </summary>
        public bool EvaluateNextInstruction()
        {
            var currentInstructionPointer = _state.NextInstructionPointer;
            if (currentInstructionPointer == _instructions.Length)
            {
                return false;
            }

            if (_state.VisitedInstructions.Contains(currentInstructionPointer))
            {
                throw new InvalidOperationException("Infinite loop detected");
            }

            var gotoInstructionPointer = EvalInstruction(_instructions[currentInstructionPointer], currentInstructionPointer);
            _state.VisitedInstructions.Add(currentInstructionPointer);
            _state.NextInstructionPointer = gotoInstructionPointer ?? currentInstructionPointer + 1;
            return true;
        }

        private long? EvalInstruction(Instruction instruction, long currentInstructionPointer) =>
            instruction.Operation switch
            {
                "acc" => EvalAccumulateInstruction(instruction),
                "jmp" => EvalJumpInstruction(instruction, currentInstructionPointer),
                "nop" => EvalNoOpInstruction(),
                _ => throw new InvalidOperationException("Invalid operation: " + instruction.Operation)
            };

        private long? EvalAccumulateInstruction(Instruction instruction)
        {
            _state.Accumulator += instruction.Argument;
            return null;
        }

        private static long? EvalJumpInstruction(Instruction instruction, long currentInstructionPointer) => currentInstructionPointer + instruction.Argument;

        private static long? EvalNoOpInstruction() => null;
    }
}
