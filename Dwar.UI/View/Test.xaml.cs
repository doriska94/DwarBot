using Microsoft.Web.WebView2.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Dwar.UI.View
{
    
    public partial class Test : Window
    {
        WebView2 _webView;
        public Test(WebView2 webView)
        {
            InitializeComponent();
            _webView = webView;
            grid.Children.Add(_webView);
        }
    }
}
