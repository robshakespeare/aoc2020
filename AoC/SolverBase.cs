using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Crayon;

namespace AoC
{
    public interface ISolver
    {
        int DayNumber { get; }

        string DayName { get; }

        void Run();
    }

    public abstract class SolverBase : SolverBase<long?, long?>
    {
    }

    public abstract class SolverBase<TOutputPart1, TOutputPart2> : ISolver
    {
        private readonly InputLoader _inputLoader;

        public int DayNumber { get; }

        public abstract string DayName { get; }

        protected SolverBase() => _inputLoader = new InputLoader(DayNumber = SolverFactory.GetDayNumber(this));

        public void Run()
        {
            try
            {
                SolvePart1();
                SolvePart2(title: false);
            }
            catch (Exception e)
            {
                FileLogging.Logger.Error(e, "Solver failed");
                throw;
            }
        }

        [return: MaybeNull]
        private TOutput SolvePartTimed<TOutput>(int partNum, Func<string> input, Func<string, TOutput?> solve, bool title)
        {
            if (title)
            {
                Console.WriteLine($"Day {DayNumber}{(DayName is null or "" ? "" : ": " + DayName)}".Yellow());
                Console.WriteLine();
            }

            var puzzleInput = input()
                .NormalizeLineEndings().TrimEnd(); // Normalize line endings, and remove all trailing white-space (including trailing line endings)

            using var timer = new TimingBlock($"Part {partNum}");
            var result = solve(puzzleInput);
            timer.Stop();
            Console.WriteLine($"Part {partNum}: {result?.ToString().Green()}");
            return result;
        }

        [return: MaybeNull]
        public TOutputPart1 SolvePart1(bool title = true) => SolvePart1(_inputLoader.PuzzleInputPart1, title);

        [return: MaybeNull]
        public TOutputPart2 SolvePart2(bool title = true) => SolvePart2(_inputLoader.PuzzleInputPart2, title);

        [return: MaybeNull]
        public TOutputPart1 SolvePart1(string input, bool title = false) => SolvePartTimed(1, () => input, SolvePart1Impl, title);

        [return: MaybeNull]
        public TOutputPart2 SolvePart2(string input, bool title = false) => SolvePartTimed(2, () => input, SolvePart2Impl, title);

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

    public class SolverFactory
    {
        private readonly Dictionary<string, Type> _solvers;

        private SolverFactory(Assembly scanAssembly)
        {
            _solvers = scanAssembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.GetInterfaces().Any(it => it == typeof(ISolver)))
                .Select(t => new
                {
                    match = DayNumRegex.Match(t.FullName ?? ""),
                    type = t
                })
                .Where(x => x.match.Success)
                .ToDictionary(x => x.match.Groups["dayNum"].Value, x => x.type);
        }

        public ISolver? TryCreateSolver(string? dayNumber) => _solvers.TryGetValue(dayNumber ?? "", out var solverType)
            ? (ISolver?) Activator.CreateInstance(solverType)
            : null;

        public static SolverFactory CreateFactory<TStartup>() => new(typeof(TStartup).Assembly);

        public static SolverFactory CreateFactory() => CreateFactory<SolverFactory>();

        private static readonly Regex DayNumRegex = new(@"Day(?<dayNum>\d+)", RegexOptions.Compiled);

        private static int GetDayNumber(Type solverType)
        {
            var fullName = solverType.FullName;
            Match match;

            if (fullName != null &&
                (match = DayNumRegex.Match(fullName)).Success &&
                match.Groups["dayNum"].Success)
            {
                return int.Parse(match.Groups["dayNum"].Value);
            }

            throw new InvalidOperationException("Unable to get day number from type name: " + fullName);
        }

        public static int GetDayNumber<TOutputPart1, TOutputPart2>(SolverBase<TOutputPart1, TOutputPart2> solver) => GetDayNumber(solver.GetType());
    }
}
