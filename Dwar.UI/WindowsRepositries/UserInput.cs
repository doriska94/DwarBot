using Dwar.Services;
using WindowsInput;
using WindowsInput.Events;

namespace Dwar.UI.WindowsRepositries;

public class UserInput : IUserInput
{
    public UserInput()
    {

    }
    public void Down()
    {
        Simulate.Events().Click(KeyCode.Down);
    }

    public void Left()
    {
        Simulate.Events().Click(KeyCode.Left);

    }

    public void Right()
    {
        Simulate.Events().Click(KeyCode.Right);
    }

    public void Up()
    {
        Simulate.Events().Click(KeyCode.Up);
    }
}
