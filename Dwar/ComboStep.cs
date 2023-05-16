using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dwar
{
    public class ComboStep
    {
        public int Order { get; set; }
        public int Delay { get;set; }
        public ComboStepType Type { get;set; }
        public ComboStep(int order,int delay, ComboStepType type)
        {
            Order = order;
            Delay = delay;
            Type = type;
        }
        public ComboStep()
        {

        }

        public override string ToString()
        {
            return Type.ToString();
        }

    }
}
