using Dwar.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Dwar.UI.Model;

public class ActionModel
{
    public Guid Id { get; set; }
    public string Key { get; set;} = string.Empty;
    public string UiName { get; set; } = string.Empty;
    public string? Method { get; set; }
    public string? Option { get; set; }
    public int WaitAfterExecute { get; set; }
    public RequestType RequestType { get; set; }
    public BindingList<Mob> Mobs { get; set;} = new BindingList<Mob>();
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
            Mobs = entity.Mobs.ToBindingList(),
            Paramerters = entity.Paramerters.ToBindingList()
        };
    }
}
