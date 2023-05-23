using Dwar.UI.Controllers;
using System.Windows;

namespace Dwar.UI.View;

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
