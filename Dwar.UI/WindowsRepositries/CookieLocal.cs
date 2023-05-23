using Dwar.Http;
using System.Collections.Generic;
using System.Net;

namespace Dwar.UI.WindowsRepositries;

public class CookieLocal : ICookie
{
    private string _cookie = null!;
    private Dictionary<string, string> _cookiesDict = new Dictionary<string, string>();
    private CookieContainer _cookieContainer = null!;

    private IDomain _domain;
    public CookieLocal(IDomain domain) 
    {
        _domain = domain;
    }
    public CookieContainer Get()
    {
        return _cookieContainer;
    }

    public string GetToString()
    {
        return "Cookie:" + _cookie;
    }

    public void Set( string value)
    {
        _cookie = value;

        var keysAndValues = value.Trim().Split(";");
        _cookiesDict = new Dictionary<string, string>();
        foreach (var key in keysAndValues)
        {
            var item = key.Trim().Split("=");
            _cookiesDict.Add(item[0], item[1]);
        }
        _cookieContainer = new CookieContainer();

        string[] cookies = value.Split(';');
        foreach (string cookie in cookies)
            _cookieContainer.SetCookies(_domain.GetBaseUri(), cookie);
        

    }
}
