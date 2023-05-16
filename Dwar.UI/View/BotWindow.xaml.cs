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
    public partial class BotWindow : Window
    {
        private BotController _botController;
        public BotWindow(BotController botController)
        {
            InitializeComponent();
            _botController = botController;
            DataContext= _botController;
        }

        private void OnCreateNewClick(object sender, RoutedEventArgs e)
        {
            _botController.Create();
        }

        private void OnSaveClick(object sender, RoutedEventArgs e)
        {
            _botController.Update();
        }

        private void OnDeleteClick(object sender, RoutedEventArgs e)
        {
            _botController.Remove();
        }
    }
}
