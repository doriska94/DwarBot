

namespace Dwar.Dto
{

    public class FightDto
    {
        public Guid Id { get; set; } = Guid.Empty;
        public string Name { get; set; } = String.Empty;
        public Guid AttackId { get; set; } = Guid.Empty;
        public List<Guid> StartUpActions { get; set; } = new();
    }

}
