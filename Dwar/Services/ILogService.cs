namespace Dwar.Services;

public interface ILogService
{
    Task Write(string name, string message);
    Task Write(Exception exception);
}
