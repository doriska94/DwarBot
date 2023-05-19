using Dwar.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Dwar.Repositorys
{
    public interface ITimeOutRepository
    {
        ITimeOut Get();
        void Save(TimeOut timeOut);
    }
}
