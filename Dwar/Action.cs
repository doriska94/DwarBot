using Dwar.Services;
using System.Collections.Specialized;
using System.Web;

namespace Dwar
{
    public class Action
    {
        private string? _action;
        private string? _option;
        private NameValueCollection? _nameValueCollection;

        public Guid Id { get; } = Guid.NewGuid();
        public string Key { get; }
        public string UiName { get; set; }
        public RequestType RequestType { get; }
        public Action(string key, string uiName, string action, string option, RequestType requestType)
        {
            UiName = uiName;
            SetAction(action);
            SetOptions(option);
            RequestType = requestType;
            Key = key;
        }

        public void SetAction(string action)
        {
            if(action == null)
            {
                _action = string.Empty;
                return;
            }
            _action = action;
        }

        public void SetOptions(string option)
        {
            if(option== null)
            {
                _option = string.Empty;
                _nameValueCollection= null;
                return;
            }

            _option = option;
            _nameValueCollection = HttpUtility.ParseQueryString(_option);
        }
        public string GetAction()
        {
            return _action ?? "";
        }

        public string GetParameters(IEnumerable<Paramerter> paramerters)
        {
            if(paramerters == null) 
                throw new ArgumentNullException(nameof(paramerters));
            if(_nameValueCollection == null)
                return string.Empty;

            foreach (var paramerter in paramerters)
            {
                if(_nameValueCollection.HasKey(paramerter.Key))
                    _nameValueCollection.Set(paramerter.Key, paramerter.Value);
            }

            return _nameValueCollection.ToString() ?? "";
        }

    }
}