using Dwar.Services;

namespace Dwar.Repositorys;

public interface ITimeOutRepository
{
    ITimeOutService Get();
    void Save(TimeOut timeOut);
}
