using ReactiveUI;
using Simulator.Models;
using System;
using System.Collections.Generic;
using System.Text;

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

        public MainViewModel()
        {
            Logic = new((counter, direction, frequency) => (Counter, Direction, Frequency) = (counter, direction, frequency));
            Logic.Inputs[0].Text = "Close Limit";
            Logic.Inputs[1].Text = "Open Limit";
            Logic.Inputs[2].Text = "Index";
            Logic.Inputs[3].Text = "Pulse";
            Logic.Inputs[4].Text = "Stop";

        }
    }
}
