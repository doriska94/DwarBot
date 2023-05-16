using Dwar.UI.Controllers;
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
    public partial class ComboWindow : Window
    {
        private ComboController _controller;
        public ComboWindow(ComboController controller)
        {
            InitializeComponent();
            _controller = controller;
            DataContext = controller;
        }

        private void OnDeleteStep(object sender, RoutedEventArgs e)
        {
            _controller.RemoveStep();
        }

        private void OnAddStep(object sender, RoutedEventArgs e)
        {
            _controller.AddNewStep();
        }

        private void OnSaveClick(object sender, RoutedEventArgs e)
        {
            _controller.Save();
        }
    }
}
