using System;
using System.Text;
using Crayon;

namespace AoC
{
    public class Program
    {
        public static void Main()
        {
            Console.Clear();
            Console.OutputEncoding = Encoding.Unicode;
            Console.WriteLine("🎄 Advent of Code 2020 🎅");

            var solverFactory = SolverFactory.CreateFactory<Program>();

            bool exit;
            var defaultDay = DateTime.Now.Day.ToString();
            do
            {
                Console.WriteLine($"Type day number or blank for {defaultDay} or 'x' to exit".Green());
                var dayNumber = Console.ReadLine() ?? "";
                dayNumber = string.IsNullOrWhiteSpace(dayNumber) ? defaultDay : dayNumber;

                exit = dayNumber == "x" || dayNumber == "exit";
                if (!exit)
                {
                    Console.Clear();
                    var solver = solverFactory.CreateSolver(dayNumber);
                    if (solver != null)
                    {
                        solver.Run();
                    }
                    else
                    {
                        Console.WriteLine($"No solver for day '{dayNumber.BrightCyan()}'.".Red());
                    }
                }
            } while (!exit);
        }
    }
}
