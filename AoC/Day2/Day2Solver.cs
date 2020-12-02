using System.Linq;

namespace AoC.Day2
{
    public class Day2Solver : SolverBase
    {
        protected override long? SolvePart1Impl(string input) => Solve(input, new PasswordPolicy1());

        protected override long? SolvePart2Impl(string input) => Solve(input, new PasswordPolicy2());

        private static long? Solve(string input, IPasswordPolicy policy)
        {
            var lines = input.ReadAllLines().Select(PasswordLine.Parse);
            return policy.GetValidLines(lines).Count();
        }
    }
}
