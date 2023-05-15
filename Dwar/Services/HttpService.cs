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

        public HttpService(IActionRepository actionRepository, ISendRequest sendRequest, IGetRequest getRequest)
        {
            _actionRepository = actionRepository;
            _sendRequest = sendRequest;
            _getRequest = getRequest;
        }

        public async Task ExecuteAsync(Action action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            switch (action.RequestType)
            {
                case RequestType.Get:
                    await _getRequest.GetAsync(action.GetAction(), action.GetParameters());
                    break;
                case RequestType.Send:
                    await _sendRequest.SendAsync(action.GetAction(), action.GetParameters());
                    break;
            }
        }
    }
}
