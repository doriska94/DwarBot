using Dwar.UI.Controllers;
using System.Windows;

namespace Dwar.UI.View;

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

    private void CheckBox_Checked(object sender, RoutedEventArgs e)
    {

    }
}
