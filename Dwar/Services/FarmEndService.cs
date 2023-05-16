using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dwar.Services
{
    public class FarmEndService : IHandleFightState
    {
        public const string FarmEnd = "hunt_conf.php?mode=farm&action=cancel";
        private FarmState _state;
        public void SetStartFarm()
        {
            _state = FarmState.Running;
        }
        public async Task WaitEnd(int timeOut, StopBotCommand stopBot)
        {
            var count = 0;
            while (IsRunning())
            {
                await Task.Delay(500);
                if (stopBot.Stop)
                    return;
                //if (timeOut * 2 < count)
                //    return;
                count++;
            }
        }
        private bool IsRunning()
        {
            return _state == FarmState.Running;
        }
        public void HandleRequest(string url)
        {
            if(url.Contains(FarmEnd))
            {
                _state = FarmState.Stop;
            }
        }
    }
    public enum FarmState
    {
        Stop,
        Running
    }
}
