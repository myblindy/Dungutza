using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Text;

namespace Simulator.ViewModels
{
    class MainViewModel : ReactiveObject
    {
        readonly Logic logic;

        int counter;
        public int Counter { get => counter; set => this.RaiseAndSetIfChanged(ref counter, value); }

        Direction direction;
        public Direction Direction { get => direction; set => this.RaiseAndSetIfChanged(ref direction, value); }

        double frequency;
        public double Frequency { get => frequency; set => this.RaiseAndSetIfChanged(ref frequency, value); }

        public MainViewModel()
        {
            logic = new((counter, direction, frequency) => (Counter, Direction, Frequency) = (counter, direction, frequency));
        }
    }
}
