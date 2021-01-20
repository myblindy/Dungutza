using CommonLogic;
using ReactiveUI;
using Simulator.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows.Threading;

namespace Simulator.ViewModels
{
    class MainViewModel : ReactiveObject
    {
        public Logic<InputModel> Logic { get; }

        int counter;
        public int Counter { get => counter; set => this.RaiseAndSetIfChanged(ref counter, value); }

        Direction direction;
        public Direction Direction { get => direction; set => this.RaiseAndSetIfChanged(ref direction, value); }

        double frequency;
        public double Frequency { get => frequency; set => this.RaiseAndSetIfChanged(ref frequency, value); }

        bool receivedResult;
        public bool ReceivedResult { get => receivedResult; set => this.RaiseAndSetIfChanged(ref receivedResult, value); }

        readonly Dispatcher mainDispatcher;
        readonly Thread logicThread;

        public MainViewModel()
        {
            mainDispatcher = Dispatcher.CurrentDispatcher;

            Logic = new((counter, direction, frequency) => mainDispatcher.BeginInvoke(() => (Counter, Direction, Frequency, ReceivedResult) = (counter, direction, frequency, true)),
                pin => Logic.Inputs[pin].Active);
            Logic.Inputs[0].Text = "Close Limit";
            Logic.Inputs[1].Text = "Open Limit";
            Logic.Inputs[2].Text = "Index";
            Logic.Inputs[3].Text = "Pulse";
            Logic.Inputs[4].Text = "Stop";

            logicThread = new(() => { while (true) Logic.Step(); }) { Name = "Logic Thread", IsBackground = true };
            logicThread.Start();
        }
    }
}
