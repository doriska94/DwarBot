namespace Dwar.Services;

public class RefreshService 
{
    public event System.Action Refresh = null!;
    private const int DelayAfterClickMillis = 500;

    public RefreshService()
    {
    }

    public async Task<bool> ClickHunt()
    {
        Refresh?.Invoke();

        await Task.Delay(DelayAfterClickMillis);
        return true;
    }
}
