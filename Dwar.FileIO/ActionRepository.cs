using Dwar.Repositorys;

namespace Dwar.FileIO;

public class ActionRepository : IActionRepository
{
    private const string HuntActionKey = "hunt_conf.php";
    private List<Action> _actions = new();
    public Action Create(string key, string uiName, RequestType requestType, string action = "", string option = "")
    {
        var _action = new Action(key, uiName, action, option, requestType);
        _actions.Add(_action);
        Update(_action);
        return _action;
    }

    public void Delete(Action action)
    {
        throw new NotImplementedException();
    }

    public Action Get(Guid id)
    {
        return _actions.Single(x => x.Id == id);
    }

    public Action GetActionGetTargets()
    {
        var action = _actions.FirstOrDefault(x=>x.Key == HuntActionKey);
        action ??= Create(HuntActionKey, "For Search Mobs",RequestType.Get, "/hunt_conf.php");

        return action;
    }

    public void Update(Action action)
    {
        throw new NotImplementedException();
    }
}