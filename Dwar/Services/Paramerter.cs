namespace Dwar.Services;

public class Paramerter
{
    public string Key { get; }
    public string Value { get; }
    public bool IsRandom { get; }

    public Paramerter(string key, string value, bool isRandom)
    {
        Key = key;
        Value = value;
        IsRandom = isRandom;
    }


}
