using System;
using System.Reflection;
using System.Text;

namespace HS.Log
{
    public class LogData
    {
        private LogData() { Module = Assembly.GetCallingAssembly().CodeBase; Timestamp = DateTime.Now; }

        /// <summary>
        /// 로그 데이터 생성
        /// </summary>
        /// <param name="Level">로그 레벨</param>
        /// <param name="Message">메세지</param>
        public LogData(LogLevel Level, string Message) : this()
        { this.Level = Level; this.Message = Message; }
        /// <summary>
        /// 로그 데이터 생성
        /// </summary>
        /// <param name="Exception">예외</param>
        /// <param name="Level">로그 레벨</param>
        public LogData(Exception Exception, LogLevel Level = LogLevel.ERROR) : this()
        { this.Exception = Exception; this.Level = Level; this.Message = Exception.Message; }
        /// <summary>
        /// 로그 데이터 생성
        /// </summary>
        /// <param name="Exception">예외</param>
        /// <param name="Message">메세지 (null 이면 예외 문자열로 설정)</param>
        /// <param name="Level">로그 레벨</param>
        public LogData(Exception Exception, string Message, LogLevel Level = LogLevel.ERROR) : this()
        { this.Exception = Exception; this.Level = Level; this.Message = Message; }

        public LogLevel Level { get; set; }
        public string Message { get; set; }
        public string Module { get; set; }
        public Exception Exception { get; set; }
        public DateTime Timestamp { get; set; }

        /// <summary>
        ///  로그 
        /// </summary>
        /// <returns>어셈블리: [YYYY-MM-DD hh:mm:ss.aaa] [로그레벨] 메세지</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(Module);
            sb.AppendFormat(": [{0}] [", Timestamp.DatetimeToString(true));

            if (Level == LogLevel.DEBUG || Level == LogLevel.ERROR) sb.Append(Level.ToString()).Append("] ");//.Append("   ] ");
            else if (Level == LogLevel.WARN || Level == LogLevel.INFO) sb.Append(Level.ToString()).Append(" ] ");//.Append("    ] ");
            else sb.Append(Level.ToString()).Append("] ");

            if(Message != null && Exception != null) sb.AppendFormat("{0} [{1}]", Message, Exception.Message);
            else sb.Append(Exception == null ? Message : Exception.Message);

            return sb.ToString();
        }
    }
}
