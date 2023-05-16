using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dwar.UI
{
    public static class IEnumerableExtensions
    {
        public static BindingList<T> ToBindingList<T>(this IEnumerable<T> source)
        {
            var bindingList = new BindingList<T>();
            foreach ( var item in source)
            {
                bindingList.Add(item);
            }
            return bindingList;
        }
    }
}
