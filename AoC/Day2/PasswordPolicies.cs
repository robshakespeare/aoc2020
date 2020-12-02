using System.Collections.Generic;
using System.Linq;

namespace AoC.Day2
{
    public interface IPasswordPolicy
    {
        bool IsValid(PasswordLine passwordLine);

        IEnumerable<PasswordLine> GetValidLines(IEnumerable<PasswordLine> passwordLines) => passwordLines.Where(IsValid);
    }

    public class PasswordPolicy1 : IPasswordPolicy
    {
        public bool IsValid(PasswordLine passwordLine)
        {
            var (lowerBound, upperBound, requiredChar, password) = passwordLine;

            var requiredCharCount = password.Count(c => c == requiredChar);

            // Lower and Upper bound are both inclusive.
            return requiredCharCount >= lowerBound && requiredCharCount <= upperBound;
        }
    }

    public class PasswordPolicy2 : IPasswordPolicy
    {
        public bool IsValid(PasswordLine passwordLine)
        {
            var (lowerBound, upperBound, requiredChar, password) = passwordLine;

            // Bounds are the positions, and are one-based indexes.
            var index1 = lowerBound - 1;
            var index2 = upperBound - 1;

            var chars = new[] { password[index1], password[index2] };

            return chars.Count(c => c == requiredChar) == 1;
        }
    }
}
