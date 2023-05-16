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
    public partial class FightWindow : Window
    {
        private FightController _fightController;
        public FightWindow(FightController fightController)
        {
            InitializeComponent();
            _fightController = fightController;
            DataContext = _fightController;
        }

        private void OnAddActions(object sender, RoutedEventArgs e)
        {
            var actionsWindow = new ActionSelect(_fightController.AttackActions);
            actionsWindow.ShowDialog();

            if(actionsWindow.DialogResult  == true && actionsWindow.SelectedAction != null)
            {
                _fightController.AddStartUpActions(actionsWindow.SelectedAction);
            }    
        }

        private void OnDeleteActions(object sender, RoutedEventArgs e)
        {
            _fightController.RemoveStartUpActions();
        }

        private void OnCreateNewClick(object sender, RoutedEventArgs e)
        {
            _fightController.Create();
        }

        private void OnSaveClick(object sender, RoutedEventArgs e)
        {
            _fightController.Update();
        }

        private void OnDeleteClick(object sender, RoutedEventArgs e)
        {
            _fightController.Delete();
        }
    }
}
