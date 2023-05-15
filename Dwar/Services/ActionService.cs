using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dwar.Repositorys;

namespace Dwar.Services
{
    public  class ActionService
    {
        private IActionRepository _actionRepository;
        private ISendRequest _sendRequest;
        private IGetRequest _getRequest;

        public ActionService(IActionRepository actionRepository, ISendRequest sendRequest, IGetRequest getRequest)
        {
            _actionRepository = actionRepository;
            _sendRequest = sendRequest;
            _getRequest = getRequest;
        }

        public async Task ExecuteAsync(Guid actionId, IEnumerable<Paramerter> paramerters)
        {
            if(actionId == Guid.Empty)
                throw new ArgumentException(nameof(actionId));
            if(paramerters == null)
                throw new ArgumentNullException(nameof(paramerters));

            Action action = _actionRepository.Get(actionId);

            switch (action.RequestType)
            {
                case RequestType.Get:
                    await _getRequest.GetAsync(action.GetAction(), action.GetParameters(paramerters));
                    break;
                case RequestType.Send:
                        await _sendRequest.SendAsync(action.GetAction(),action.GetParameters(paramerters));
                    break;
            }
        }
    }
}
