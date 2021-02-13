using CommonLogic;
using ReactiveUI;
using Simulator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Threading;

namespace Simulator.ViewModels
{
    class MainViewModel : ReactiveObject
    {
        public Logic Logic { get; }

        int counter;
        public int Counter { get => counter; set => this.RaiseAndSetIfChanged(ref counter, value); }

        int leftOverCounter;
        public int LeftOverCounter { get => leftOverCounter; set => this.RaiseAndSetIfChanged(ref leftOverCounter, value); }

        Direction direction;
        public Direction Direction { get => direction; set => this.RaiseAndSetIfChanged(ref direction, value); }

        double frequency;
        public double Frequency { get => frequency; set => this.RaiseAndSetIfChanged(ref frequency, value); }

        bool receivedResult;
        public bool ReceivedResult { get => receivedResult; set => this.RaiseAndSetIfChanged(ref receivedResult, value); }

        public InputModel[] Inputs { get; } = Logic.Pins.Select(name => new InputModel { Text = name }).ToArray();

        readonly Dispatcher mainDispatcher;
        readonly Thread logicThread;

        public MainViewModel()
        {
            mainDispatcher = Dispatcher.CurrentDispatcher;

            Logic = new((counter, leftOverCounter, direction, frequency) => mainDispatcher.BeginInvoke(() => (Counter, LeftOverCounter, Direction, Frequency, ReceivedResult) = (counter, leftOverCounter, direction, frequency, true)),
                pin => Inputs[pin].Active);

            logicThread = new(() => { while (true) Logic.Step(); }) { Name = "Logic Thread", IsBackground = true };
            logicThread.Start();
        }
    }
}
