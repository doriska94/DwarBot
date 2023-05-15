namespace Dwar;

public class Target
{

    public int Id { get; }
    public int FightId { get; }
    public string Name { get; }

    public Target(int id, string name, int fightId)
    {
        Id = id;
        Name = name;
        FightId = fightId;
    }


}
