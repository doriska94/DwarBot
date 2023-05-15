
namespace Dwar
{
    public class Domain : IDomain
    {
        public string Url { get; set; } = string.Empty;
        public Domain(string url)
        {
            Url = url;
        }
        public Domain()
        {
            
        }
        public Uri GetBaseUri()
        {
            return new Uri(Url);
        }

        public string GetUrl()
        {
            return Url;
        }

        public void SetUrl(string url)
        {
            if(url == null || string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentNullException("url");
            }
            Url = url;
        }
    }
}
