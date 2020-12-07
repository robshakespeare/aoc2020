namespace AoC.Day7
{
    public class Day7Solver : SolverBase
    {
        protected override long? SolvePart1Impl(string input) => BagRules.Parse(input).CountBagColorsCanContain("shiny gold");

        protected override long? SolvePart2Impl(string input) => BagRules.Parse(input).CountBagsRequiredInside("shiny gold");
    }
}
