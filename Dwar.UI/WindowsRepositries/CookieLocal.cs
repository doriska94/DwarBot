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
        private string _cookies = string.Empty;
        private Dictionary<string, string> _cookiesDict = new Dictionary<string, string>();
        public CookieLocal() 
        {
            
        }
        public CookieContainer Get()
        {
            var coockieContainer = new CookieContainer();
            foreach (var cookie in _cookiesDict)
            {
                coockieContainer.Add(new Cookie(cookie.Key,cookie.Value));
            }
            return coockieContainer;
        }
        public void Set( string value)
        {
            _cookies = value;

            var keysAndValues = value.Trim().Split(";");

            foreach (var key in keysAndValues)
            {
                var item = key.Trim().Split("=");
                _cookiesDict.Add(item[0], item[1]);
            }

        }
    }
}
