using Dwar.Repositorys;
using System.Reflection.Metadata;
using System.Text.Json;

namespace Dwar.FileIO;

public class ActionRepository : IActionRepository
{
    private readonly string _fileName = "action.json";
    private const string _huntActionKey = "hunt_conf.php";
    private List<Action> _actions = new();
    public ActionRepository()
    {
        var action = Deserialize(_fileName);
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
        var action = _actions.FirstOrDefault(x=>x.Key == _huntActionKey);
        action ??= Create(_huntActionKey, "For Search Mobs",RequestType.Get, "/hunt_conf.php");

        return action;
    }
    public IEnumerable<Action> GetAll(IEnumerable<Guid> ids)
    {
        return _actions.Where(x => ids.Any(id => id == x.Id)).ToArray();
    }
    public IEnumerable<Action> GetAll()
    {
        return _actions.ToArray();
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
    private void Serialize()
    {
        var json = JsonSerializer.Serialize(_actions);
        File.WriteAllText(_fileName,json);
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
        return null;
    }

    
}