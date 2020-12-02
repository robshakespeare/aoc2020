using System.Linq;

namespace AoC.Day2
{
    public class Day2Solver : SolverBase
    {
        protected override long? SolvePart1Impl(string input)
        {
            var lines = input.ReadAllLines().Select(PasswordLinePolicy1.Parse);

            return lines.Count(line => line.IsValid);
        }

        protected override long? SolvePart2Impl(string input)
        {
            var lines = input.ReadAllLines().Select(PasswordLinePolicy2.Parse);

            return lines.Count(line => line.IsValid);
        }
    }
}
