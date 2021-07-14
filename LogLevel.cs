using System;

namespace HS.Log
{
    [Flags]
    public enum LogLevel : int
    {
        ALL = INFO | DEBUG | WARN | ERROR | CRITICAL,
        INFO = 2,
        DEBUG = 4,
        WARN = 8,
        ERROR = 16,
        CRITICAL = 32
    }
}
