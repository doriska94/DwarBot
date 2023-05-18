using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dwar.Repositorys
{
    public interface ITargetRepository
    {
        IEnumerable<Target> GetTargets();
        Target GetFreeTargets(string? name);
        Target? GetById(Target target);
        Target? GetByAnthorId(Target target);
    }
}
