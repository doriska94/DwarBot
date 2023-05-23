using Dwar.Services;
using System.Windows.Controls;

namespace Dwar.UI.WindowsRepositries;

public class Notifyer : INotifyerService
{
    private TextBlock _textBlock;
    public Notifyer(TextBlock textBlock)
    {
        _textBlock = textBlock;
    }
    public void Notify(string status)
    {
        _textBlock.Text = status + "\n";
    }
}
