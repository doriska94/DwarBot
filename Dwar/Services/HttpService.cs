using Dwar.Repositorys;

namespace Dwar.Services;

public class HttpService
{
    private IActionRepository _actionRepository;
    private ISendRequestService _sendRequest;
    private IGetRequest _getRequest;
    private ITargetRepository _targetRepository;
    private Random _random;
    public HttpService(IActionRepository actionRepository,
                       ISendRequestService sendRequest,
                       IGetRequest getRequest,
                       Random random,
                       ITargetRepository targetRepository)
    {
        _actionRepository = actionRepository;
        _sendRequest = sendRequest;
        _getRequest = getRequest;
        _random = random;
        _targetRepository = targetRepository;
    }

    public async Task<bool> ExecuteAsync(Action action)
    {
        if (action == null)
            throw new ArgumentNullException(nameof(action));

        Target? target = null;

        var mobs = action.Mobs.ToArray();
        
        do
        {
            for (int i = 0; i < mobs.Length && target == null; i++)
            {
                target = _targetRepository.GetFreeTargetsOrDefault(mobs[i].Name);
            }

            await Task.Delay(100);

        } while (target != null);

        var result = false;
        switch (action.RequestType)
        {
            case RequestType.Get:
                result =  _getRequest.Get(action.GetAction(), action.GetParameters(_random, target!));
                break;
            case RequestType.Send:
                result =   _sendRequest.Send(action.GetAction(), action.GetParameters(_random, target!));
                break;
        }

        await Task.Delay(action.WaitAfterExecute * 1000);
        return result;
    }
}
