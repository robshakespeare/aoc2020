using System;
using System.Linq;

namespace AoC
{
    public static class Program
    {
        public static void Main() => RunDay(3);

        private static void RunDay(int dayNumber)
        {
            var solverType = typeof(Program).Assembly.GetTypes()
                .Where(t => t.IsClass && t.GetInterfaces().Any(it => it == typeof(ISolver)))
                .Single(t => t.FullName?.Contains($"Day{dayNumber}") == true);

            var solver = (ISolver)Activator.CreateInstance(solverType)!;

            solver.Run();
        }
    }
}
