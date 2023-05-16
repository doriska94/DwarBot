using Dwar.Http;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Dwar.UI.WindowsRepositries
{
    public class CookieLocal : ICookie
    {
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
        public void Set( string value)
        {

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
}
