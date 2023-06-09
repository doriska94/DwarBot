﻿using Dwar.Services;
using System.Collections.Specialized;
using System.Text.Json.Serialization;
using System.Web;

namespace Dwar;

public class Action
{
    private string? _option;
    private string? _method;

    public string? Method { get => _method; set { _method = value; SetAction(value); } }
    public string? Option { get => _option; set { _option = value; SetOptions(value); } }
    private NameValueCollection? _nameValueCollection;

    public Guid Id { get; set; } = Guid.NewGuid();
    public string Key { get; set; } = string.Empty;
    public string UiName { get; set; } = string.Empty;
    public int WaitAfterExecute { get; set; }
    public List<Paramerter> Paramerters { get; set; } = new();
    public RequestType RequestType { get; set; }
    public IEnumerable<Mob> Mobs { get; set; } = new List<Mob>();

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

    public void SetAction(string? action)
    {
        if (action == null)
        {
            _method = string.Empty;
            return;
        }
        _method = action;
    }

    public void SetOptions(string? option)
    {
        if (option == null)
        {
            _option = string.Empty;
            _nameValueCollection = null;
            return;
        }

        _option = option;
        _nameValueCollection = HttpUtility.ParseQueryString(_option);
    }
    public string GetAction()
    {
        return Method ?? "";
    }

    public string GetParameters(Random random,Target target)
    {

        if (_nameValueCollection == null)
            return string.Empty;

        foreach (var paramerter in Paramerters)
        {
            if (_nameValueCollection.HasKey(paramerter.Key))
                _nameValueCollection.Set(paramerter.Key, paramerter.GetValue(random, target));
        }

        string optionsString ="";

        if (_nameValueCollection.AllKeys.Length > 0)
        {
            var firstKey = _nameValueCollection.AllKeys.First();
            optionsString += firstKey + "=" + _nameValueCollection.Get(firstKey);
            for (int i = 1; i<_nameValueCollection.AllKeys.Length;  i++) 
            {
                var key = _nameValueCollection.GetKey(i);
                optionsString += "&" + key + "=" + _nameValueCollection.Get(key);
            }
        }

        return optionsString;
    }

}