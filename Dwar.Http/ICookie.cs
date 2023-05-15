using System.Net;

namespace Dwar.Http;

public interface ICookie
{
    CookieContainer Get();
}
