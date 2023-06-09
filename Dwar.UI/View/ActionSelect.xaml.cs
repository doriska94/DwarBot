﻿using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Dwar.UI.View;

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
