using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dwar.Services
{
    public interface IGetRequest
    {
        bool Get(string action, string paramater);
    }
}
