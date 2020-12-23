using HS.Log.Logger;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HS.Log
{
    public static class LogHS
    {
        public static List<ILogger> Logger { get; private set; }
        public static bool Inited { get { return Logger != null && Logger.Count > 0; } }
        public static LogLevel Level { get; set; }

        /// <summary>
        /// 로거 초기화
        /// </summary>
        /// <param name="Level">로그 레벨 설정</param>
        /// <param name="Logger"></param>
        public static void Init(
            LogLevel Level = LogLevel.CRITICAL | LogLevel.DEBUG | LogLevel.ERROR | LogLevel.INFO | LogLevel.WARN, 
            params ILogger[] Logger)
        {
            LogHS.Level = Level;
            LogHS.Logger = new List<ILogger>(Logger);
        }

        public static void AddLoger(ILogger Logger) { if(Logger != null) LogHS.Logger.Add(Logger); }
        public static void RemoveLogger(int Index)
        {
            if (Logger != null)
            {
                var logger = LogHS.Logger[Index];
                LogHS.Logger.RemoveAt(Index);
                logger.Dispose();
            }
        }

        #region Write
        public static void Write(LogData data)
        {
            if (Logger != null)
            {
                for (int i = 0; i < Logger.Count; i++)
                    if (data.LevelMatch(Level)) Logger[i].Write(data);
            }
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
        /// <returns>YYYY-MM-DD (hh:mm:ss.aaa)</returns>
        public static string DatetimeToString(this DateTime date, bool IncludeTime)
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
                .Append(date.Second.ToString("00")).Append(".")
                .Append(date.Millisecond.ToString("000"));
            }
            return sb.ToString();
        }
        #endregion
    }
}
