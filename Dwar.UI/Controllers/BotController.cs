using Dwar.Repositorys;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Dwar.UI.Controllers
{
    public class BotController : NotifyChanged
    {
        private IBotRepository _botRepository;
        private IFightRepository _fightRepository;
        private Bot? _selectedBot;
        private Fight? _selectedFight;

        public BindingList<Bot> Bots { get; set; }
        public BindingList<Fight> Fights { get; set; }
        public BindingList<SequenceType> Sequences { get; set; }
        public Bot? SelectedBot { get => _selectedBot; set { _selectedBot = value;  SetSelectedFight(value); OnPropertyChanged(); } }
        public Fight? SelectedFight { get => _selectedFight; set { _selectedFight = value; SetFightId(value); OnPropertyChanged(); } }

        public BotController(IBotRepository botRepository, IFightRepository fightRepository)
        {
            _botRepository = botRepository;
            _fightRepository = fightRepository;

            Bots = _botRepository.GetAll().ToBindingList();

            Fights = _fightRepository.GetAll().ToBindingList();

            Bots.ToList().ForEach(x => x.Fight = _fightRepository.Get(x.FightId));

            Sequences = Enum.GetValues(typeof(SequenceType)).Cast<SequenceType>().ToBindingList();
        }

        public void Create()
        {
            var bot = _botRepository.Create();
            Bots.Add(bot);
            SelectedBot = bot;
            OnPropertyChanged(nameof(SelectedBot));
        }
        public void Update()
        {
            if (SelectedBot == null)
                return;

            _botRepository.Save(SelectedBot);
        }
        public void Remove()
        {
            if (SelectedBot == null)
                return;
            _botRepository.Delete(SelectedBot);
            Bots.Remove(SelectedBot);
        }
        private void SetFightId(Fight? fight)
        {
            if(fight == null) return;
            if(SelectedBot== null) return;

            SelectedBot.FightId = fight.Id;
        }
        private void SetSelectedFight(Bot? value)
        {
            if (value == null || value.FightId == Guid.Empty)
            {
                _selectedFight = null!;
                return;
            }
            _selectedFight = _fightRepository.Get(value.FightId);
        }
    }
}
