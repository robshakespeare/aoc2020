using System;
using Crayon;

namespace AoC
{
    public class Program
    {
        public static void Main()
        {
            Console.WriteLine("Advent of Code 2020!");

            var solverFactory = SolverFactory.CreateFactory<Program>();

            bool exit;
            do
            {
                Console.WriteLine("Type day number or blank to exit".Green());
                var dayNumber = Console.ReadLine() ?? "";

                exit = dayNumber == "" || dayNumber == "exit";

                if (!exit)
                {
                    var solver = solverFactory.CreateSolver(dayNumber);
                    if (solver != null)
                    {
                        Console.Clear();
                        solver.Run();
                    }
                    else
                    {
                        Console.WriteLine($"No solver for day '{dayNumber.Blue()}'.");
                    }
                }
            } while (!exit);
        }
    }
}
