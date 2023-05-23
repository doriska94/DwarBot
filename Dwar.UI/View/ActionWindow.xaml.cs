using Dwar.UI.Controllers;
using Dwar.Services;
using System.Windows;
using Dwar.Repositorys;

namespace Dwar.UI.View;

public partial class ActionWindow : Window
{
    private ActionController _controller;
    private ITargetRepository _targetRepository;
    public ActionWindow(ActionController controller, ITargetRepository targetRepository)
    {
        InitializeComponent();
        _controller = controller;
        DataContext = controller;
        _targetRepository = targetRepository;
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

    private void ClickOnAddMob(object sender, RoutedEventArgs e)
    {
        var targets = _targetRepository.GetTargets();
        var targetSelect = new TargetSelect(targets);
        targetSelect.ShowDialog();
        if (targetSelect.DialogResult == true && targetSelect.SelectedTarget != null)
            _controller.AddTarget(targetSelect.SelectedTarget);
    }

    private void ClickOnRemoveMob(object sender, RoutedEventArgs e)
    {
        _controller.RemoveSelectedTarget();
    }
}
