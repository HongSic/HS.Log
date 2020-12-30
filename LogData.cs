using System;
using System.Reflection;
using System.Text;

namespace HS.Log
{
    public class LogData
    {
        private LogData() {  Timestamp = DateTime.Now; }

        /// <summary>
        /// 로그 데이터 생성
        /// </summary>
        /// <param name="Level">로그 레벨</param>
        /// <param name="Message">메세지</param>
        public LogData(LogLevel Level, string Message) : this()
        { 
            this.Level = Level; 
            this.Message = Message; 
            this.Tag = Assembly.GetCallingAssembly().GetName().Name;
        }
        /// <summary>
        /// 로그 데이터 생성
        /// </summary>
        /// <param name="Tag">태그</param>
        /// <param name="Level">로그 레벨</param>
        /// <param name="Message">메세지</param>
        public LogData(string Tag, LogLevel Level, string Message) : this()
        {
            this.Tag = Tag;
            this.Level = Level; 
            this.Message = Message; 
        }
        /// <summary>
        /// 로그 데이터 생성
        /// </summary>
        /// <param name="Exception">예외</param>
        /// <param name="Level">로그 레벨</param>
        public LogData(Exception Exception, LogLevel Level = LogLevel.ERROR) : this()
        { 
            this.Exception = Exception; 
            this.Level = Level; 
            this.Tag = Assembly.GetCallingAssembly().GetName().Name;
        }
        /// <summary>
        /// 로그 데이터 생성
        /// </summary>
        /// <param name="Tag">태그</param>
        /// <param name="Exception">예외</param>
        /// <param name="Level">로그 레벨</param>
        public LogData(string Tag, Exception Exception, LogLevel Level = LogLevel.ERROR) : this()
        {
            this.Exception = Exception; 
            this.Level = Level; 
            this.Tag = Tag;
        }
        /// <summary>
        /// 로그 데이터 생성
        /// </summary>
        /// <param name="Exception">예외</param>
        /// <param name="Message">메세지 (null 이면 예외 문자열로 설정)</param>
        /// <param name="Level">로그 레벨</param>
        public LogData(Exception Exception, string Message, LogLevel Level = LogLevel.ERROR) : this()
        {
            this.Exception = Exception; 
            this.Level = Level;
            this.Message = Message;
            this.Tag = Assembly.GetCallingAssembly().GetName().Name;
        }
        /// <summary>
        /// 로그 데이터 생성
        /// </summary>
        /// <param name="Tag">태그</param>
        /// <param name="Exception">예외</param>
        /// <param name="Message">메세지 (null 이면 예외 문자열로 설정)</param>
        /// <param name="Level">로그 레벨</param>
        public LogData(string Tag, Exception Exception, string Message, LogLevel Level = LogLevel.ERROR) : this()
        {
            this.Exception = Exception;
            this.Level = Level;
            this.Message = Message;
            this.Tag = Tag;
        }

        public LogLevel Level { get; private set; }
        public string Message { get; private set; }
        public string Tag { get; private set; }
        public Exception Exception { get; private set; }
        public DateTime Timestamp { get; private set; }

        /// <summary>
        ///  로그 
        /// </summary>
        /// <returns>[YYYY-MM-DD hh:mm:ss.aaa] [로그레벨] [어셈블리]: 메세지</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("[{0}] [", Timestamp.DatetimeToString(true));

            if (Level == LogLevel.DEBUG || Level == LogLevel.ERROR) sb.Append(Level.ToString()).Append("] ");//.Append("   ] ");
            else if (Level == LogLevel.WARN || Level == LogLevel.INFO) sb.Append(Level.ToString()).Append(" ] ");//.Append("    ] ");
            else sb.Append(Level.ToString()).Append("] ");

            if (Tag != null) sb.AppendFormat("[{0}]: ", Tag);

            sb.Append(Message);

            if (Exception != null)
            {
                //if (Message == null)
                //{
                //    sb.Append(Exception.Message).AppendLine();
                //    sb.AppendFormat("  {0}:", Exception.GetType().FullName).AppendLine();
                //}
                //else
                {
                    sb.AppendLine();
                    sb.AppendFormat("  {0}: {1}", Exception.GetType().FullName, Exception.Message).AppendLine();
                }

                string[] str = Exception.StackTrace.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                sb.Append("  ").Append(str[0]);
                for (int i = 1; i < str.Length; i++) sb.AppendLine().AppendFormat("  {0}", str[i]);
            }

            return sb.ToString();
        }
    }
}
