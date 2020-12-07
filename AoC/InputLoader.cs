using System;
using System.IO;
using Crayon;

namespace AoC
{
    public class InputLoader
    {
        private readonly int _dayNumber;
        private readonly Lazy<string> _part1;
        private readonly Lazy<string> _part2;

        public string PuzzleInputPart1 => _part1.Value;

        public string PuzzleInputPart2 => _part2.Value;

        public InputLoader(int dayNumber)
        {
            _dayNumber = dayNumber;
            _part1 = new Lazy<string>(() => LoadInput(GetInputFilePath("input.txt")));
            _part2 = new Lazy<string>(() =>
            {
                var part2FilePath = GetInputFilePath("input-part-2.txt");

                if (!File.Exists(part2FilePath))
                {
                    return _part1.Value;
                }

                Console.WriteLine("Part 2 has separate input file".Blue());
                return LoadInput(part2FilePath);
            });
        }

        private string GetInputFilePath(string fileName) => Path.Combine($"Day{_dayNumber}", fileName);

        private static string LoadInput(string filePath)
        {
            var input = File.ReadAllText(filePath);

            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine($"[WARNING] Input file `{filePath.BrightCyan()}` is empty".BrightRed());
            }

            return input;
        }
    }
}
