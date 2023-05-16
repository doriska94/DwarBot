using System.Text.Json.Serialization;

namespace Dwar.Services;

public class Paramerter
{
    public string Key { get; set; }
    public string Value { get; set; }
    public bool IsRandom { get; set; }
    public bool OnMobName { get; set; }
    public bool IsGuid { get; set; }
    [JsonConstructor]
    public Paramerter(string key, string value, bool isRandom)
    {
        Key = key;
        Value = value;
        IsRandom = isRandom;
    }
    public string GetValue(Random random,Target target)
    {
        if(IsRandom)
            return Value + random.Next().ToString();
        
        if(OnMobName) 
            return target?.Id.ToString()??"";
        if(IsGuid)
            return Guid.NewGuid().ToString().Replace("-","");

        return Value;
    }


}
