using Dwar.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Dwar.FileIO
{
    public class Logger : ILog
    {
        private const string FileName = "log.txt";

        public Logger()
        {
            if (File.GetCreationTime(FileName).AddDays(7) > DateTime.Now)
            {
                File.Delete(FileName);
            }
            if(File.Exists(FileName) == false)
                File.Create(FileName).Dispose();

        }
        public async Task Write(string name, string message)
        {
            string date = "[" + DateTime.Now.ToString() + "] ";
            await File.AppendAllTextAsync(FileName, date +name + message,CancellationToken.None);
        }

        public async Task Write(Exception exception)
        {
            string date = "[" + DateTime.Now.ToString() + "] ";
            await File.AppendAllTextAsync(FileName, date + exception.Message + exception.StackTrace);
        }
    }
}
