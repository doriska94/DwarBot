using Dwar.Dto;
using Dwar.Repositorys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Dwar.FileIO
{
    public class DomainRepositry : IDomainRepositry
    {
        private readonly string _fileName = "domain.json";
        private Domain _domain = null!;
        public DomainRepositry()
        {
            var domain = Deserialize(_fileName);
            if (domain != null)
                _domain = domain;
        }

        public Domain Get()
        {
            _domain??= new Domain("https://dwarlegacy.ru/");

            return _domain;
        }

        public void Set(Domain domain)
        {
            _domain= domain;
            Serialize();
        }
        private void Serialize()
        {
            var json = JsonSerializer.Serialize(_domain);
            File.WriteAllText(_fileName, json);
        }

        private static Domain? Deserialize(string fileName)
        {
            if (File.Exists(fileName) == false)
                File.Create(fileName).Dispose();
            try
            {
                return JsonSerializer.Deserialize<Domain>(File.ReadAllText(fileName));
            }
            catch { }
            return null;
        }
    }
}
