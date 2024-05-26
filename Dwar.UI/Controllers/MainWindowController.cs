using Dwar.Repositorys;
using Dwar.Services;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Dwar.UI.Controllers;

public class MainWindowController
{
    public event System.Action Stoped = null!;
    private StopBotCommand? _stopBotCommand;
    private IBotRepository _botRepository;
    private BotService _botService;
    public BindingList<Bot> Bots { get; set; }
    public Bot? SelectedBot { get; set; }

    public MainWindowController(IBotRepository botRepository, BotService botService)
    {
        _botRepository = botRepository;
        _botService = botService;
        Bots = _botRepository.GetAll().ToBindingList();
        SelectedBot = Bots.FirstOrDefault();
    }
    public void Refresh()
    {
        var bots = _botRepository.GetAll().ToList();
        foreach (var bot in bots)
        {
            if(Bots.Contains(bot) == false)
                Bots.Add(bot);
        }

        foreach (var bot in Bots)
        {
            if (bots.Contains(bot) == false)
                Bots.Remove(bot);
        }
    }

    public async void StartAsync()
    {
        if (SelectedBot == null)
            return;
        if (_stopBotCommand != null)
            return;

        _stopBotCommand = new StopBotCommand();
        try
        {
            await _botService.StartAsync(SelectedBot, _stopBotCommand);
        }
        catch (TaskCanceledException)
        {

        }
        finally 
        { 
            _stopBotCommand = null;
            Stoped?.Invoke();
        }
    }

    public void Stop()
    {
        if (_stopBotCommand == null)
            return;
        _stopBotCommand.Stop = true;
        _botService.Stop();
        Stoped?.Invoke();
        _stopBotCommand = null;
    }
}
