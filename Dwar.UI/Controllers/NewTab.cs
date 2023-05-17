using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.Wpf;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Dwar.UI.Controllers
{
    public  class NewTab
    {
        private List<CoreWebView2Cookie> _coreWebManager = null!;
        private WebView2 _webView;
        private WebView2 _webViewSecondTab = null!;
        private IDomain _domain;
        private TabItem _tabItem;
        private Grid _tabGrid;
        private Startup _startup;
        public NewTab(WebView2 webView, IDomain domain, TabItem tabItem, Startup startup, Grid tabGrid)
        {
            _webView = webView;
            _domain = domain;
            _tabItem = tabItem;
            _startup = startup;
            _tabGrid = tabGrid;
        }

        private WebView2 CreateSecondTab(string url)
        {
            if (_webViewSecondTab == null)
            {
                _webViewSecondTab = new WebView2();
                _webViewSecondTab.ContentLoading += OnContentLoading;
            }
            _webViewSecondTab.Source= new Uri(url);
            return _webViewSecondTab;
        }
        public void OpenTab(string url)
        {
            _tabItem.Visibility = Visibility.Visible;
            
            if(_webViewSecondTab != null)
            {
                CreateSecondTab(url);
            }

            _tabGrid.Children.Add(_webViewSecondTab);

        }
        public void CloseTab()
        {
            _tabItem.Visibility = Visibility.Collapsed;
            
            _tabGrid.Children.Clear();
        }
        private async void OnContentLoading(object? sender, CoreWebView2ContentLoadingEventArgs e)
        {
            _coreWebManager = await _webView.CoreWebView2.CookieManager.GetCookiesAsync(_domain.GetUrl());

            foreach (var cookie in _coreWebManager)
            {
                _webViewSecondTab.CoreWebView2.CookieManager.AddOrUpdateCookie(cookie);
            }

            string filter = "*";

            _webView.CoreWebView2.AddWebResourceRequestedFilter(filter,
                                                       CoreWebView2WebResourceContext.All);
            _webView.CoreWebView2.WebResourceRequested += CoreWebView2_WebResourceRequested;
        }

        private void CoreWebView2_WebResourceRequested(object? sender, CoreWebView2WebResourceRequestedEventArgs e)
        {
            _startup.GetHandleFightStates().ToList().ForEach(state =>
            {
                state.HandleRequest(e.Request.Uri);
            });
        }
    }
}
