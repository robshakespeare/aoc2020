using System.Linq;

namespace AoC.Day2
{
    public class Day2Solver : SolverBase
    {
        protected override long? SolvePart1Impl(string input)
        {
            IPasswordPolicy policy = new PasswordPolicy1();
            var lines = input.ReadAllLines().Select(PasswordLine.Parse);
            return policy.GetValidLines(lines).Count();
        }

        protected override long? SolvePart2Impl(string input)
        {
            IPasswordPolicy policy = new PasswordPolicy2();
            var lines = input.ReadAllLines().Select(PasswordLine.Parse);
            return policy.GetValidLines(lines).Count();
        }
    }
}
