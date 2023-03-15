using System;
using System.Threading.Tasks;

namespace HS.Log.Logger
{
    public class LoggerConsole : ILogger
    {
        public bool EnableColor { get; set; } = false;
        public bool EnableBackColor { get; set; } = false;

        public override void Write(LogData Data)
        {
            string message = Data.ToString();
            if(EnableColor)
            {
                if(Data.Level == LogLevel.CRITICAL)
                {
                    if (EnableBackColor)
                    {
                        Console.BackgroundColor = ConsoleColor.Red;
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else Console.ForegroundColor = ConsoleColor.Red;
                }
                else if(Data.Level == LogLevel.ERROR)
                {
                    if (EnableBackColor)
                    {
                        Console.BackgroundColor = ConsoleColor.DarkRed;
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else Console.ForegroundColor = ConsoleColor.DarkRed;
                }
                else if(Data.Level == LogLevel.WARN)
                {
                    if (EnableBackColor)
                    {
                        Console.BackgroundColor = ConsoleColor.DarkYellow;
                        Console.ForegroundColor = ConsoleColor.Black;
                    }
                    else Console.ForegroundColor = ConsoleColor.DarkYellow;
                }
                else if(Data.Level == LogLevel.DEBUG)
                {
                    if (EnableBackColor)
                    {
                        Console.BackgroundColor = ConsoleColor.DarkGreen;
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else Console.ForegroundColor = ConsoleColor.DarkGreen;
                }
                else if(Data.Level == LogLevel.TRACE)
                {
                    if (EnableBackColor)
                    {
                        Console.BackgroundColor = ConsoleColor.DarkGray;
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else Console.ForegroundColor = ConsoleColor.DarkGray;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                }
                Console.WriteLine(message);
                Console.ResetColor();
            }
            else Console.WriteLine(message);
        }
        public override Task WriteAsync(LogData Data) { Write(Data); return Task.CompletedTask; }

        public override void Dispose() { }
    }
}
