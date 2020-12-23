using System;
using System.Threading.Tasks;

namespace HS.Log.Logger
{
    public class LoggerConsole : ILogger
    {
        public override void Write(LogData Data) { Console.WriteLine(Data.ToString()); }
        public override Task WriteAsync(LogData Data) { Console.WriteLine(Data.ToString()); return Task.CompletedTask; }

        public override void Dispose() { }
    }
}
