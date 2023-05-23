using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Dwar.UI.Controllers;

public class NotifyChanged : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string name = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
