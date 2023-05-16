namespace Dwar;

public class Hp
{
    public Hp(int actual, int maximal)
    {
        Actual = actual;
        Maximal = maximal;
    }

    public int Actual { get; }
    public int Maximal { get; }


    public bool IsFull()
    { 
        return Actual == Maximal; 
    }
}
