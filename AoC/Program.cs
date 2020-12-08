using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crayon;

namespace AoC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.Unicode;

            static void PrintTitle()
            {
                Console.Clear();
                Console.WriteLine("🎄 Advent of Code 2020 🎅");
            }

            PrintTitle();

            var solverFactory = SolverFactory.CreateFactory<Program>();

            bool exit;
            var defaultDay = Math.Min(DateTime.Now.Day, 25).ToString();
            var cliDays = new Queue<string>(args.Any() ? args : new[] {""});
            do
            {
                Console.WriteLine($"Type day number or blank for {defaultDay} or 'x' to exit".Green());
                var dayNumber = cliDays.TryDequeue(out var cliDay) ? cliDay : Console.ReadLine() ?? "";
                dayNumber = string.IsNullOrWhiteSpace(dayNumber) ? defaultDay : dayNumber;

                exit = dayNumber == "x" || dayNumber == "exit";
                if (!exit)
                {
                    PrintTitle();
                    var solver = solverFactory.TryCreateSolver(dayNumber);
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
