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

    public Target GetByAnthorId(Target target)
    {
        throw new NotImplementedException();
    }

    public Target GetById(Target target)
    {
        throw new NotImplementedException();
    }

    public Target GetFreeTargets(string? name)
    {
        if (string.IsNullOrEmpty(name))
            return null!;

        return _targets.FirstOrDefault(t => t.Name == name)!;
    }

    public Target? GetFreeTargetsOrDefault(string? name)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Target> GetTargets()
    {
        return _targets;
    }
}
