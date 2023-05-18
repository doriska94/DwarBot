using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dwar.Services
{
    public interface ILog
    {
        Task Write(string name, string message);
        Task Write(Exception exception);
    }
}
