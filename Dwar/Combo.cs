using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dwar
{
    public class Combo
    {
        private int _step;
        public List<ComboStep> ComboSteps { get; set; } = new();
        public bool FightInDefence { get;set; }
        public Combo(List<ComboStep> comboSteps)
        {
            ComboSteps = comboSteps;
        }
        public Combo()
        {

        }
        public ComboStep GetNext()
        {
            var nextStep = ComboSteps[_step];
            _step++;
            _step %= ComboSteps.Count;
            return nextStep;
        }
    }
}
