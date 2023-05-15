using Dwar.Http;
using Dwar.Repositorys;
using Microsoft.Web.WebView2.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Collections.Specialized.BitVector32;

namespace Dwar.UI
{
    public partial class MainWindow : Window
    {
        private Startup startup = new Startup();
        public MainWindow()
        {
            InitializeComponent();
            

        }

        private async void GetTargets()
        {
            var targetRepository = startup.GetService<ITargetRepository>();
            var targets = await targetRepository.GetTargetsAsync();
        }

        private void OnTestClick(object sender, RoutedEventArgs e)
        {
            GetTargets();
        }

        private void OnOpenBotClick(object sender, RoutedEventArgs e)
        {

        }

        private void CoreWebView2_WebResourceRequested(object sender, CoreWebView2WebResourceRequestedEventArgs e)
        {
            var coockie = startup.GetService<ICookie>();
            var headers = e.Request.Headers;
            string postData = null!;
            var content = e.Request.Content;

            // get content from stream
            if (content != null)
            {
                using (var ms = new MemoryStream())
                {
                    content.CopyTo(ms);
                    ms.Position = 0;
                    postData = Encoding.UTF8.GetString(ms.ToArray());
                }
            }
            var url = e.Request.Uri.ToString();

            // collect the headers from the collection into a string buffer
            StringBuilder sb = new StringBuilder();
            foreach (var header in headers)
            {
                if (header.Key.ToLower() == "cookie")
                {
                    coockie.Set(header.Value);
                }
                sb.AppendLine($"{header.Key}: {header.Value}");
            }

            // for demo write out captured string vals
            Debug.WriteLine($"{url}\n{sb.ToString()}\n{postData}\n---");
        }

        private async void _webView_ContentLoadingAsync(object sender, Microsoft.Web.WebView2.Core.CoreWebView2ContentLoadingEventArgs e)
        {
            string filter = "*main.php*";   // or "*" for all requests
            _webView.CoreWebView2.AddWebResourceRequestedFilter(filter,
                                                       CoreWebView2WebResourceContext.All);
            _webView.CoreWebView2.WebResourceRequested += CoreWebView2_WebResourceRequested!;
            var tt = await _webView.CoreWebView2.ExecuteScriptAsync("document.documentElement.outerHTML");
        }

        private void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            startup.Configure();

            _webView.ContentLoading += _webView_ContentLoadingAsync!;

            var domain = startup.GetService<IDomain>();
            domain.SetUrl("https://dwarlegacy.ru/");

        }
    }
}
