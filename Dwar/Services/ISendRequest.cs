using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dwar.Services
{
    public interface ISendRequest
    {
        Task SendAsync(string action, string paramater);

    }
}
