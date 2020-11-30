using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.RegularExpressions;
using Crayon;

namespace AoC
{
    public class SolverBase : SolverBase<long?, long?>
    {

    }

    public class SolverBase<TOutputPart1, TOutputPart2>
    {
        public int DayNumber { get; }

        public SolverBase()
        {
            DayNumber = this.GetDayNumber();
        }

        public void Run()
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine($"Day {DayNumber}".Yellow());

            // rs-todo: resume here:
            ////if (!Input.IsValueCreated)
            ////{
            ////    var input = Input.Value;
            ////    Trace.WriteLine("Ensured input was loaded. Input type is " + input?.GetType());
            ////}

            ////SolvePart1();
            ////SolvePart2();
        }

        private static TOutput SolvePartTimed<TOutput>(int partNum, Func<TOutput> solve)
        {
            using var timer = new TimingBlock($"Part {partNum}");
            var result = solve();
            timer.Stop();
            Console.Write($"Part {partNum}: {result?.ToString().Green()}");
            return result;
        }

        [return: MaybeNull]
        public virtual TOutputPart1 SolvePart1(string input)
        {
            Console.WriteLine("Part 1 not yet implemented");
            return default;
        }

        [return: MaybeNull]
        public virtual TOutputPart2 SolvePart2(string input)
        {
            Console.WriteLine("Part 2 not yet implemented");
            return default;
        }
    }

    public static class SolverBaseExtensions
    {
        private static readonly Regex DayNumRegex = new Regex(@"Day(?<dayNum>\d+)", RegexOptions.Compiled);

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
