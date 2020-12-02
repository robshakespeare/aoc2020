using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.RegularExpressions;
using Crayon;

namespace AoC
{
    public interface ISolver
    {
        int DayNumber { get; }

        void Run();
    }

    public abstract class SolverBase : SolverBase<long?, long?>
    {
    }

    public abstract class SolverBase<TOutputPart1, TOutputPart2> : ISolver
    {
        private readonly InputLoader _inputLoader;

        public int DayNumber { get; }

        protected SolverBase() => _inputLoader = new InputLoader(DayNumber = this.GetDayNumber());

        public void Run()
        {
            try
            {
                Console.OutputEncoding = Encoding.UTF8;
                Console.WriteLine($"Day {DayNumber}".Yellow());

                SolvePart1();
                SolvePart2();
            }
            catch (Exception e)
            {
                FileLogging.Logger.Error(e, "Solver failed");
                throw;
            }
        }

        [return: MaybeNull]
        private delegate TResult SolverFunc<in T, out TResult>(T arg);

        [return: MaybeNull]
        private static TOutput SolvePartTimed<TOutput>(int partNum, string input, SolverFunc<string, TOutput> solve)
        {
            input = input.NormalizeAndTrimEnd();

            using var timer = new TimingBlock($"Part {partNum}");
            var result = solve(input);
            timer.Stop();
            Console.WriteLine($"Part {partNum}: {result?.ToString().Green()}");
            return result;
        }

        [return: MaybeNull]
        public TOutputPart1 SolvePart1() => SolvePart1(_inputLoader.PuzzleInputPart1);

        [return: MaybeNull]
        public TOutputPart2 SolvePart2() => SolvePart2(_inputLoader.PuzzleInputPart2);

        [return: MaybeNull]
        public TOutputPart1 SolvePart1(string input) => SolvePartTimed(1, input, SolvePart1Impl);

        [return: MaybeNull]
        public TOutputPart2 SolvePart2(string input) => SolvePartTimed(2, input, SolvePart2Impl);

        [return: MaybeNull]
        protected virtual TOutputPart1 SolvePart1Impl(string input)
        {
            Console.WriteLine("Part 1 not yet implemented".BrightMagenta());
            return default;
        }

        [return: MaybeNull]
        protected virtual TOutputPart2 SolvePart2Impl(string input)
        {
            Console.WriteLine("Part 2 not yet implemented".BrightMagenta());
            return default;
        }
    }

    public static class SolverBaseExtensions
    {
        private static readonly Regex DayNumRegex = new(@"Day(?<dayNum>\d+)", RegexOptions.Compiled);

        public static int GetDayNumber<TOutputPart1, TOutputPart2>(this SolverBase<TOutputPart1, TOutputPart2> solver)
        {
            var fullName = solver.GetType().FullName;
            Match match;

            if (fullName != null &&
                (match = DayNumRegex.Match(fullName)).Success &&
                match.Groups["dayNum"].Success)
            {
                return int.Parse(match.Groups["dayNum"].Value);
            }

            throw new InvalidOperationException("Unable to get day number from type name: " + fullName);
        }
    }
}
