namespace Dwar.Repositorys;

public interface IFightRepository
{
    Fight Get(Guid id);
    IEnumerable<Fight> GetAll();
    void Save(Fight entity);
    void Delete(Guid id);
    void Delete(Fight fight);
    Fight Create(string name, Guid attackId, IEnumerable<Guid> StartUpActions, Guid after5Fight);

}
