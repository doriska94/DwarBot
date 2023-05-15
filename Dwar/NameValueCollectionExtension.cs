using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dwar
{
    public static class NameValueCollectionExtension
    {
        public static bool HasKey(this NameValueCollection nameValueCollection, string key)
        {
            foreach (var nameKey in nameValueCollection.AllKeys)
            {
                if((nameKey == null && key == null) || (nameKey != null && nameKey == key))
                    return true;
            }
            return false;
        }
    }
}
