using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dwar.Services
{
    public interface IGetRequest
    {
        Task<bool> GetAsync(string action, string paramater);
    }
}
