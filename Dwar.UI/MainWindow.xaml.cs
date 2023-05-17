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

namespace Dwar.UI;

public partial class MainWindow : Window
{
    private Startup _startup = new();
    private List<IHandleFightState> _handleFightStates = new();
    private MainWindowController _windowController;
    private NewTab _newTab = null!;
    public MainWindow()
    {
        _startup.Configure();
        InitializeComponent();
        _windowController = new MainWindowController(_startup.GetService<IBotRepository>(),
                                                     _startup.GetService<BotService>());
        _windowController.Stoped += OnBotStoped;
        DataContext= _windowController;
    }

    private void OnBotStoped()
    {
        WindowState = WindowState.Maximized;
        Focus();

        var domain = _startup.GetService<Domain>();
        _webView.CoreWebView2.Navigate(new Uri(domain.GetBaseUri(), "/main.php").AbsoluteUri);
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
        refreshService.GoToMain += OnBotStoped;

        _newTab = new(_webView, domain, huntTab, _startup, tabGrid);

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
                                                    //new MemoryTargetRepository());
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
        _newTab.CloseTab();
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

    WebView2 _webViewSecondTab;
    private void OnTestClick(object sender, RoutedEventArgs e)
    {
        var domain = _startup.GetService<Domain>();
        var uri = new Uri(domain.GetBaseUri(), "/hunt.php");
        if (_webViewSecondTab == null)
        {
            new Test(_webViewSecondTab).Show();
        }
    }
}
