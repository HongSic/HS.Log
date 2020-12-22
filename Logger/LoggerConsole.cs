using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HS.Log.Logger
{
    public class LoggerConsole : ILogger
    {
        public override void Dispose()
        {
            throw new NotImplementedException();
        }

        public override void Write(LogData Data)
        {
            throw new NotImplementedException();
        }

        public override Task WriteAsync(LogData Data)
        {
            throw new NotImplementedException();
        }
    }
}
