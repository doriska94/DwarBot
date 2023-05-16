using Dwar.Repositorys;
using System.Text.Json;

namespace Dwar.FileIO;

public class ComboRepository : IComboRepository
{
    private readonly string _fileName = "combo.json";
    private Combo _combo = null!;
    public ComboRepository()
    {
        var combo = Deserialize(_fileName);
        if (combo != null)
            _combo = combo;
    }
    public Combo Get()
    {
        _combo ??= new Combo();
        return _combo;
    }

    public void Set(Combo combo)
    {
        _combo = combo;
        Serialize();
    }

    private void Serialize()
    {
        var json = JsonSerializer.Serialize(_combo);
        File.WriteAllText(_fileName, json);
    }

    private static Combo? Deserialize(string fileName)
    {
        if (File.Exists(fileName) == false)
            File.Create(fileName).Dispose();
        try
        {
            return JsonSerializer.Deserialize<Combo>(File.ReadAllText(fileName));
        }
        catch { }
        return null;
    }
}
