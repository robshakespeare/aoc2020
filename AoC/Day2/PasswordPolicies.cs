using System.Collections.Generic;
using System.Linq;

namespace AoC.Day2
{
    public interface IPasswordPolicy
    {
        bool IsValid(PasswordLine line);

        IEnumerable<PasswordLine> GetValidLines(IEnumerable<PasswordLine> lines) => lines.Where(IsValid);
    }

    public class PasswordPolicy1 : IPasswordPolicy
    {
        public bool IsValid(PasswordLine line)
        {
            var requiredCharCount = line.Password.Count(c => c == line.RequiredChar);

            // Lower and Upper bound are both inclusive.
            return requiredCharCount >= line.LowerBound && requiredCharCount <= line.UpperBound;
        }
    }

    public class PasswordPolicy2 : IPasswordPolicy
    {
        public bool IsValid(PasswordLine line)
        {
            // Bounds are the positions, and are one-based indexes.
            var index1 = line.LowerBound - 1;
            var index2 = line.UpperBound - 1;

            var chars = new[] { line.Password[index1], line.Password[index2] };

            return chars.Count(c => c == line.RequiredChar) == 1;
        }
    }
}
