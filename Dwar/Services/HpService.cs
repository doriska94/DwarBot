﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dwar.Repositorys;

namespace Dwar.Services
{
    public class HpService
    {
        private const int CheckHpTimeMillis = 2000;
        private IHpRepository _hpRepository;

        public HpService(IHpRepository hpRepository)
        {
            _hpRepository = hpRepository;
        }

        public async Task WaitFullHp()
        {
            Hp hp = _hpRepository.Get();
            while (hp.IsFull() == false) 
            {
                await Task.Delay(CheckHpTimeMillis);
                hp = _hpRepository.Get();

            } 
        }
    }
}
