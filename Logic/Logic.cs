using System;
using System.Linq;

namespace CommonLogic
{
    public interface IInput
    {
        bool Active { get; }
    }

    public enum Direction { Up, Down }

    public class Logic<TInput> where TInput : IInput, new()
    {
        const int ClosePin = 0;
        const int OpenPin = 1;
        const int IndexPin = 2;
        const int PulsePin = 3;
        const int StopPin = 4;
        public TInput[] Inputs { get; } = Enumerable.Range(0, 5).Select(_ => new TInput()).ToArray();

        readonly Action<int /*counter*/, Direction, double /*freq in Hz*/> finished;
        readonly Func<int, bool> updatePin;

        public Logic(Action<int, Direction, double> finished, Func<int, bool> updatePin) =>
            (this.finished, this.updatePin) = (finished, updatePin);

        int step, counter, cycleCounter;
        DateTime cycleStart = DateTime.Now;
        Direction direction;

        public void Step()
        {
            void callFinished() { finished?.Invoke(counter, direction, cycleCounter / (DateTime.Now - cycleStart).TotalSeconds); (cycleStart, cycleCounter, counter) = (DateTime.Now, 0, 0); }

            switch (step)
            {
                case 0: if (updatePin(OpenPin)) { step = 1; direction = Direction.Up; } else if (updatePin(ClosePin)) { step = 1; direction = Direction.Down; } break;
                case 1: if (updatePin(IndexPin)) step = 2; break;
                case 2: if (updatePin(StopPin)) { step = 0; callFinished(); } else if (updatePin(PulsePin)) { step = 3; ++counter; } break;
                case 3: if (updatePin(StopPin)) { step = 0; callFinished(); } else if (!updatePin(PulsePin)) step = 2; break;
            }

            ++cycleCounter;

        }
    }
}