using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dwar.UI.Model
{
    public class FightModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public Action Attack { get; set; } = null!;
        public BindingList<Action> StartUpActions { get; set; } = new();

        public static FightModel Create(Fight fight) 
        {
            return new FightModel
            {
                Name = fight.Name,
                Id = fight.Id
            };
        }
    }
}
