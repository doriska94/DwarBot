using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Dwar.Services
{
    public class ActionExecuteService : IActionService, IActionSetService
    {
        private IDivnoStekloService _gameService;
        private int WinnPoinst = 13;
        public ActionExecuteService(IDivnoStekloService gameService)
        {
            _gameService = gameService;
        }

        private Fight _fightConfig = null!;
        public async Task ExecuteAsync(StopBotCommand stopBot)
        {
            if (_fightConfig == null)
                return;

            _gameService.StartGame();
            await Task.Delay(10);
            var text = "";
            if(_gameService.CannStartGame50Execute())
                text = _gameService.StartGame50();

            await Task.Delay(10);

            while (GetPoints(text) < WinnPoinst)
            {
                if (_gameService.CannMakeStepExecute())
                    _gameService.MakeStep();
                await Task.Delay(10);
                if (_gameService.CannGetGameEvaluateExecute())
                    text = _gameService.GetGameEvaluate();
                await Task.Delay(10);
            }
            if (_gameService.CannFinishGameExecute())
                _gameService.FinishGame();
            await Task.Delay(10);

            if (_gameService.CannNextExecute())
                _gameService.Next();

            await Task.Delay(10);
        }

        private int GetPoints(string text)
        {
            if (text == null)
                return 20;
            string searchTxt = "На данный момент вами набрано";

            var txt = text.Remove(0, text.IndexOf(searchTxt) + searchTxt.Length + 4);
            txt = txt.Remove(txt.IndexOf("<"));

            if (int.TryParse(text, out int result))
                return result;
            else
                return WinnPoinst + 2;
            
        }

        public void SetAttack(Fight fightConfig)
        {
            _fightConfig = fightConfig;
            _gameService.SetAttack(_fightConfig);  
        }
    }
}
