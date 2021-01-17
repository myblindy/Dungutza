using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator.Models
{
    class InputModel : ReactiveObject, IInput
    {
        bool active;
        public bool Active { get =>active; set => this.RaiseAndSetIfChanged(ref active, value); }
    }
}
