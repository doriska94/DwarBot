using Dwar.UI.Controllers;
using System.Windows;

namespace Dwar.UI.View;

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
