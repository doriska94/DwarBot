namespace Dwar;

public interface IDomain
{
    Uri GetBaseUri();
    string GetUrl();
    void SetUrl(string url);
}
