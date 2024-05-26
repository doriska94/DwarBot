using Dwar.Services;
using WindowsInput;
using WindowsInput.Native;

namespace Dwar.UI.WindowsRepositries;

public class UserInput : IUserInputService
{
    private InputSimulator _simulator;
    public UserInput()
    {
        _simulator = new InputSimulator();
    }
    public void Down()
    {
        _simulator.Keyboard.KeyPress(VirtualKeyCode.VK_E);
    }

    public void Left()
    {
        _simulator.Keyboard.KeyPress(VirtualKeyCode.VK_R);

    }
    public void PressT()
    {
        _simulator.Keyboard.KeyPress(VirtualKeyCode.VK_T);

    }

    public void Right()
    {
        _simulator.Keyboard.KeyPress(VirtualKeyCode.VK_W);
    }

    public void Up()
    {
        _simulator.Keyboard.KeyPress(VirtualKeyCode.VK_Q);
    }
}
