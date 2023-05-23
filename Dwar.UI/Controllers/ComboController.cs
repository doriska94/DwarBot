using Dwar.Repositorys;
using Dwar.UI.Model;
using System;
using System.ComponentModel;
using System.Linq;

namespace Dwar.UI.Controllers;

public class ComboController : NotifyChanged
{
    private IComboRepository _comboRepository;
    private ComboModel _combo = null!;

    public ComboModel Combo { get => _combo; set { _combo = value; OnPropertyChanged(); } }
    public ComboStep? SelectedStep { get; set; }
    public BindingList<ComboStepType> StepTypes { get; set; }
    public ComboController(IComboRepository comboRepository)
    {
        _comboRepository = comboRepository;
        Combo = ComboModel.Create(_comboRepository.Get());
        StepTypes = Enum.GetValues(typeof(ComboStepType)).Cast<ComboStepType>().ToBindingList();
    }

    public void AddNewStep()
    {
        var comboStep = new ComboStep(Combo.ComboSteps.Count + 1, 1, ComboStepType.Up);
        Combo.ComboSteps.Add(comboStep);
        OnPropertyChanged(nameof(Combo.ComboSteps));
    }

    public void RemoveStep()
    {
        if (SelectedStep == null)
            return;
        Combo.ComboSteps.Remove(SelectedStep);
        OnPropertyChanged(nameof(Combo.ComboSteps));
    }

    public void Save()
    {
        _comboRepository.Set(ToCombo());
    }
    private Combo ToCombo()
    {
        var combo = new Combo(Combo.ComboSteps.ToList());
        combo.FightInDefence = Combo.FightInDefence;
        return combo;

    }
}
