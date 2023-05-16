using Dwar.Repositorys;
using Dwar.Services;
using Dwar.UI.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dwar.UI.Controllers
{
    public class ActionController : NotifyChanged
    {
        private BindingList<ActionModel> _actions;
        private ActionModel? _selectedAction;
        private IActionRepository _actionRepository;
        private ITargetRepository _targetRepository;
        private Target selectedTarget;

        public IEnumerable<ActionModel> Actions { get => _actions; }
        public ActionModel? SelectedAction { get => _selectedAction; set { _selectedAction = value; OnPropertyChanged(); } }
        public BindingList<RequestType> RequestTypes { get; set; }
        public BindingList<Target> Targets { get; set; }
        public Target SelectedTarget { get => selectedTarget; set { selectedTarget = value; OnPropertyChanged(); SetTarget(value); } }
        public Paramerter? SelectedParameter { get; set; }
        public ActionController(IActionRepository actionRepository, ITargetRepository targetRepository)
        {
            _actions = actionRepository.GetAll().Select(x => ActionModel.Create(x)).ToBindingList();
            _actionRepository = actionRepository;
            _targetRepository = targetRepository;
            LoadTargets();
            RequestTypes = Enum.GetValues(typeof(RequestType)).Cast<RequestType>().ToBindingList();
        }

        private async void LoadTargets()
        {
            var targets = await _targetRepository.GetTargetsAsync();
            Targets = targets.ToBindingList();
            OnPropertyChanged(nameof(Targets));
        }

        private void SetTarget(Target target)
        {
            if (target == null)
                return;
            if (SelectedAction == null)
                return;
            SelectedAction.MobName = target.Name;
        }
        public void SetAction(ActionModel? action)
        {
            if (action == null)
                return;

            selectedTarget = Targets.ToList().FirstOrDefault(x => x.Name == action.MobName)!;
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
            entity.MobName = SelectedAction.MobName;
            entity.WaitAfterExecute = SelectedAction.WaitAfterExecute;
            return entity;
        }
    }
}
