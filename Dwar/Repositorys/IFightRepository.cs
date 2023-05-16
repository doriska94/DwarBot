using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dwar.Repositorys
{
    public interface IFightRepository
    {
        Fight Get(Guid id);
        IEnumerable<Fight> GetAll();
        void Save(Fight entity);
        Fight Create(string name, Action attack, IEnumerable<Action> StartUpActions);

    }
}
