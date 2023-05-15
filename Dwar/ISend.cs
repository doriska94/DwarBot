using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dwar
{
    public interface ISend
    {
        void Send(Action action);
    }
}
