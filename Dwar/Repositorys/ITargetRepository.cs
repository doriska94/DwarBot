using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dwar.Repositorys
{
    public interface ITargetRepository
    {
        Task<IEnumerable<Target>> GetTargetsAsync();
        Task<Target> GetFreeTargetsAsync(string name);
    }
}
