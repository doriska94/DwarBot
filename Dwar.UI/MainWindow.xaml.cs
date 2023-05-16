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
using Dwar.UI.View;
using System.Windows.Controls;
using Dwar.UI.Controllers;
using System.Security.Principal;
using System.Windows.Media;
using Microsoft.Web.WebView2.Wpf;

namespace Dwar.UI;

public partial class MainWindow : Window
{
    private Startup _startup = new();
    private List<IHandleFightState> _handleFightStates = new();
    private MainWindowController _windowController;
    private WebView2 _serviceHandling;
    public MainWindow()
    {
        _startup.Configure();
        InitializeComponent();
        _windowController = new MainWindowController(_startup.GetService<IBotRepository>(),
                                                     _startup.GetService<BotService>());
        DataContext= _windowController;
    }

    private void OnTestClick(object sender, RoutedEventArgs e)
    {
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

        var actionRepository = _startup.GetService<IActionRepository>();
        var action = actionRepository.Get(Guid.Parse("4a3893fe-e69b-4d57-b0a5-2b8d1ca9d728"));

        var actionSetService = _startup.GetService<IActionSetService>();
        var fightrepository = _startup.GetService<IFightRepository>();
        actionSetService.SetAttack(fightrepository.Create("Skeleton", action.Id, new List<Guid>()));

        var fightService = _startup.GetService<FightService>();
        //await fightService.ExecuteAsync();
    }

    private void CoreWebView2_WebResourceRequested(object sender, CoreWebView2WebResourceRequestedEventArgs e)
    {
        var coockie = _startup.GetService<ICookie>();
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

    private void OnContentLoadingAsync(object sender, CoreWebView2ContentLoadingEventArgs e)
    {
        string filter = "*";

        _webView.CoreWebView2.AddWebResourceRequestedFilter(filter,
                                                   CoreWebView2WebResourceContext.All);
        _webView.CoreWebView2.WebResourceRequested += CoreWebView2_WebResourceRequested!;
        
    }


    private void OnWindowLoaded(object sender, RoutedEventArgs e)
    {
        
        _handleFightStates = _startup.GetHandleFightStates().ToList();
        _webView.ContentLoading += OnContentLoadingAsync!;

        var domain = _startup.GetService<IDomain>();
        domainInput.Text = domain.GetUrl();

        var refreshService = _startup.GetService<RefreshService>();
        refreshService.Refresh += OnRefresh;

    }

    private void OnRefresh()
    {
        var domain = _startup.GetService<Domain>();
        _webView.CoreWebView2.Navigate(new Uri(domain.GetBaseUri(), "/hunt.php").AbsoluteUri);
    }

    private void OnDomainUrlChanged(object sender, TextChangedEventArgs e)
    {
        var domain = _startup.GetService<Domain>();
        domain.SetUrl(domainInput.Text);
        _startup.GetService<IDomainRepositry>().Set(domain);
        _webView.Source = domain.GetBaseUri();
    }

    private void OnActionOpenClick(object sender, RoutedEventArgs e)
    {
        var actionController = new ActionController(_startup.GetService<IActionRepository>(),
                                                    _startup.GetService<ITargetRepository>()); ;
        new ActionWindow(actionController).Show();
    }

    private void OnBotOpenClick(object sender, RoutedEventArgs e)
    {
        new BotWindow(new BotController(_startup.GetService<IBotRepository>(),
                                      _startup.GetService<IFightRepository>())).Show();
    }

    private void OnFightOpenClick(object sender, RoutedEventArgs e)
    {
        new FightWindow(new FightController(_startup.GetService<IFightRepository>(), 
                                            _startup.GetService<IActionRepository>())
                       ).Show();
    }

    private void OnComboOpenClick(object sender, RoutedEventArgs e)
    {
        new ComboWindow(new ComboController(_startup.GetService<IComboRepository>())).Show();
    }

    private void OnStartClick(object sender, RoutedEventArgs e)
    {
        _windowController.StartAsync();
    }

    private void OnStopClick(object sender, RoutedEventArgs e)
    {
        _windowController.Stop();
    }

    private void OnUpdateClick(object sender, RoutedEventArgs e)
    {
        _windowController.Refresh();
    }

    private void OnScreenshotClick(object sender, RoutedEventArgs e)
    {
        var bitmapRepository = _startup.GetService<IScreenshot>();
        var bitmap = bitmapRepository.TakeScreenShot();
        bitmap.Save("screen.png");
    }
}
