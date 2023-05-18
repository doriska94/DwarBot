using System.Net;

namespace Dwar.Http;

public interface ICookie
{
    string GetToString();
    CookieContainer Get();
    void Set(string value);
}
