using Dwar.Dto;
using Dwar.Repositorys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace Dwar.FileIO
{
    public class FightRepository : IFightRepository
    {
        private readonly string _fileName = "fights.json";
        private List<FightDto> _fightDtos= new();
        public FightRepository()
        {
            var fights = Deserialize(_fileName);
            if (fights != null)
                _fightDtos = fights;
        }
        public Fight Create(string name, Guid attackid, IEnumerable<Guid> StartUpActions,Guid after5Fight)
        {
            var fightdto = new FightDto()
            { 
                Id = Guid.NewGuid(),
                Name = name, 
                AttackId = attackid,
                StartUpActions = StartUpActions.ToList() ,
                After5FightID = after5Fight
            };
            _fightDtos.Add(fightdto);
            Serialize();

            return Fight.Convert.ToEntity(fightdto);
        }

        public Fight Get(Guid id)
        {
            return Fight.Convert.ToEntity(_fightDtos.Single(x => x.Id == id));
        }

        public IEnumerable<Fight> GetAll()
        {
            return _fightDtos.Select(x => Fight.Convert.ToEntity(x)).ToList();
        }

        public void Save(Fight entity)
        {
            var dto = Fight.Convert.ToDto(entity);
            var listItem = _fightDtos.FirstOrDefault(item => item.Id == dto.Id);
            if (listItem != null)
            {
                if (listItem != dto)
                {
                    _fightDtos.Remove(listItem);
                    _fightDtos.Add(dto);
                }
            }
            else
            {
                _fightDtos.Add(dto);
            }

            Serialize();
        }

        private void Serialize()
        {
            var json = JsonSerializer.Serialize(_fightDtos);
            File.WriteAllText(_fileName, json);
        }

        private static List<FightDto>? Deserialize(string fileName)
        {
            if (File.Exists(fileName) == false)
                File.Create(fileName).Dispose();
            try
            {
                return JsonSerializer.Deserialize<List<FightDto>>(File.ReadAllText(fileName));
            }
            catch { }
            return new List<FightDto>();
        }

        public void Delete(Guid id)
        {
            var dto = _fightDtos.Single(x=>x.Id == id);
            _fightDtos.Remove(dto);
            Serialize();
        }

        public void Delete(Fight fight)
        {
            var dto = _fightDtos.Single(x => x.Id == fight.Id);
            _fightDtos.Remove(dto);
            Serialize();
        }
    }
}
