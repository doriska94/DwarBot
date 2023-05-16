using Dwar.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dwar.UI.Model
{
    public class ActionModel
    {
        public Guid Id { get; set; }
        public string Key { get; set;} = string.Empty;
        public string UiName { get; set; } = string.Empty;
        public string? Method { get; set; }
        public string? Option { get; set; }
        public int WaitAfterExecute { get; set; }
        public RequestType RequestType { get; set; }
        public BindingList<Paramerter> Paramerters { get; set; } = new();
             
        public static ActionModel Create(Action entity)
        {
            return new ActionModel
            {
                Id = entity.Id,
                Key = entity.Key,
                UiName = entity.UiName,
                Method = entity.Method,
                Option = entity.Option,
                RequestType = entity.RequestType,
                WaitAfterExecute = entity.WaitAfterExecute,
                Paramerters = entity.Paramerters.ToBindingList()
            };
        }
    }
}
