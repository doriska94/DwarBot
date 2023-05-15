using Dwar.Repositorys;
using System.Reflection.Metadata;
using System.Text.Json;

namespace Dwar.FileIO;

public class ActionRepository : IActionRepository
{
    private readonly string FileName = "action.json";
    private const string HuntActionKey = "hunt_conf.php";
    private List<Action> _actions = new();
    public ActionRepository()
    {
        var action = Deserialize(FileName);
        if (action != null)
            _actions = action;
    }
    public Action Create(string key, string uiName, RequestType requestType, string action = "", string option = "")
    {
        var _action = new Action(key, uiName, action, option, requestType);
        _actions.Add(_action);
        Update(_action);
        return _action;
    }

    public void Delete(Action action)
    {
        var listItem = _actions.FirstOrDefault(item => item.Id == action.Id);
        if (listItem != null)
        {
            if (listItem != action)
            {
                _actions.Remove(listItem);
            }
            else
            {
                _actions.Remove(action);
            }
        }
        Serialize();
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
        var listItem = _actions.FirstOrDefault(item => item.Id == action.Id);
        if (listItem != null)
        {
            if(listItem != action)
            {
                _actions.Remove(listItem);
                _actions.Add(action);
            }
        }
        else
        { 
            _actions.Add(action); 
        }

        Serialize();
    }
    private string Serialize()
    {
        return JsonSerializer.Serialize(_actions);
    }

    private static List<Action>? Deserialize(string fileName)
    {
        if (File.Exists(fileName) == false)
            File.Create(fileName).Dispose();
        try
        {
            return JsonSerializer.Deserialize<List<Action>>(File.ReadAllText(fileName));
        }
        catch { }
        return new List<Action>();
    }

}