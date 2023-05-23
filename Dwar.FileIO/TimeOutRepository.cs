using Dwar.Dto;
using Dwar.Repositorys;
using Dwar.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Dwar.FileIO
{
    public class TimeOutRepository : ITimeOutRepository
    {
        private TimeOut _timeout = null!;
        private const string _fileName = "timeout.json";

        public TimeOutRepository()
        {
            var timeOut = Deserialize(_fileName);
            if (timeOut == null)
                timeOut = new TimeOut();
            _timeout = timeOut;
            _timeout.TimeOutMinuts = 5;
        }
        public ITimeOutService Get()
        {
            return _timeout;
        }

        public void Save(TimeOut timeOut)
        {
            _timeout = timeOut;
            Serialize();
        }
        private void Serialize()
        {
            var json = JsonSerializer.Serialize(_timeout);
            File.WriteAllText(_fileName, json);
        }

        private static TimeOut Deserialize(string fileName)
        {
            if (File.Exists(fileName) == false)
                File.Create(fileName).Dispose();
            try
            {
                return JsonSerializer.Deserialize<TimeOut>(File.ReadAllText(fileName))!;
            }
            catch 
            { 
            }
            return null!;
        }
    }
}
