namespace Dwar.Repositorys;

public interface IActionRepository
{
    Action Create(string key, string uiName, RequestType requestType, string action = "", string option = "");
    void Update(Action action);
    void Delete(Action action);
    Action GetActionGetTargets();
    Action Get(Guid id);
    IEnumerable<Action> GetAll(IEnumerable<Guid> ids);
    IEnumerable<Action> GetAll();
}
