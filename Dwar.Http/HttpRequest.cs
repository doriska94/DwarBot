using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using Dwar.Factories;
using Dwar.Repositorys;
using Dwar.Services;

namespace Dwar.Http;

public class HttpRequest: ISendRequest, IGetRequest, ITargetRepository
{
    private ICookie _cookie;
    private IDomain _domain;
    private IActionRepository _actionRepository;
    public HttpRequest(ICookie cookie, IDomain domain, IActionRepository actionRepository)
    {
        _cookie = cookie;
        _domain = domain;
        _actionRepository = actionRepository;
    }

    public async Task SendAsync(string action, string paramater)
    {
        using var handler = new HttpClientHandler() { CookieContainer = _cookie.Get() };
        using var client = new HttpClient(handler) { BaseAddress = _domain.GetBaseUri() };
        var result = await client.PostAsync(_domain.GetBaseUri(), new StringContent(paramater));

    }

    public async Task<string> GetAsync(string action, string parameter)
    {
        var getUri = new Uri(_domain.GetBaseUri(), action + "?" + parameter);
        return await GetAsync(getUri);
    }

    private async Task<string> GetAsync(Uri getUri)
    {
        using var handler = new HttpClientHandler() { CookieContainer = _cookie.Get() };
        using var client = new HttpClient(handler) { BaseAddress = _domain.GetBaseUri() };
        var result = await client.GetAsync(getUri);
        return await result.Content.ReadAsStringAsync();
    }

    public async Task<IEnumerable<Target>> GetTargetsAsync()
    {
        var action = _actionRepository.GetActionGetTargets();    
        
        var uri =  new Uri(_domain.GetBaseUri(), action.GetAction());
        var result = await GetAsync(uri);
        return TargetFactory.Parse(result);
    }
}