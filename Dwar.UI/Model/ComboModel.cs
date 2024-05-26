using System.ComponentModel;

namespace Dwar.UI.Model;

public class ComboModel
{
    public BindingList<ComboStep> ComboSteps { get; set; } = new();
    public bool FightInDefence { get; set; }
    public bool UseSkill { get; set; }

    public static ComboModel Create(Combo combo) 
    {
        return new ComboModel()
        {
            FightInDefence = combo.FightInDefence,
            UseSkill = combo.UseSkill,
            ComboSteps = combo.ComboSteps.ToBindingList(),
        };
    }

}
