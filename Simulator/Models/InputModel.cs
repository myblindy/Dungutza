using CommonLogic;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator.Models
{
    class InputModel : ReactiveObject
    {
        bool active;
        public bool Active { get => active; set => this.RaiseAndSetIfChanged(ref active, value); }

        string text;
        public string Text { get => text; set => this.RaiseAndSetIfChanged(ref text, value); }
    }
}
