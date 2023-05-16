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

        public BindingList<Bot> Bots { get; set; }
        public BindingList<Fight> Fights { get; set; }
        public BindingList<SequenceType> Sequences { get; set; }
        public Bot? SelectedBot { get => _selectedBot; set { _selectedBot = value; OnPropertyChanged(); } }

        public BotController(IBotRepository botRepository, IFightRepository fightRepository)
        {
            _botRepository = botRepository;
            _fightRepository = fightRepository;

            Bots = _botRepository.GetAll().ToBindingList();
            Fights = _fightRepository.GetAll().ToBindingList();
            SequenceType = Enum.GetValues(typeof(SequenceType)).Cast<SequenceType>().ToBindingList();
        }

        public void Create()
        {
            var bot = _botRepository.Create();
            Bots.Add(bot);
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
    }
}
