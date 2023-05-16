using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dwar.UI.Model
{
    public class ComboModel
    {
        public BindingList<ComboStep> ComboSteps { get; set; } = new();
        public bool FightInDefence { get; set; }

        public static ComboModel Create(Combo combo) 
        {
            return new ComboModel()
            {
                FightInDefence = combo.FightInDefence,
                ComboSteps = combo.ComboSteps.ToBindingList(),
            };
        }

    }
}
