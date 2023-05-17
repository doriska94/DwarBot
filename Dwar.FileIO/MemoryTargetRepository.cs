using Dwar.Repositorys;
using System.Xml.Linq;

namespace Dwar.FileIO;

public class MemoryTargetRepository : ITargetRepository
{
    private List<Target> _targets = new List<Target>()
    {
        new Target(1,"M1",0),
        new Target(1,"M2",0),
        new Target(1,"M3",0),
        new Target(1,"M4",0),
        new Target(1,"M5",0),
        new Target(1,"M6",0),
    };

    public Task<Target> GetFreeTargetsAsync(string name)
    {
        return Task.Factory.StartNew(()=>_targets.FirstOrDefault(t => t.Name == name));
    }

    public Task<IEnumerable<Target>> GetTargetsAsync()
    {
        return Task< IEnumerable<Target>>.Factory.StartNew(() => _targets);
    }
}
