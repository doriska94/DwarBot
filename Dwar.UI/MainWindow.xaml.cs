using System;
using System.Collections.Generic;
using System.Linq;
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
        public MainWindow()
        {
            InitializeComponent();
            var mob_id = 123456;
            var fight_id = 6789;
            //var paramsString = "object_class=ARTIFACT&object_id=" + object_id + 
            //                   "&action_id=1638&url_success=action_form.php%3Fsuccess%3D1%26default%3DARTIFACT_" + object_id + 
            //                   "_1638&url_error=action_form.php%3Ffailed%3D1%26default%3DARTIFACT_" + object_id + 
            //                   "_1638&artifact_id=" + object_id + "&in%5Bobject_class%5D=ARTIFACT&in%5Bobject_id%5D=" + object_id + 
            //                   "&in%5Baction_id%5D=1638&in%5Burl_success%5D=action_form.php%3Fsuccess%3D1&in%5B" +
            //                   "url_error%5D=action_form.php%3Ffailed%3D1";

            var paramsString = "/provocation.php?&action=agr&mob_id=" + mob_id + "&gdata=" + fight_id + "&open=1";

            var action = "/doit.php";
            var getUri = new Uri(new Uri(new Uri("https://test.de/"), action), paramsString);

        }
    }
}
