using HS.Log.Logger;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HS.Log
{
    public delegate bool LogWritingEventHandler(LogData data);
    public delegate bool LogWritingMatchEventHandler(LogData data);

    public static class LogHS
    {
        public static event LogWritingEventHandler LogWriting;
        public static event LogWritingMatchEventHandler LogWritingMatch;

        public static List<ILogger> Logger { get; private set; }
        public static bool Inited { get { return Logger != null && Logger.Count > 0; } }
        public static LogLevel Level { get; set; }

        public static bool UseQueue { get; private set; }
        public static int MaxQueue { get; set; } = 1000;
        public static int QueueCount { get { return LogQueue.Count; } }

        private static Queue<LogData> LogQueue;
        private static Thread QueueThread;

        /// <summary>
        /// 로거 초기화
        /// </summary>
        /// <param name="Level">로그 레벨 설정</param>
        /// <param name="Logger"></param>
        public static void Init(bool UseQueue = false, LogLevel Level = LogLevel.ALL, params ILogger[] Logger)
        {
            LogHS.Level = Level;
            LogHS.Logger = new List<ILogger>(Logger);

            if(UseQueue)
            {
                LogQueue = new Queue<LogData>(MaxQueue);
                QueueThread = new Thread(new ThreadStart(() =>
                {
                    while (true)
                    {
                        if (LogQueue.Count > 0) _Write(LogQueue.Dequeue());
                        Thread.Sleep(5);
                    }
                }));
                QueueThread.Start();
            }
        }

        public static void Dispose()
        {
            QueueThread?.Interrupt();
        }

        public static void AddLoger(ILogger Logger) { if(Logger != null) LogHS.Logger.Add(Logger); }
        public static void RemoveLogger(int Index)
        {
            if (Logger != null)
            {
                LogHS.Logger.RemoveAt(Index);
                Logger[Index].Dispose();
            }
        }

        public static void Reset()
        {
            Level = 0;
            if (Logger != null)
            {
                for(int i = Logger.Count - 1; i >= 0; i--)
                {
                    try { Logger[i].Dispose(); } catch { }
                    Logger.RemoveAt(i);
                }
            }
        }

        #region Write
        private static void _Write(LogData data)
        {
            bool Continue = true;
            try { if (LogWriting != null) Continue = LogWriting.Invoke(data); } catch { }
            if (Continue && Logger != null)
            {
                try { if (LogWritingMatch != null) Continue = LogWritingMatch.Invoke(data); } catch { }
                if (Continue)
                    for (int i = 0; i < LogHS.Logger.Count; i++)
                        if (data.LevelMatch(Level)) Logger[i].Write(data);
            }
        }
        public static void Write(LogData data) 
        {
            if (UseQueue) { if (QueueCount < MaxQueue) LogQueue.Enqueue(data); }
            else _Write(data);
        }
        private static async Task _WriteAsync(LogData data)
        {
            bool Continue = true;
            try { if (LogWriting != null) Continue = LogWriting.Invoke(data); } catch { }
            if (Continue && data.LevelMatch(Level))
            {
                try { if (LogWritingMatch != null) Continue = LogWritingMatch.Invoke(data); } catch { }
                if (Continue)
                    if (Logger != null)
                        for (int i = 0; i < Logger.Count; i++)
                            await Logger[i].WriteAsync(data);
            }
        }
        public static async Task WriteAsync(LogData data)
        {
            if (UseQueue) { if (QueueCount < MaxQueue) LogQueue.Enqueue(data); }
            else await _WriteAsync(data);
        }
        #endregion

        #region Utils
        /// <summary>
        /// 로그 데이터 레벨이랑 현재 레벨이랑 일치하는지 확인합니다
        /// </summary>
        /// <param name="Data"></param>
        /// <param name="Level"></param>
        /// <returns></returns>
        public static bool LevelMatch(this LogData Data, LogLevel Level) { return (Level & Data.Level) == Data.Level; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <param name="IncludeTime">True 면 시간포함 False 면 포함하지 않음</param>
        /// <param name="MilliSecond">True 면 밀리초포함 False 면 포함하지 않음 (IncludeTime 이 True 여야 함)</param>
        /// <returns>YYYY-MM-DD (hh:mm:ss.aaa)</returns>
        public static string DatetimeToString(this DateTime date, bool IncludeTime, bool MilliSecond = true)
        {
            StringBuilder sb = new StringBuilder(IncludeTime ? 19 : 10);
            sb.Append(date.Year).Append("-")
            .Append(date.Month.ToString("00")).Append("-")
            .Append(date.Day.ToString("00"));

            if (IncludeTime)
            {
                sb.Append(" ")
                .Append(date.Hour.ToString("00")).Append(":")
                .Append(date.Minute.ToString("00")).Append(":")
                .Append(date.Second.ToString("00")).Append(".");
                if(MilliSecond) sb.Append(date.Millisecond.ToString("000"));
            }
            return sb.ToString();
        }
        #endregion
    }
}
