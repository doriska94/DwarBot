namespace Dwar.Repositorys;

public interface ITargetRepository
{
    IEnumerable<Target> GetTargets();
    Target? GetFreeTargetsOrDefault(string? name);
    Target? GetById(Target target);
    Target? GetByAnthorId(Target target);
}
