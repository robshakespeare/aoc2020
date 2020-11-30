using System;
using System.Diagnostics;
using Crayon;

namespace AoC
{
    public class TimingBlock : IDisposable
    {
        private readonly string _name;
        private readonly Stopwatch _stopwatch;

        public TimingBlock(string name)
        {
            _name = name;
            _stopwatch = Stopwatch.StartNew();
        }

        public void Stop() => _stopwatch.Stop();

        public void Dispose()
        {
            Stop();

            Console.WriteLine($"[{_name}] time taken (seconds): {_stopwatch.Elapsed.TotalSeconds:0.000}".Dim());
            Console.WriteLine();
        }
    }
}
