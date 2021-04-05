using System;
using System.Diagnostics;
using System.Linq;

namespace CommonLogic
{
    public enum Direction { Up, Down }

    public class Logic
    {
        public static readonly string[] Pins = new[] { "Close Limit", "Open Limit", "Index", "Pulse", "Stop" };

        const int ClosePin = 5;
        const int OpenPin = 6;
        const int IndexPin = 13;
        const int PulsePin = 19;
        const int StopPin = 16;

        readonly Action<int /*counter*/, int /*left over counter*/, Direction, double /*freq in Hz*/> finished;
        readonly Func<int, bool> readPin;

        public Logic(Action<int, int, Direction, double> finished, Func<int, bool> readPin) =>
            (this.finished, this.readPin) = (finished, readPin);

        int step, pulseCounter, leftOverPulseCounter, cycleCounter;
        readonly Stopwatch cycleStart = Stopwatch.StartNew();
        Direction direction;

        public void Step()
        {
            void callFinished() { finished?.Invoke(pulseCounter, leftOverPulseCounter, direction, cycleCounter / cycleStart.Elapsed.TotalSeconds); (cycleCounter, pulseCounter, leftOverPulseCounter) = (0, 0, 0); cycleStart.Restart(); }

            switch (step)
            {
                case 0 when readPin(OpenPin): step = 1; direction = Direction.Down; break;
                case 0 when readPin(ClosePin): step = 1; direction = Direction.Up; break;
                case 1 when readPin(IndexPin): step = 2; break;
                case 2 when readPin(StopPin): step = 4; break;
                case 2 when readPin(PulsePin): step = 3; ++pulseCounter; break;
                case 3 when readPin(StopPin): step = 4; break;
                case 3 when !readPin(PulsePin): step = 2; break;
                case 4 when readPin(IndexPin): step = 0; callFinished(); break;
                case 4 when readPin(PulsePin): step = 5; ++leftOverPulseCounter; break;
                case 5 when readPin(IndexPin): step = 0; callFinished(); break;
                case 5 when !readPin(PulsePin): step = 4; break;
            }

            ++cycleCounter;
        }
    }
}