namespace Dwar.Repositorys;

public interface IHpRepository
{
    Task<Hp> GetAsync();
}
