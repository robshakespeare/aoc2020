using System.Collections.Generic;
using System.Linq;
using static System.Environment;

namespace AoC.Day6
{
    public class Day6Solver : SolverBase
    {
        protected override long? SolvePart1Impl(string input) => input
            .Split($"{NewLine}{NewLine}")
            .Select(group => group.Where(char.IsLetter).Distinct())
            .Sum(distinctGroupAnswers => distinctGroupAnswers.Count());

        protected override long? SolvePart2Impl(string input) => input
            .Split($"{NewLine}{NewLine}")
            .Select(group =>
            {
                var groupPeople = group.ReadLines().ToArray();
                var groupAnswers = new Dictionary<char, int>();

                foreach (var personAnswer in groupPeople.SelectMany(personAnswers => personAnswers))
                {
                    if (!groupAnswers.ContainsKey(personAnswer))
                    {
                        groupAnswers[personAnswer] = 0;
                    }

                    groupAnswers[personAnswer]++;
                }

                return groupAnswers.Where(groupAnswer => groupAnswer.Value == groupPeople.Length);
            })
            .Sum(answersInGroupWhereEveryoneAnsweredYes => answersInGroupWhereEveryoneAnsweredYes.Count());
    }
}
