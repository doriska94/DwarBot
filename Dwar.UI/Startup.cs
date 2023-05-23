using Dwar.FileIO;
using Dwar.Http;
using Dwar.Repositorys;
using Dwar.Services;
using Dwar.UI.WindowsRepositries;
using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace Dwar.UI;

public class Startup
{
    private Dictionary<Type, object> _services = new();
    private List<IHandleFightState> _handleFightStates = new();


    public void Configure(TextBlock textBlock)
    {
        
        _services.Add(typeof(ITimeOutRepository), new TimeOutRepository());
        _services.Add(typeof(ITimeOutService), GetService<ITimeOutRepository>().Get());
        _services.Add(typeof(INotifyerService), new Notifyer(textBlock));
        _services.Add(typeof(Random), new Random(100000000));
        _services.Add(typeof(IActionRepository), new ActionRepository());
        _services.Add(typeof(ILogService), new Logger(false));
        _services.Add(typeof(IFightRepository), new FightRepository());
        _services.Add(typeof(IBitmapRepository), new BitMapRepository());
        _services.Add(typeof(IUserInputService), new UserInput());
        _services.Add(typeof(IDomainRepositry),new DomainRepositry());
        _services.Add(typeof(IComboRepository), new ComboRepository());
        _services.Add(typeof(IBotRepository), new BotRepository());


        _services.Add(typeof(IDomain), GetService<IDomainRepositry>().Get());
        _services.Add(typeof(Domain), GetService<IDomainRepositry>().Get());

        _services.Add(typeof(ICookie), new CookieLocal(GetService<IDomain>()));
        _services.Add(typeof(IScreenshot), new ScreenshotRepository());

        var httpRequest = new HttpRequest(GetService<ICookie>(), GetService<IDomain>(), GetService<IActionRepository>(),GetService<ILogService>());

        _services.Add(typeof(IDivnoStekloService), new GameService(httpRequest, GetService<IActionRepository>(),GetService<IDomain>()));

        var actionExecuteService = new ActionExecuteService(GetService<IDivnoStekloService>());

        _services.Add(typeof(ActionExecuteService), actionExecuteService);

        _services.Add(typeof(ISendRequestService), httpRequest);
        _services.Add(typeof(IGetRequest), httpRequest);
        _services.Add(typeof(ITargetRepository), httpRequest);
        _services.Add(typeof(IHpRepository), new HpRepository(httpRequest,GetService<IDomain>()));

        var start = new StartFightService(GetService<IBitmapRepository>(), 
                                         GetService<IScreenshot>(),
                                         GetService<INotifyerService>(),
                                         GetService<ILogService>(),
                                         GetService<ITimeOutRepository>());

        _services.Add(typeof(StartFightService), start);
        _handleFightStates.Add(start);

        var fight = new FightControlService(GetService<StartFightService>(),
                                            GetService<IUserInputService>(),
                                            GetService<IComboRepository>(), 
                                            GetService<ILogService>(),
                                            GetService<INotifyerService>(),
                                            GetService<ITimeOutRepository>());

        _services.Add(typeof(FightControlService), fight);

        _services.Add(typeof(RefreshService), new RefreshService());
        _services.Add(typeof(HpService), new HpService(GetService<IHpRepository>()));

        _services.Add(typeof(HttpService), new HttpService(GetService<IActionRepository>(), 
                                                           GetService<ISendRequestService>(), 
                                                           GetService<IGetRequest>(),
                                                           GetService<Random>(),
                                                           GetService<ITargetRepository>()));

        var fightAction = new FightService(GetService<HttpService>(),
                                                                    GetService<RefreshService>(),
                                                                    GetService<StartFightService>(),
                                                                    GetService<FightControlService>(),
                                                                    GetService<IActionRepository>(),
                                                                    GetService<ILogService>(),
                                                                    GetService<INotifyerService>(),
                                                                    GetService<ITimeOutRepository>()
                                                                    );

        _services.Add(typeof(FightService), fightAction);

        _services.Add(typeof(FarmEndService), new FarmEndService(GetService<StartFightService>()));

        var farmAction = new FarmService(
                                         GetService<RefreshService>(),
                                         GetService<IActionRepository>(),
                                         GetService<HttpService>(),
                                         GetService<FarmEndService>(),
                                         GetService<StartFightService>(),
                                         GetService<FightControlService>());

        _handleFightStates.Add(GetService<FarmEndService>());

        _services.Add(typeof(FarmService), farmAction);

        _services.Add(typeof(StandartExecuteService), new StandartExecuteService(GetService<HttpService>(), GetService<IActionRepository>()));

        _services.Add(typeof(BotService), new BotService(GetService<FarmService>(),
                                                         GetService<HpService>(),
                                                         GetService<FightService>(),
                                                         GetService<IFightRepository>(),
                                                         GetService<INotifyerService>(), 
                                                         GetService<ILogService>(),
                                                         actionExecuteService,
                                                         GetService<HttpService>(),
                                                         GetService<IActionRepository>(),
                                                         GetService<StandartExecuteService>()));
    }
    public T GetService<T>() where T : class
    {
        return (T)_services[typeof(T)];
    }

    public IEnumerable<IHandleFightState> GetHandleFightStates() => _handleFightStates;
}
