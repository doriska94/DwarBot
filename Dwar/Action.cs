using Dwar.Services;
using System.Collections.Specialized;
using System.Web;

namespace Dwar
{
    public class Action
    {
        public string? Method { get; set; }
        public string? Option { get; set; }
        private NameValueCollection? _nameValueCollection;

        public Guid Id { get; set; } = Guid.NewGuid();
        public string Key { get; set; } = string.Empty;
        public string UiName { get; set; } = string.Empty;
        public List<Paramerter> Paramerters { get; set; } = new();
        public RequestType RequestType { get; set; }

        public Action()
        {
            
        }

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
                Method = string.Empty;
                return;
            }
            Method = action;
        }

        public void SetOptions(string option)
        {
            if(option== null)
            {
                Option = string.Empty;
                _nameValueCollection= null;
                return;
            }

            Option = option;
            _nameValueCollection = HttpUtility.ParseQueryString(Option);
        }
        public string GetAction()
        {
            return Method ?? "";
        }

        public string GetParameters()
        {
            
            if(_nameValueCollection == null)
                return string.Empty;

            foreach (var paramerter in Paramerters)
            {
                if(_nameValueCollection.HasKey(paramerter.Key))
                    _nameValueCollection.Set(paramerter.Key, paramerter.GetValue());
            }

            return _nameValueCollection.ToString() ?? "";
        }

    }
}