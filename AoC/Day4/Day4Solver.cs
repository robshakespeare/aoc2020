using System.Linq;

namespace AoC.Day4
{
    public class Day4Solver : SolverBase
    {
        public override string DayName => "Passport Processing";

        protected override long? SolvePart1Impl(string input) => Passport.ParseBatch(input).Count(passport => passport.HasRequiredFields);

        protected override long? SolvePart2Impl(string input) => Passport.ParseBatch(input).Count(passport => passport.IsValid);
    }
}
