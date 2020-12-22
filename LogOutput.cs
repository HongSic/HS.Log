using System;

namespace HS.Log
{
    [Flags]
    public enum LogOutput : int
    {
        CONSOLE = 2,
        FILE = 4,
        //WEBSOCKET = 8
    }
}
