using System;
using System.Collections.Generic;
using AoC;
using Crayon;

Console.OutputEncoding = System.Text.Encoding.Unicode;

static void PrintTitle()
{
    Console.Clear();
    Console.WriteLine("🎄 Advent of Code 2020 🎅");
}

PrintTitle();

var solverFactory = SolverFactory.CreateFactory();

bool exit;
var defaultDay = Math.Min(DateTime.Now.Day, 25).ToString();
var cliDays = new Queue<string>(args.Length > 0 ? args : new[] { "" });
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
