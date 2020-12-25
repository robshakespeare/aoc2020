// .:*~*:._.:*~*:._.:*~*:._.:*~*:._.:*~*:._.:*~*:*~*:._.:*~*:._.:*~*:._.:*~*:._.:*~*:._.:*~*:*~*:._.:*~*:.
// .                                                                                                     .
// .       *         __  __                         _____ _          _     _                       _     .
// .      /.\       |  \/  |                       / ____| |        (_)   | |                     | |    .
// .     /..'\      | \  / | ___ _ __ _ __ _   _  | |    | |__  _ __ _ ___| |_ _ __ ___   __ _ ___| |    .
// .     /'.'\      | |\/| |/ _ \ '__| '__| | | | | |    | '_ \| '__| / __| __| '_ ` _ \ / _` / __| |    .
// .    /.''.'\     | |  | |  __/ |  | |  | |_| | | |____| | | | |  | \__ \ |_| | | | | | (_| \__ \_|    .
// .    /.'.'.\     |_|  |_|\___|_|  |_|   \__, |  \_____|_| |_|_|  |_|___/\__|_| |_| |_|\__,_|___(_)    .
// .   /'.''.'.\                            __/ |                                                        .
// .   ^^^[_]^^^                           |___/                                                         .
// .                                                                                                     .
// .:*~*:._.:*~*:._.:*~*:._.:*~*:._.:*~*:._.:*~*:*~*:._.:*~*:._.:*~*:._.:*~*:._.:*~*:._.:*~*:*~*:._.:*~*:.

using System;

namespace AoC.Day25
{
    public class Day25Solver : SolverBase<long, string>
    {
        public override string DayName => "Combo Breaker";

        /// <summary>
        /// What encryption key is the handshake trying to establish?
        /// </summary>
        protected override long SolvePart1Impl(string input)
        {
            var publicKeys = input.NormalizeLineEndings().Split(Environment.NewLine);
            var cardPublicKey = int.Parse(publicKeys[0]);
            var doorPublicKey = int.Parse(publicKeys[1]);

            var cardLoopSize = DetermineLoopSize(cardPublicKey);

            return TransformSubjectNumber(doorPublicKey, cardLoopSize);
        }

        public static int DetermineLoopSize(int publicKey)
        {
            const int subjectNumber = 7;
            var loopSize = 0;
            var value = 1;

            while (value != publicKey)
            {
                loopSize++;
                value *= subjectNumber;
                value %= 20201227;
            }

            return loopSize;
        }

        public static long TransformSubjectNumber(int subjectNumber, int loopSize)
        {
            long value = 1;

            for (var i = 0; i < loopSize; i++)
            {
                value *= subjectNumber;
                value %= 20201227;
            }

            return value;
        }

        private const string MerryChristmas = @"
.:*~*:._.:*~*:._.:*~*:._.:*~*:._.:*~*:._.:*~*:*~*:._.:*~*:._.:*~*:._.:*~*:._.:*~*:._.:*~*:*~*:._.:*~*:.
.                                                                                                     .
.       *         __  __                         _____ _          _     _                       _     .
.      /.\       |  \/  |                       / ____| |        (_)   | |                     | |    .
.     /..'\      | \  / | ___ _ __ _ __ _   _  | |    | |__  _ __ _ ___| |_ _ __ ___   __ _ ___| |    .
.     /'.'\      | |\/| |/ _ \ '__| '__| | | | | |    | '_ \| '__| / __| __| '_ ` _ \ / _` / __| |    .
.    /.''.'\     | |  | |  __/ |  | |  | |_| | | |____| | | | |  | \__ \ |_| | | | | | (_| \__ \_|    .
.    /.'.'.\     |_|  |_|\___|_|  |_|   \__, |  \_____|_| |_|_|  |_|___/\__|_| |_| |_|\__,_|___(_)    .
.   /'.''.'.\                            __/ |                                                        .
.   ^^^[_]^^^                           |___/                                                         .
.                                                                                                     .
.:*~*:._.:*~*:._.:*~*:._.:*~*:._.:*~*:._.:*~*:*~*:._.:*~*:._.:*~*:._.:*~*:._.:*~*:._.:*~*:*~*:._.:*~*:.
";

        protected override string SolvePart2Impl(string input) => $"Day 25, part 2, was free :){Environment.NewLine}{MerryChristmas}";
    }
}
