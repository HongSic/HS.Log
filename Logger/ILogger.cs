using HS.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HS.Log.Logger
{
    public abstract class ILogger
    {
        private static readonly Random Random = new Random(20201223);
        private string ID { get; set; }
        public ILogger() { ID = Random.NextString(20); }

        public abstract Task WriteAsync(LogData Data);
        public abstract void Write(LogData Data);

        public abstract void Dispose();

        public virtual bool Equals(ILogger other) { return other != null && other.ID == ID; }
    }
}
