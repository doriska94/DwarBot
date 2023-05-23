using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dwar.Repositorys
{
    public interface IHttpRequest
    {
        string Post(string url, string strPost, bool refer = false);
        string GetRequest(string url,string refer = null);
    }
}
