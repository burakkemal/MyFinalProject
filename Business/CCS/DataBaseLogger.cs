using System;

namespace Business.CCS
{
    public class DataBaseLogger : ILogger
    {
        public void log()
        {
            Console.WriteLine("Veritabanına Loglandı");
        }
    }
}
