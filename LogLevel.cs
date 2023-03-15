using System;

namespace HS.Log
{
    [Flags]
    public enum LogLevel : int
    {
        TRACE = 1,
        DEBUG = 2,
        INFO = 4,
        WARN = 8,
        ERROR = 16,
        CRITICAL = 32,
        ALL = TRACE | DEBUG | INFO | WARN | ERROR | CRITICAL,
    }
}
