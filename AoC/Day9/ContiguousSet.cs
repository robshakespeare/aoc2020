using System;

namespace AoC.Day9
{
    public class ContiguousSet
    {
        public long Total { get; private set; }
        public long Smallest { get; private set; }
        public long Largest { get; private set; }
        public long RangeSize => Smallest + Largest;

        public ContiguousSet(long num1, long num2)
        {
            Total = num1 + num2;
            Smallest = Math.Min(num1, num2);
            Largest = Math.Max(num1, num2);
        }

        public void Update(long num)
        {
            Total += num;
            Smallest = Math.Min(Smallest, num);
            Largest = Math.Max(Largest, num);
        }
    }
}
