namespace Dwar.Services
{
    public interface IActionService
    {
        Task ExecuteAsync(StopBotCommand stopBot);
    }
}