using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dwar.Repositorys
{
    public interface IComboRepository
    {
        Combo Get();
        void Set(Combo combo);
    }
}
