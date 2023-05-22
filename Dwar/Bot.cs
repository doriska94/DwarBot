using Dwar.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Dwar
{
    public class Bot
    {
        private Fight? _fight;

        public Guid Id { get; set; } = Guid.Empty;
        public string Name { get; set; } = String.Empty;
        public bool WaitHp { get; set; }
        public int FightCount { get; set; }
        public int FightTime { get; set; }
        public Guid FightId { get; set; } = Guid.Empty;

        [JsonIgnore]
        public Fight? Fight 
        { 
            get => _fight; 
            set 
            { 
                _fight = value; 
                if (value != null) 
                    FightId = value.Id; 
                else 
                    FightId = Guid.Empty; 
            } 
        }
        
        public SequenceType Type { get; set; }
        
        public Bot()
        {

        }
    }
    public enum SequenceType
    {
        Fight,
        Farm,
        Divnoe,
        Execute
    }
}
