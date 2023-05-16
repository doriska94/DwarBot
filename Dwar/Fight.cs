
using Dwar.Dto;

namespace Dwar;

public class Fight
{
    private FightDto _dto; 
    public Guid Id =>_dto.Id;
    public string Name => _dto.Name;
    public Guid AttackId => _dto.AttackId;
    public IEnumerable<Guid> StartUpActions => _dto.StartUpActions;

    private Fight(FightDto fightDto)
    {
        _dto = fightDto;
    }

    public static class FightFactory
    {
        public static Fight Create(Guid id, string Name, Action attack, IEnumerable<Action> startUpActions)
        {
            var dto = new FightDto()
            {
                Id = id,
                Name = Name,
                AttackId = attack.Id,
                StartUpActions = startUpActions.Select(x=>x.Id).ToList()
            };
            return new Fight(dto);
        }
    }
    public static class Convert
    {
        public static FightDto ToDto(Fight fight) => fight._dto;
        public static Fight ToEntity(FightDto fight) => new Fight(fight);
    }
}
