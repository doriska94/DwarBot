using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dwar
{
    public class Hp
    {
        public Hp(int actual, int maximal)
        {
            Actual = actual;
            Maximal = maximal;
        }

        public int Actual { get; }
        public int Maximal { get; }


        public bool IsFull()
        { 
            return Actual == Maximal; 
        }
    }
}
