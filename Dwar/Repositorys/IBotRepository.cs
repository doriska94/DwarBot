namespace Dwar.Repositorys;

public interface IBotRepository
{
    Bot Create();
    IEnumerable<Bot> GetAll();
    Bot GetById(Guid id);
    void Save(Bot bot);
    void Delete(Bot bot);
    void Delete(Guid id);

}
