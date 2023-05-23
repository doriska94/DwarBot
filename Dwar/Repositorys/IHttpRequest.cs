namespace Dwar.Repositorys;

public interface IHttpRequest
{
    string Post(string url, string strPost, bool refer = false);
    string GetRequest(string url,string refer = null);
}
