using System;
using System.Linq;

public interface IInput
{
    bool Active { get; }
}

public enum Direction { Up, Down }

public class Logic<TInput> where TInput : IInput, new()
{
    public TInput[] Inputs { get; } = Enumerable.Range(0, 5).Select(_ => new TInput()).ToArray();
    readonly Action<int /*counter*/, Direction, double /*freq in Hz*/> finished;

    public Logic(Action<int, Direction, double> finished)
    {
        this.finished = finished;
    }
}
