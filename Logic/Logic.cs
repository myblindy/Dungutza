using System;

public interface IInput
{
    bool Active { get; }
}

public enum Direction { Up, Down }

public class Logic<TInput>where TInput:IInput
{
    public TInput[] Inputs { get; } = new TInput[5];
    readonly Action<int /*counter*/, Direction, double /*freq in Hz*/> finished;

    public Logic(Action<int, Direction, double> finished)
    {
        this.finished = finished;
    }
}
