using Dwar.Repositorys;
using Dwar.UI.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dwar.UI.Controllers
{
    public class FightController : NotifyChanged
    {
        private readonly BindingList<FightModel> _fights;

        public IEnumerable<FightModel> Fights => _fights;
        public FightModel? SelectedFight { get => _selectedFight; set { _selectedFight = value; OnPropertyChanged(); } }
        public BindingList<Action> AttackActions { get; set; }
        public Action? SelectedStartUpAction { get; set; }


        private IFightRepository _repository;
        private IActionRepository _actionRepository;
        private FightModel? _selectedFight;

        public FightController(IFightRepository repository, IActionRepository actionRepository)
        {
            _repository = repository;
            _actionRepository = actionRepository;

            AttackActions = _actionRepository.GetAll().ToBindingList();

            _fights = _repository.GetAll().Select(x => Convert(x)).ToBindingList();
        }

        private FightModel Convert(Fight fight)
        {
            var fightModel = FightModel.Create(fight);
            fightModel.Attack = AttackActions.FirstOrDefault(x=> x.Id == fight.AttackId)!;
            fightModel.Attack = AttackActions.FirstOrDefault(x=> x.Id == fight.AttackId)!;
            fightModel.StartUpActions = _actionRepository.GetAll(fight.StartUpActions).ToBindingList();
            return fightModel;
        }
        public void Create()
        {
            SelectedFight = new FightModel();
            _fights.Add(SelectedFight);
        }

        public void Update()
        {
            if (SelectedFight == null)
                return;

            Fight fight;

            if (SelectedFight.Id == Guid.Empty)
            {
                fight = _repository.Create(SelectedFight.Name, SelectedFight.Attack.Id, SelectedFight.StartUpActions.Select(x => x.Id));
            }
            else
            {
                fight = Fight.FightFactory.Create(SelectedFight.Id, SelectedFight.Name, SelectedFight.Attack, SelectedFight.StartUpActions);
            }

            _repository.Save(fight);

        }

        public void Delete()
        {
            if (SelectedFight == null)
                return;
            if (SelectedFight.Id == Guid.Empty)
            {
                _fights.Remove(SelectedFight);
                return;
            }

            _repository.Delete(SelectedFight.Id);
            _fights.Remove(SelectedFight);
        }
        public void AddStartUpActions(Action action)
        {
            if (SelectedFight == null)
                return;
            SelectedFight.StartUpActions.Add(action);
        }
        public void RemoveStartUpActions()
        {
            if (SelectedFight == null || SelectedStartUpAction == null)
                return;

            SelectedFight.StartUpActions.Remove(SelectedStartUpAction);
            OnPropertyChanged(nameof(SelectedFight.StartUpActions));
        }

    }
}
