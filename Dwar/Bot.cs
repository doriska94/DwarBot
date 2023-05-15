using Dwar.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dwar
{
    public class Bot
    {
        private List<Guid> _actionsid = new List<Guid>();
        private Dictionary<Guid, IEnumerable<Paramerter>> _actionsParameters = new();
        public bool WaitHp { get; set; }
        public IEnumerable<Guid> ActionsId => _actionsid;


        public Bot()
        {

        }

        public void Add(Guid actionsid,IEnumerable<Paramerter> paramerters)
        {
            _actionsid.Add(actionsid);
            _actionsParameters.Add(actionsid, paramerters);
        }
        public IEnumerable<Paramerter> GetParamerters(Guid actionid)
        {
            return _actionsParameters[actionid];
        }

    }
}
