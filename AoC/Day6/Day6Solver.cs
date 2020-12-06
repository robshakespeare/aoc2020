using System.Collections.Generic;
using System.Linq;
using static System.Environment;

namespace AoC.Day6
{
    public class Day6Solver : SolverBase
    {
        protected override long? SolvePart1Impl(string input) => input
            .Split($"{NewLine}{NewLine}")
            .Select(group => group.Where(answer => answer >= 'a' && answer <= 'z').Distinct())
            .Aggregate(0, (accumulate, current) => accumulate + current.Count());

        protected override long? SolvePart2Impl(string input) => input
            .Split($"{NewLine}{NewLine}")
            .Select(group =>
            {
                var groupPeople = group.ReadAllLines().ToArray();
                var groupAnswers = new Dictionary<char, int>();
                foreach (var personAnswers in groupPeople)
                {
                    foreach (var personAnswer in personAnswers)
                    {
                        if (!groupAnswers.ContainsKey(personAnswer))
                        {
                            groupAnswers[personAnswer] = 0;
                        }

                        groupAnswers[personAnswer]++;
                    }
                }

                return groupAnswers.Where(groupAnswer => groupAnswer.Value == groupPeople.Length);
            })
            .Aggregate(0, (accumulate, current) => accumulate + current.Count());
    }
}
