using Dwar.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Dwar.UI.WindowsRepositries
{
    public class Notifyer : INotifyer
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
}
