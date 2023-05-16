using Dwar.Repositorys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dwar.Http
{
    public class HpRepository : IHpRepository
    {
        public Hp Get()
        {
            return new Hp(180, 180);
        }
    }
}
