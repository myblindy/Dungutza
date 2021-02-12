using Avalonia.Threading;
using CommonLogic;
using LiveRpi.Models;
using MoreLinq;
using ReactiveUI;
using System;
using System.Device.Gpio;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LiveRpi.ViewModels
{
    class MainViewModel : ReactiveObject, IDisposable
    {
        public Logic Logic { get; }

        int counter;
        public int Counter { get => counter; set => this.RaiseAndSetIfChanged(ref counter, value); }

        Direction direction;
        public Direction Direction { get => direction; set => this.RaiseAndSetIfChanged(ref direction, value); }

        double frequency;
        public double Frequency { get => frequency; set => this.RaiseAndSetIfChanged(ref frequency, value); }

        bool receivedResult;
        private bool disposedValue;

        public bool ReceivedResult { get => receivedResult; set => this.RaiseAndSetIfChanged(ref receivedResult, value); }

        public InputModel[] Inputs { get; } = Logic.Pins.Zip(new[] { 2, 3, 4, 17, 27 }, (name, pinId) => (name, pinId))
            .Select(w => new InputModel { Text = w.name, PinId = w.pinId })
            .ToArray();

        readonly Dispatcher mainDispatcher;
        readonly Thread logicThread;
        readonly GpioController gpioController = new();
        readonly StreamWriter logStreamWriter = new StreamWriter(new FileStream("log.csv", FileMode.Append, FileAccess.Write, FileShare.ReadWrite, 1024 * 1024));
        bool requestStopThread;

        public MainViewModel()
        {
            mainDispatcher = Dispatcher.UIThread;

            Inputs.ForEach(i => gpioController.OpenPin(i.PinId, PinMode.Input));

            Logic = new((counter, direction, frequency) =>
                {
                    _ = mainDispatcher.InvokeAsync(() => (Counter, Direction, Frequency, ReceivedResult) = (counter, direction, frequency, true));
                    _ = logStreamWriter.WriteLineAsync($"{DateTime.Now},{counter},{direction},{frequency}");
                },
                pin => gpioController.Read(Inputs[pin].PinId) == PinValue.High);

            logicThread = new(() => { while (!requestStopThread) Logic.Step(); }) { Name = "Logic Thread", IsBackground = true };
            logicThread.Start();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // managed state
                }

                // unmanaged state
                requestStopThread = true;
                logicThread.Join();
                logStreamWriter.Dispose();

                disposedValue = true;
            }
        }

        ~MainViewModel()
        {
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
