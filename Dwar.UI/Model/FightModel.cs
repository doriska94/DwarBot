using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dwar.UI.Model
{
    public class FightModel
    {
        public string Name { get; set; } = null!;
        public Action Attack { get; set; } = null!;
        public List<Action> StartUpActions { get; set; } = new();
    }
}
