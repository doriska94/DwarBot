using Dwar.Repositorys;
using Dwar.Services;
using Dwar.UI.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Dwar.UI.Controllers;

public class ActionController : NotifyChanged
{
    private BindingList<ActionModel> _actions;
    private ActionModel? _selectedAction;
    private IActionRepository _actionRepository;
    private Mob? _selectedMob ;

    public IEnumerable<ActionModel> Actions { get => _actions; }
    public ActionModel? SelectedAction { get => _selectedAction; set { _selectedAction = value; OnPropertyChanged(); } }
    public BindingList<RequestType> RequestTypes { get; set; }
    public Mob? SelectedMob { get => _selectedMob; set => _selectedMob = value; }
    public Paramerter? SelectedParameter { get; set; }
    public ActionController(IActionRepository actionRepository)
    {
        _actions = actionRepository.GetAll().Select(x => ActionModel.Create(x)).ToBindingList();
        _actionRepository = actionRepository;
        RequestTypes = Enum.GetValues(typeof(RequestType)).Cast<RequestType>().ToBindingList();
    }

    public void RemoveSelectedTarget()
    {
        if(SelectedAction == null)
            return;
        if (_selectedMob == null)
            return;

        SelectedAction.Mobs.Remove(_selectedMob);
    }
    public void AddTarget(Target target)
    {
        if (target == null)
            return;

        if (SelectedAction == null)
            return;

        if (SelectedAction.Mobs.Any(x => x.Name == target.Name) == false)
        {
            SelectedAction.Mobs.Add(new Mob()
            {
                Order = SelectedAction.Mobs.Count + 1,
                Name = target.Name,
            });
        }
    }

    public void Create()
    {
        SelectedAction = new ActionModel();
        _actions.Add(SelectedAction);
        OnPropertyChanged(nameof(Actions));
    }

    public void Update()
    {
        if (SelectedAction == null)
            return;

        Action entity;

        if (SelectedAction.Id != Guid.Empty)
        {
            entity = _actionRepository.Get(SelectedAction.Id);
            entity = ToEntity(entity);
        }
        else
        {

            entity = _actionRepository.Create(SelectedAction.Key,
                                              SelectedAction.UiName,
                                              SelectedAction.RequestType,
                                              SelectedAction.Method!,
                                              SelectedAction.Option!);
            entity = ToEntity(entity);
        }
        entity.Paramerters = SelectedAction.Paramerters.ToList();

        _actionRepository.Update(entity);
    }

    public void AddParameters(Paramerter paramerter)
    {
        if (SelectedAction == null)
            return;

        SelectedAction.Paramerters.Add(paramerter);

        OnPropertyChanged(nameof(SelectedAction.Paramerters));
    }

    public void Remove()
    {
        if (SelectedAction == null)
            return;
        var deleteID = SelectedAction.Id;
        _actions.Remove(SelectedAction);

        if (deleteID == Guid.Empty)
            return;

        var entity = _actionRepository.Get(deleteID);
        _actionRepository.Delete(entity);
    }

    public void RemoveParameter()
    {
        if (SelectedAction == null || SelectedParameter == null) return;

        SelectedAction.Paramerters.Remove(SelectedParameter);

    }
    private Action ToEntity(Action entity)
    {
        if (SelectedAction == null)
            return entity;

        entity.Key = SelectedAction.Key;
        entity.RequestType = SelectedAction.RequestType;
        entity.Option = SelectedAction.Option;
        entity.Method = SelectedAction.Method;
        entity.UiName = SelectedAction.UiName;
        entity.Mobs = SelectedAction.Mobs;
        entity.WaitAfterExecute = SelectedAction.WaitAfterExecute;
        return entity;
    }
}
