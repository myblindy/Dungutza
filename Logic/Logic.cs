using System;

public interface IInput
{
    bool Active { get; set; }
}

public enum Direction { Up, Down }

public class Logic
{
    readonly IInput[] inputs;
    readonly Action<int /*counter*/, Direction, double /*freq in Hz*/> finished;

    public Logic(Action<int, Direction, double> finished)
    {
        this.finished = finished;
    }
}
