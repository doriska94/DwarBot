using Dwar.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dwar
{
    public class TimeOut : ITimeOutService
    {
        private int _timeOutMinuts;

        public int TimeOutMinuts { get => _timeOutMinuts; set => _timeOutMinuts = value; }
        private DateTime _lastAction;
        public bool IsOut()
        {
            return DateTime.Now > _lastAction.AddMinutes(TimeOutMinuts);
        }

        public void HandleAction()
        {
            _lastAction = DateTime.Now;
        }
    }
}
