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
            var requiredCharCount = passwordLine.Password.Count(c => c == passwordLine.RequiredChar);

            // Lower and Upper bound are both inclusive.
            return requiredCharCount >= passwordLine.LowerBound && requiredCharCount <= passwordLine.UpperBound;
        }
    }

    public class PasswordPolicy2 : IPasswordPolicy
    {
        public bool IsValid(PasswordLine passwordLine)
        {
            // Bounds are the positions, and are one-based indexes.
            var index1 = passwordLine.LowerBound - 1;
            var index2 = passwordLine.UpperBound - 1;

            var chars = new[] { passwordLine.Password[index1], passwordLine.Password[index2] };

            return chars.Count(c => c == passwordLine.RequiredChar) == 1;
        }
    }
}
