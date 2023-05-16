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
        private Dictionary<Type, object> _services = new Dictionary<Type, object>();
        private List<IHandleFightState> handleFightStates = new List<IHandleFightState>();


        public void Configure()
        {
            _services.Add(typeof(Random), new Random(100000000));
            _services.Add(typeof(IActionRepository), new ActionRepository());
            _services.Add(typeof(IBitmapRepository), new BitMapRepository());
            _services.Add(typeof(IUserInput), new UserInput());

            _services.Add(typeof(IDomain), new Domain());
            _services.Add(typeof(ICookie), new CookieLocal(GetService<IDomain>()));
            _services.Add(typeof(IScreenshot), new ScreenshotRepository());

            var httpRequest = new HttpRequest(GetService<ICookie>(), GetService<IDomain>(), GetService<IActionRepository>());

            _services.Add(typeof(ISendRequest), httpRequest);
            _services.Add(typeof(IGetRequest), httpRequest);
            _services.Add(typeof(ITargetRepository), httpRequest);

            var start = new StartFightService(GetService<IBitmapRepository>(), GetService<IScreenshot>());
            _services.Add(typeof(StartFightService), start);
            handleFightStates.Add(start);

            var fight = new FightControlService(GetService<IBitmapRepository>(),
                                                                               GetService<StartFightService>(),
                                                                               GetService<IUserInput>());
            _services.Add(typeof(FightControlService), fight);
            _services.Add(typeof(IComboSetService), fight);
            handleFightStates.Add(fight);

            _services.Add(typeof(MouseService), new MouseService(GetService<IBitmapRepository>(), GetService<IScreenshot>()));

            _services.Add(typeof(HttpService), new HttpService(GetService<IActionRepository>(), 
                                                               GetService<ISendRequest>(), 
                                                               GetService<IGetRequest>(),
                                                               GetService<Random>()));

            var actionControl = new ActionFightService(GetService<HttpService>(),
                                                                        GetService<MouseService>(),
                                                                        GetService<StartFightService>(),
                                                                        GetService<FightControlService>());

            _services.Add(typeof(IActionService), actionControl);
            _services.Add(typeof(IActionSetService), actionControl);
            
        }
        public T GetService<T>() where T : class
        {
            return (T)_services[typeof(T)];
        }

        public IEnumerable<IHandleFightState> GetHandleFightStates() => handleFightStates;
    }
}
