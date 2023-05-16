using Dwar.UI.Controllers;
using Dwar.Services;
using System.Windows;

namespace Dwar.UI.View;

public partial class ActionWindow : Window
{
    private ActionController _controller;
    public ActionWindow(ActionController controller)
    {
        InitializeComponent();
        _controller = controller;
        DataContext = controller;
    }

    private void OnCreateNewClick(object sender, RoutedEventArgs e)
    {
        _controller.Create();
    }

    private void OnSaveClick(object sender, RoutedEventArgs e)
    {
        _controller.Update();
    }

    private void OnAddParameter(object sender, RoutedEventArgs e)
    {
        _controller.AddParameters(new Paramerter("", "", false));
    }

    private void OnDeleteClick(object sender, RoutedEventArgs e)
    {
        _controller.Remove();
    }

    private void OnDeleteParameter(object sender, RoutedEventArgs e)
    {
        _controller.RemoveParameter();
    }
}
