using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zelda.EventHandlers
{
    class InputEventArgs : EventArgs
    {
        public Input Input { get; set; }

        public InputEventArgs(Input input)
        {
            this.Input = input;
        }
    }
}
