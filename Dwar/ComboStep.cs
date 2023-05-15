using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dwar
{
    public class ComboStep
    {
        public int Delay { get;set; }
        public ComboStepType Type { get;set; }
        public ComboStep(int delay, ComboStepType type)
        {
            Delay = delay;
            Type = type;
        }

             

    }
}
