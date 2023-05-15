namespace Dwar.Services;

public class Paramerter
{
    public string Key { get; }
    public string Value { get; }
    public bool IsRandom { get; }
    private Random _random;
    public Paramerter(string key, string value, bool isRandom,Random random)
    {
        _random = random;
        Key = key;
        Value = value;
        IsRandom = isRandom;
    }
    public string GetValue()
    {
        if(IsRandom)
            return Value+_random.Next().ToString();
        return Value;
    }


}
