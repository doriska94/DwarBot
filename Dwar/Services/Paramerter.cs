using System.Text.Json.Serialization;

namespace Dwar.Services;

public class Paramerter
{
    public string Key { get; }
    public string Value { get; }
    public bool IsRandom { get; }

    [JsonConstructor]
    public Paramerter(string key, string value, bool isRandom)
    {
        Key = key;
        Value = value;
        IsRandom = isRandom;
    }
    public string GetValue(Random random)
    {
        if(IsRandom)
            return Value + random.Next().ToString();
        return Value;
    }


}
