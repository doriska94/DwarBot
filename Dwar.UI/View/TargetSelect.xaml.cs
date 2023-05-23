using System.Collections.Generic;
using System.Linq;
using System.Windows;


namespace Dwar.UI.View;


public partial class TargetSelect : Window
{
    public Target? SelectedTarget { get; set; }
    private List<Target> _targets = new();
    public TargetSelect(IEnumerable<Target> targets)
    { 
        InitializeComponent();
        _targets = targets.ToList();
        targetBox.ItemsSource = _targets;
    }

    private void OnOkClicks(object sender, RoutedEventArgs e)
    {
        DialogResult = SelectedTarget!= null;
    }

    private void OnCancelClicks(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
    }
}
