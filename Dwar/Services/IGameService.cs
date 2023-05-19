using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dwar.Services
{
    public interface IGameService
    {
        void StartGame();
        string StartGame50();
        void MakeStep();
        string GetGameEvaluate();
        void FinishGame();
        void Next();

        bool CannStartGame50Execute();
        bool CannMakeStepExecute();
        bool CannGetGameEvaluateExecute();
        bool CannFinishGameExecute();
        bool CannNextExecute();

    }
}
