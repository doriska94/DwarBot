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



        public void Configure()
        {
            _services.Add(typeof(IActionRepository), new ActionRepository());
            _services.Add(typeof(IBitmapRepository), new BitMapRepository());
            _services.Add(typeof(IUserInput), new UserInput());

            _services.Add(typeof(ICookie), new CookieLocal());
            _services.Add(typeof(IDomain), new Domain());
            _services.Add(typeof(IScreenshot), new ScreenshotRepository());

            var httpRequest = new HttpRequest(GetService<ICookie>(), GetService<IDomain>(), GetService<IActionRepository>());

            _services.Add(typeof(ISendRequest), httpRequest);
            _services.Add(typeof(IGetRequest), httpRequest);
            _services.Add(typeof(ITargetRepository), httpRequest);


            _services.Add(typeof(StartFightService), new StartFightService(GetService<IBitmapRepository>(),GetService<IScreenshot>()));

            _services.Add(typeof(FightControlService), new FightControlService(GetService<IBitmapRepository>(), 
                                                                               GetService<IScreenshot>(),
                                                                               GetService<StartFightService>(),
                                                                               GetService<IUserInput>()));

            _services.Add(typeof(MouseService), new MouseService(GetService<IBitmapRepository>(), GetService<IScreenshot>()));

            _services.Add(typeof(HttpService), new HttpService(GetService<IActionRepository>(), 
                                                               GetService<ISendRequest>(), 
                                                               GetService<IGetRequest>()));

            _services.Add(typeof(IActionService), new ActionFightService(GetService<HttpService>(),
                                                                        GetService<MouseService>(),
                                                                        GetService<StartFightService>(),
                                                                        GetService<FightControlService>()));

        }
        public T GetService<T>() where T : class
        {
            return (T)_services[typeof(T)];
        }
    }
}
