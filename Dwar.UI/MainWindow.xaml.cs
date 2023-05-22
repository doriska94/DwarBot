using Dwar.Http;
using Dwar.Repositorys;
using Dwar.Services;
using Microsoft.Web.WebView2.Core;
using System.Windows;
using System.Linq;
using System.Collections.Generic;
using System;
using Dwar.UI.View;
using System.Windows.Controls;
using Dwar.UI.Controllers;
using Microsoft.Web.WebView2.Wpf;
using System.Reflection.Metadata;
using System.IO;
using System.Text;
using System.DirectoryServices.ActiveDirectory;

namespace Dwar.UI;

public partial class MainWindow : Window
{
    private bool Setted = false;
    private Startup _startup = new();
    private List<IHandleFightState> _handleFightStates = new();
    private MainWindowController _windowController;
    private NewTab _newTab = null!;
    public MainWindow()
    {
        InitializeComponent();
        _startup.Configure(Log);
        _windowController = new MainWindowController(_startup.GetService<IBotRepository>(),
                                                     _startup.GetService<BotService>());
        _windowController.Stoped += OnBotStoped;
        DataContext = _windowController;
    }

    private void OnBotStoped()
    {
        WindowState = WindowState.Maximized;
        Focus();
        _newTab.CloseTab();
    }

    private void CoreWebView2_WebResourceRequested(object sender, CoreWebView2WebResourceRequestedEventArgs e)
    {
        var coockie = _startup.GetService<ICookie>();
        var headers = e.Request.Headers;
        var content = e.Request.Content;
        _handleFightStates.ForEach(x => x.HandleRequest(e.Request.Uri));

        foreach (var header in headers)
        {
            if (header.Key.ToLower() == "cookie")
            {
                coockie.Set(header.Value);
            }
        }

    }

    public const string Victory = "*fightover_victory.ogg*";
    public const string Defeat = "*fightover_defeat.ogg*";
    public const string StartFight = "*combo.css*";
    public const string FarmEnd = "*hunt_conf.php?mode=farm&action=cancel*";

    private void OnContentLoadingAsync(object sender, CoreWebView2ContentLoadingEventArgs e)
    {
        if (Setted)
            return;

        string filter = "*main.php*";

        _webView.CoreWebView2.AddWebResourceRequestedFilter(Victory,
                                                   CoreWebView2WebResourceContext.All);
        _webView.CoreWebView2.AddWebResourceRequestedFilter(Defeat,
                                                   CoreWebView2WebResourceContext.All);
        _webView.CoreWebView2.AddWebResourceRequestedFilter(StartFight,
                                                   CoreWebView2WebResourceContext.All);
        _webView.CoreWebView2.AddWebResourceRequestedFilter(FarmEnd,
                                                   CoreWebView2WebResourceContext.All);
        _webView.CoreWebView2.AddWebResourceRequestedFilter(filter,
                                                   CoreWebView2WebResourceContext.All);
        _webView.CoreWebView2.WebResourceRequested += CoreWebView2_WebResourceRequested!;

        Setted = true;
    }


    private void OnWindowLoaded(object sender, RoutedEventArgs e)
    {

        _handleFightStates = _startup.GetHandleFightStates().ToList();
        _webView.ContentLoading += OnContentLoadingAsync!;

        var domain = _startup.GetService<IDomain>();
        domainInput.Text = domain.GetUrl();

        var refreshService = _startup.GetService<RefreshService>();
        refreshService.Refresh += OnRefresh;
        refreshService.GoToMain += OnBotStoped;

        _newTab = new(_webView, domain, huntTab, _startup, tabGrid, windowTabs);

    }

    private void OnRefresh()
    {
        var domain = _startup.GetService<Domain>();
        _newTab.OpenTab(new Uri(domain.GetBaseUri(), "/hunt.php").AbsoluteUri);
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

    private async void OnTestClick(object sender, RoutedEventArgs e)
    {
        var actionExecuter = _startup.GetService<ActionExecuteService>();
        var fightRepository = _startup.GetService<IFightRepository>();
        var actionRepository = _startup.GetService<IActionRepository>();
        //var hhtp = _startup.GetService<HttpService>();
        //var acti = actionRepository.Get(Guid.Parse("908f48a2-cf54-4af8-8a58-980ad2a93064"));
       // await hhtp.ExecuteAsync(acti);
        //var startGame = actionRepository.Create("StartGame Divnoe Seklo", "StartGame Divnoe Seklo", RequestType.Get, "npc.php", "f_id=10&npc_id=197&global_npc=0&quest_id=561&point_id=11052&9ffe5073e20bad7218272862a52cfa52");
        //var startGame50 = actionRepository.Create("StartGame50 Divnoe Seklo", "StartGame Divnoe Seklo", RequestType.Get, "npc.php", "f_id=10&npc_id=197&global_npc=0&quest_id=561&point_id=11052&action=answer&ref=11079&b7907c10a00709ab462b03a7e9b3b5d4");
        //var makeStep = actionRepository.Create("MakeStep Divnoe Seklo", "StartGame Divnoe Seklo", RequestType.Get, "npc.php", "f_id=10&npc_id=197&global_npc=0&quest_id=561&point_id=11052&action=answer&ref=11086&231a00b41bc2bfcd58cce6cb0394600b");
        //var getGameEvaluate = actionRepository.Create("GetGameEvaluate Divnoe Seklo", "StartGame Divnoe Seklo", RequestType.Get, "npc.php", "f_id=10&npc_id=197&global_npc=0&quest_id=561&point_id=11052&action=answer&ref=11088&08b941c6de694c379d7d3d623f6b1936");
        //var finishGame = actionRepository.Create("FinishGame Divnoe Seklo", "StartGame Divnoe Seklo", RequestType.Get, "npc.php", "f_id=10&npc_id=197&global_npc=0&quest_id=561&point_id=11052&action=answer&ref=11087&bbb8d70fee7d9427850a8fcea58a94c8");
        //var next = actionRepository.Create("Next Divnoe Seklo", "StartGame Divnoe Seklo", RequestType.Get, "npc.php", "f_id=10&npc_id=197&global_npc=0&quest_id=561&point_id=11052&action=done&787f12a4c084f4bae0d43a1a0f144a00");
        
        //actionRepository.Update(startGame);
        //actionRepository.Update(startGame50);
        //actionRepository.Update(makeStep);
        //actionRepository.Update(getGameEvaluate);
        //actionRepository.Update(finishGame);
        //actionRepository.Update(next);


        //var fight = fightRepository.Create("Divnoe Steklo", startGame.Id, new[] { startGame50.Id, makeStep.Id, getGameEvaluate.Id, finishGame.Id, next.Id });
        //fightRepository.Save(fight);
        var fight = fightRepository.Get(Guid.Parse("f6ebcd61-246e-4f8b-98a2-c1dc5323569c"));
        //var dd = fight.Id.ToString();

        actionExecuter.SetAttack(fight);
        await actionExecuter.ExecuteAsync(new StopBotCommand());
    }
}
