using Dwar.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WindowsInput;
using WindowsInput.Native;
using static System.Net.Mime.MediaTypeNames;

namespace Dwar.UI.WindowsRepositries
{
    public class UserInput : IUserInput
    {
        InputSimulator _inputSimulator;
        public UserInput()
        {
            _inputSimulator = new InputSimulator();
        }
        public void Down()
        {
           _inputSimulator.Keyboard.KeyPress(VirtualKeyCode.VK_E);
        }

        public void Left()
        {
            _inputSimulator.Keyboard.KeyPress(VirtualKeyCode.VK_R);
            
        }

        public void Right()
        {
            _inputSimulator.Keyboard.KeyPress(VirtualKeyCode.VK_W);
        }

        public void Up()
        {
            _inputSimulator.Keyboard.KeyPress(VirtualKeyCode.VK_Q);
        }
    }
}
