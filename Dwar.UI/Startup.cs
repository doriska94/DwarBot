using Dwar.FileIO;
using Dwar.Http;
using Dwar.Repositorys;
using Dwar.Services;
using Dwar.UI.WindowsRepositries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dwar.UI
{
    public class Startup
    {
        private Dictionary<Type, object> _services = new();
        private List<IHandleFightState> _handleFightStates = new();


        public void Configure()
        {
            _services.Add(typeof(Random), new Random(100000000));
            _services.Add(typeof(IActionRepository), new ActionRepository());
            _services.Add(typeof(IFightRepository), new FightRepository());
            _services.Add(typeof(IBitmapRepository), new BitMapRepository());
            _services.Add(typeof(IUserInput), new UserInput());
            _services.Add(typeof(IDomainRepositry),new DomainRepositry());
            _services.Add(typeof(IComboRepository), new ComboRepository());
            _services.Add(typeof(IBotRepository), new BotRepository());

            _services.Add(typeof(IDomain), GetService<IDomainRepositry>().Get());
            _services.Add(typeof(Domain), GetService<IDomainRepositry>().Get());

            _services.Add(typeof(ICookie), new CookieLocal(GetService<IDomain>()));
            _services.Add(typeof(IScreenshot), new ScreenshotRepository());

            var httpRequest = new HttpRequest(GetService<ICookie>(), GetService<IDomain>(), GetService<IActionRepository>());

            _services.Add(typeof(ISendRequest), httpRequest);
            _services.Add(typeof(IGetRequest), httpRequest);
            _services.Add(typeof(ITargetRepository), httpRequest);

            var start = new StartFightService(GetService<IBitmapRepository>(), GetService<IScreenshot>());
            _services.Add(typeof(StartFightService), start);
            
            var fight = new FightControlService(GetService<StartFightService>(),
                                                GetService<IUserInput>(),
                                                GetService<IComboRepository>());

            _services.Add(typeof(FightControlService), fight);
            _handleFightStates.Add(fight);

            _services.Add(typeof(RefreshService), new RefreshService());

            _services.Add(typeof(HttpService), new HttpService(GetService<IActionRepository>(), 
                                                               GetService<ISendRequest>(), 
                                                               GetService<IGetRequest>(),
                                                               GetService<Random>()));

            var fightAction = new FightService(GetService<HttpService>(),
                                                                        GetService<RefreshService>(),
                                                                        GetService<StartFightService>(),
                                                                        GetService<FightControlService>(),
                                                                        GetService<IActionRepository>());

            _services.Add(typeof(FightService), fightAction);
            var farmAction = new FarmService(
                                             GetService<RefreshService>(),
                                             GetService<IActionRepository>(),
                                             GetService<HttpService>());
            _services.Add(typeof(FarmService), farmAction);

            _services.Add(typeof(BotService), new BotService(GetService<FarmService>(),
                                                             GetService<HpService>(),
                                                             GetService<FightService>(),
                                                             GetService<IFightRepository>()));
        }
        public T GetService<T>() where T : class
        {
            return (T)_services[typeof(T)];
        }

        public IEnumerable<IHandleFightState> GetHandleFightStates() => _handleFightStates;
    }
}
