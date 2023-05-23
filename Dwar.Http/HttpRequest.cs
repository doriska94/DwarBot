using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using Dwar.Factories;
using Dwar.Repositorys;
using Dwar.Services;

namespace Dwar.Http;

public class HttpRequest : ISendRequestService, IGetRequest, ITargetRepository, IHttpRequest
{
    private const string ErrorCode = "top.error_close();";
    private const string SuccesCode = "top.close();;";

    private ICookie _cookie;
    private IDomain _domain;
    private ILogService _log;
    private IActionRepository _actionRepository;

    public HttpRequest(ICookie cookie, IDomain domain, IActionRepository actionRepository, ILogService log)
    {
        _cookie = cookie;
        _domain = domain;
        _actionRepository = actionRepository;
        _log = log;
    }

    public bool Send(string action, string paramater)
    {
        
        var result =  Post(action, paramater);
        return !result.Contains(ErrorCode);
    }

#pragma warning disable SYSLIB0014 // Тип или член устарел
    public string Post(string url, string strPost, bool refer = false)
    {
        try
        {
            string coockies = _cookie.GetToString();

            String result = "";
            StreamWriter myWriter = null!;
            var uri = new Uri(_domain.GetBaseUri(), url);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri.AbsoluteUri);
            request.Host = _domain.GetBaseUri().Host;
            request.ContentLength = strPost.Length;
            request.Headers.Add("Cache-Control: max-age=0");
            request.Headers.Add("Origin: " + _domain.GetBaseUri().AbsoluteUri);
            request.Headers.Add("Upgrade-Insecure-Requests: 1");
            request.ContentType = "application/x-www-form-urlencoded";
            if (refer)
                request.Referer = _domain.GetBaseUri().AbsoluteUri+ "/npc.php?f_id=10&npc_id=197&global_npc=0&quest_id=561&point_id=11052&197211cb12a379c15aca30b313bd870f";
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/80.0.3987.163 Safari/537.36";
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9";
            request.Headers.Add("Accept-Encoding: gzip, deflate");
            request.Headers.Add("Accept-Language: ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7");
            request.Headers.Add(coockies);
            request.AutomaticDecompression = DecompressionMethods.GZip;
            request.Method = "POST";
            try
            {
                myWriter = new StreamWriter(request.GetRequestStream());
                myWriter.Write(strPost);
            }
            catch (Exception e)
            {
                _log.Write(e);
                return e.Message;
            }
            finally
            {
                myWriter.Close();
            }

            HttpWebResponse objResponse = (HttpWebResponse)request.GetResponse();
            using (StreamReader sr =
               new StreamReader(objResponse.GetResponseStream()))
            {
                result = sr.ReadToEnd();
                sr.Close();
            }
            _log.Write("POST: " + url + " \n" + strPost, result);
            return result;
        }
        catch (Exception e)
        {
            _log.Write(e);
            return ErrorCode;
        }
    }
    public string GetRequest(string url,string refer = null)
    {
        string coockies = _cookie.GetToString();
        try
        {
            string html = string.Empty;


            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            request.Method = "GET";
            request.Headers.Add("Upgrade-Insecure-Requests: 1");
            request.Headers.Add("Accept-Encoding: gzip, deflate");
            //request.Credentials = CredentialCache.DefaultCredentials;
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/113.0.0.0 Safari/537.36";
            request.Headers.Add("Accept-Language: ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7");
            if (refer != null)
                request.Referer = refer;
            request.Headers.Add(coockies);

            request.AutomaticDecompression = DecompressionMethods.GZip;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                html = reader.ReadToEnd();
            }
            _log.Write("GET: " + url, html);
            return html;
        }
        catch (Exception e)
        {
            _log.Write(e);
            return ErrorCode;
        }
    }
#pragma warning restore SYSLIB0014 // Тип или член устарел
    public bool Get(string action, string parameter)
    {
        var getUri = new Uri(_domain.GetBaseUri(), action + "?" + parameter);
        var result = GetRequest(getUri.AbsoluteUri);
        return !result.Contains(ErrorCode);
    }

    public IEnumerable<Target> GetTargets()
    {
        var action = _actionRepository.GetActionGetTargets();

        var uri = new Uri(_domain.GetBaseUri(), action.GetAction());
        var result = GetRequest(uri.AbsoluteUri);
        return TargetFactory.ParseDistinct(result);
    }

    public Target? GetFreeTargetsOrDefault(string? name)
    {
        if(string.IsNullOrEmpty(name))
                return null;

        var action = _actionRepository.GetActionGetTargets();

        var uri = new Uri(_domain.GetBaseUri(), action.GetAction());
        
        var result = GetRequest(uri.AbsoluteUri);
        
        var targets = TargetFactory.Parse(result);

        var target =  targets.FirstOrDefault(x => x.Name == name && x.FightId == 0);
       
        return target;

    }

    public Target? GetById(Target target)
    {
        if (target.Id == 0)
            return null;

        var action = _actionRepository.GetActionGetTargets();

        var uri = new Uri(_domain.GetBaseUri(), action.GetAction());
        var result = GetRequest(uri.AbsoluteUri);
        var targets = TargetFactory.Parse(result);
        return targets.FirstOrDefault(x => x.Id == target.Id);
    }

    public Target? GetByAnthorId(Target target)
    {
        if (target.Id == 0)
            return null;

        var action = _actionRepository.GetActionGetTargets();

        var uri = new Uri(_domain.GetBaseUri(), action.GetAction());
        var result = GetRequest(uri.AbsoluteUri);
        var targets = TargetFactory.Parse(result);
        return targets.FirstOrDefault(x => x.Id != target.Id && x.Name == target.Name);
    }    
}