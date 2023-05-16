using Dwar.Repositorys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Dwar.FileIO
{
    public class BotRepository : IBotRepository
    {
        private readonly string _fileName = "bots.json";
        private List<Bot> _bots = new();
        public BotRepository()
        {
            var bots = Deserialize(_fileName);
            if (bots != null)
                _bots = bots;
        }
        public Bot Create()
        {
            var bot = new Bot()
            {
                Id= Guid.NewGuid(),
            };
            _bots.Add(bot);
            return bot;
        }
        public void Delete(Bot bot)
        {
            if(_bots.Contains(bot))
                _bots.Remove(bot);
            else
            {
                var botList = GetById(bot.Id);
                _bots.Remove(botList);
            }

            Serialize();
        }

        public void Delete(Guid id)
        {
            var bot = GetById(id);
            Delete(bot);
        }

        public IEnumerable<Bot> GetAll()
        {
            return _bots;
        }

        public Bot GetById(Guid id)
        {
            return _bots.Single(x=> x.Id == id);
        }

        public void Save(Bot bot)
        {
            if(_bots.Contains(bot) == false)
            {
               var listBot = GetById(bot.Id);
                _bots.Remove(listBot);
                _bots.Add(bot);
            }

            Serialize();
        }

        private void Serialize()
        {
            var json = JsonSerializer.Serialize(_bots);
            File.WriteAllText(_fileName, json);
        }

        private static List<Bot>? Deserialize(string fileName)
        {
            if (File.Exists(fileName) == false)
                File.Create(fileName).Dispose();
            try
            {
                return JsonSerializer.Deserialize<List<Bot>>(File.ReadAllText(fileName));
            }
            catch { }
            return null;
        }
    }
}
