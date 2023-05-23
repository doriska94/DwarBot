using Dwar.Repositorys;

namespace Dwar.Services;

public class GameService : IDivnoStekloService
{
    private Fight _fight= null!;
    private IHttpRequest _httpRequest;
    private IActionRepository _actionRepository;
    private IDomain _domain;
    public GameService(IHttpRequest httpRequest, IActionRepository actionRepository, IDomain domain)
    {
        _httpRequest = httpRequest;
        _actionRepository = actionRepository;
        _domain = domain;
    }

    public void FinishGame()
    {
        var last = _actionRepository.Get(_fight.StartUpActions.ToList()[2]);
        var startAction = _actionRepository.Get(_fight.StartUpActions.ToList()[3]);
        _httpRequest.GetRequest(GetUrl(startAction.Method + "?" + startAction.Option),
                               GetUrl(last.Method + "?" + last.Option));
    }

    public string GetGameEvaluate()
    {
        var last = _actionRepository.Get(_fight.StartUpActions.ToList()[1]);
        var startAction = _actionRepository.Get(_fight.StartUpActions.ToList()[2]);
        return _httpRequest.GetRequest(GetUrl(startAction.Method + "?" + startAction.Option),
                                       GetUrl(last.Method + "?" + last.Option));
    }

    public void MakeStep()
    {
        var last = _actionRepository.Get(_fight.StartUpActions.ToList()[0]);
        var startAction = _actionRepository.Get(_fight.StartUpActions.ToList()[1]);
        _httpRequest.GetRequest(GetUrl(startAction.Method + "?" + startAction.Option),
                               GetUrl(last.Method + "?" + last.Option));
    }

    public void Next()
    {
        var last = _actionRepository.Get(_fight.StartUpActions.ToList()[3]);
        var startAction = _actionRepository.Get(_fight.StartUpActions.ToList()[4]);
        _httpRequest.GetRequest(GetUrl(startAction.Method + "?" + startAction.Option),
                                GetUrl(last.Method + "?" + last.Option));
    }

    public void StartGame()
    {
        var last = _actionRepository.Get(_fight.StartUpActions.ToList()[4]);
        var startAction = _actionRepository.Get(_fight.AttackId);

        var result = _httpRequest.GetRequest(GetUrl(startAction.Method + "?" + startAction.Option),
                                            GetUrl(last.Method + "?" + last.Option));
    }

    public string StartGame50()
    {
        var last = _actionRepository.Get(_fight.AttackId);
        var startAction = _actionRepository.Get(_fight.StartUpActions.ToList()[0]);
        return _httpRequest.GetRequest(GetUrl(startAction.Method + "?" + startAction.Option),
                                       GetUrl(last.Method + "?" + last.Option));
    }

    public void SetAttack(Fight fightConfig)
    {
        _fight = fightConfig;
    }
    private string GetUrl(string url)
    {
        var getUri = new Uri(_domain.GetBaseUri(), url);
        return getUri.AbsoluteUri;
    }

    public bool CannStartGame50Execute()
    {
        return _fight.StartUpActions.Count() > 0;
    }

    public bool CannMakeStepExecute()
    {
        return _fight.StartUpActions.Count() > 1;
    }

    public bool CannGetGameEvaluateExecute()
    {
        return _fight.StartUpActions.Count() > 2;
    }

    public bool CannFinishGameExecute()
    {
        return _fight.StartUpActions.Count() > 3;
    }

    public bool CannNextExecute()
    {
        return _fight.StartUpActions.Count() > 4;
    }
}
