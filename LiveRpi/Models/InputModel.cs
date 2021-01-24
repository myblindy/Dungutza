using CommonLogic;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveRpi.Models
{
    class InputModel : ReactiveObject
    {
        string? text;
        public string? Text { get => text; set => this.RaiseAndSetIfChanged(ref text, value); }

        public int PinId { get; set; }
    }
}
