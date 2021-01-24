using Avalonia.Threading;
using CommonLogic;
using LiveRpi.Models;
using ReactiveUI;
using System.Device.Gpio;
using System.Linq;
using System.Threading;

namespace LiveRpi.ViewModels
{
    class MainViewModel : ReactiveObject
    {
        public Logic Logic { get; }

        int counter;
        public int Counter { get => counter; set => this.RaiseAndSetIfChanged(ref counter, value); }

        Direction direction;
        public Direction Direction { get => direction; set => this.RaiseAndSetIfChanged(ref direction, value); }

        double frequency;
        public double Frequency { get => frequency; set => this.RaiseAndSetIfChanged(ref frequency, value); }

        bool receivedResult;
        public bool ReceivedResult { get => receivedResult; set => this.RaiseAndSetIfChanged(ref receivedResult, value); }

        public InputModel[] Inputs { get; } = Logic.Pins.Zip(new[] { 5, 6, 7, 8, 9 }, (name, pinId) => (name, pinId))
            .Select(w => new InputModel { Text = w.name, PinId = w.pinId })
            .ToArray();

        readonly Dispatcher mainDispatcher;
        readonly Thread logicThread;
        readonly GpioController gpioController = new();

        public MainViewModel()
        {
            mainDispatcher = Dispatcher.UIThread;

            Logic = new((counter, direction, frequency) => mainDispatcher.InvokeAsync(() => (Counter, Direction, Frequency, ReceivedResult) = (counter, direction, frequency, true)),
                pin => gpioController.Read(Inputs[pin].PinId) == PinValue.High);

            logicThread = new(() => { while (true) Logic.Step(); }) { Name = "Logic Thread", IsBackground = true };
            logicThread.Start();
        }
    }
}
