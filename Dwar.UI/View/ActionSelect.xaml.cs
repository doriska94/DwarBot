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
    public partial class ActionSelect : Window
    {
        public Action? SelectedAction { get; set; }
        private List<Action> _actions = new();

        public ActionSelect(IEnumerable<Action> actions)
        {
            InitializeComponent();
            _actions = actions.ToList();
            actionBox.ItemsSource = _actions;
        }

        private void OnOkClicks(object sender, RoutedEventArgs e)
        {
            DialogResult = SelectedAction != null;
        }

        private void OnCancelClicks(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
