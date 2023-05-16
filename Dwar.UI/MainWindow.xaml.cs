using Dwar.Http;
using Dwar.Repositorys;
using Dwar.Services;
using HtmlAgilityPack;
using Microsoft.Web.WebView2.Core;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Linq;
using System.Collections.Generic;
using System;

namespace Dwar.UI
{
    public partial class MainWindow : Window
    {
        private Startup startup = new Startup();
        private List<IHandleFightState> _handleFightStates = new();
        public MainWindow()
        {
            InitializeComponent();
            

        }

        private async void OnTestClick(object sender, RoutedEventArgs e)
        {
            var fight = startup.GetService<IComboSetService>();
            fight.SetCombo(new Combo(new List<ComboStep>()
            {
                new ComboStep(2,ComboStepType.Forward),
                new ComboStep(2,ComboStepType.Up),
                new ComboStep(2,ComboStepType.Up),
                new ComboStep(2,ComboStepType.Down)

            }));
            var actionRepository = startup.GetService<IActionRepository>();
            //action_run.php?code=ATTACK_BOT&url_success=fight.php?522331141&url_error=hunt.php&bot_id=3188&chk=test
            //bot_id = 3188
            //random

            //var action = actionRepository.Create("attack_skelet", "attack_skelet", RequestType.Get, "/action_run.php",
            //    "code=ATTACK_BOT&url_success=fight.php?301531141&url_error=hunt.php&bot_id=31888&chk=test");
            //var param = new Paramerter("bot_id", "3188", false, null!);
            //var paramRand = new Paramerter("url_success", "fight.php?", true, startup.GetService<Random>());
            //action.Paramerters.Add(param);
            //action.Paramerters.Add(paramRand);
            //actionRepository.Update(action);

            var action = actionRepository.Get(Guid.Parse("4a3893fe-e69b-4d57-b0a5-2b8d1ca9d728"));

            var actionSetService = startup.GetService<IActionSetService>();

            actionSetService.SetAttack(Fight.FightFactory.Create(Guid.NewGuid(),"Seleton", action, new List<Action>()));

            var actionfightService = startup.GetService<IActionService>();
            await actionfightService.ExecuteAsync();
        }

        private void OnOpenBotClick(object sender, RoutedEventArgs e)
        {
          
        }

        private void CoreWebView2_WebResourceRequested(object sender, CoreWebView2WebResourceRequestedEventArgs e)
        {
            var coockie = startup.GetService<ICookie>();
            var headers = e.Request.Headers;

            _handleFightStates.ForEach(x => x.HandleRequest(e.Request.Uri));

            
            foreach (var header in headers)
            {
                if (header.Key.ToLower() == "cookie")
                {
                    coockie.Set(header.Value);
                }
            }

        }

        private void OnContentLoadingAsync(object sender, Microsoft.Web.WebView2.Core.CoreWebView2ContentLoadingEventArgs e)
        {
            string filter = "*";

            _webView.CoreWebView2.AddWebResourceRequestedFilter(filter,
                                                       CoreWebView2WebResourceContext.All);
            _webView.CoreWebView2.WebResourceRequested += CoreWebView2_WebResourceRequested!;
            
        }


        private void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            startup.Configure();
            _handleFightStates = startup.GetHandleFightStates().ToList();
            _webView.ContentLoading += OnContentLoadingAsync!;

            var domain = startup.GetService<IDomain>();

            domain.SetUrl("https://dwarlegacy.ru/");

            var mouseService = startup.GetService<RefreshService>();
            mouseService.Refresh += OnRefresh;

        }

        private void OnRefresh()
        {
            _webView.Source = _webView.Source;
        }
    }
}
