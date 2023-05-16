using Dwar.Repositorys;
using Dwar.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dwar.UI.Controllers
{
    public class MainWindowController
    {
        private IBotRepository _botRepository;
        private BotService _botService;
        public BindingList<Bot> Bots { get; set; }
        public Bot SelectedBot { get; set; }

        public MainWindowController(IBotRepository botRepository, BotService botService)
        {
            _botRepository = botRepository;
            _botService = botService;
            Bots = _botRepository.GetAll().ToBindingList();
            SelectedBot = Bots.First();
        }
        public void Refresh()
        {
            var bots = _botRepository.GetAll().ToList();
            foreach (var bot in bots)
            {
                if(Bots.Contains(bot) == false)
                    bots.Add(bot);
            }

            foreach (var bot in Bots)
            {
                if (bots.Contains(bot) == false)
                    Bots.Remove(bot);
            }
        }

        public async void StartAsync()
        {
            await _botService.StartAsync(SelectedBot);
        }

        public void Stop()
        {
            _botService.Stop();
        }
    }
}
