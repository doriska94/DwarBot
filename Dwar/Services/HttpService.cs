using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dwar.Repositorys;

namespace Dwar.Services
{
    public class HttpService
    {
        private IActionRepository _actionRepository;
        private ISendRequest _sendRequest;
        private IGetRequest _getRequest;
        private ITargetRepository _targetRepository;
        private Random _random;
        public HttpService(IActionRepository actionRepository,
                           ISendRequest sendRequest,
                           IGetRequest getRequest,
                           Random random,
                           ITargetRepository targetRepository)
        {
            _actionRepository = actionRepository;
            _sendRequest = sendRequest;
            _getRequest = getRequest;
            _random = random;
            _targetRepository = targetRepository;
        }

        public async Task<bool> ExecuteAsync(Action action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            
            var target = await _targetRepository.GetFreeTargetsAsync(action.MobName);
            var result = false;
            switch (action.RequestType)
            {
                case RequestType.Get:
                    result = await _getRequest.GetAsync(action.GetAction(), action.GetParameters(_random, target));
                    break;
                case RequestType.Send:
                    result =  await _sendRequest.SendAsync(action.GetAction(), action.GetParameters(_random, target));
                    break;
            }

            await Task.Delay(action.WaitAfterExecute * 1000);
            return result;
        }
    }
}
