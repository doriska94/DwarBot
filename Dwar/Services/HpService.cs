using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dwar.Repositorys;

namespace Dwar.Services
{
    public class HpService
    {
        private const int CheckHpTimeMillis = 1000;
        private IHpRepository _hpRepository;
        public HpService(IHpRepository hpRepository)
        {
            _hpRepository = hpRepository;
        }

        public async Task WaitFullHp(StopBotCommand stopBot)
        {
            Hp hp = await _hpRepository.GetAsync();
            while (hp.IsFull() == false) 
            {
                await Task.Delay(CheckHpTimeMillis);
                hp = await _hpRepository.GetAsync();
                if (stopBot.Stop)
                    return;
            } 
        }
    }
}
