

namespace Dwar.Dto
{

    public class FightDto
    {
        public Guid Id { get; set; } = Guid.Empty;
        public string Name { get; set; } = String.Empty;
        public Action Attack { get; set; } = null!;
        public List<Action> StartUpActions { get; set; } = null!;
    }

}
