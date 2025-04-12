﻿using System;
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
            this.Message = Exception?.Message;
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
            this.Message = Exception?.Message;
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
        /// 하위 예외까지 모두 출력
        /// </summary>
        public bool ShowInnerException { get; set; } = false; //true

        /// <summary>
        ///  로그 
        /// </summary>
        /// <returns>[YYYY-MM-DD hh:mm:ss.aaa] [로그레벨] [어셈블리]: 메세지</returns>
        public override string ToString() => ToString(false, true);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ShowStacktrace"></param>
        /// <returns></returns>
        public string ToString(bool ShowStacktrace, bool ShowType)
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
                if (ShowInnerException)
                {
                    /*
                    StringBuilder error_type = new StringBuilder();
                    bool error_first = true;
                    Exception exi = ex?.InnerException;
                    if (exi != null)
                    {
                        error_type.Append("(내부: ");
                        do
                        {
                            if (!error_first) error_type.Append(" -> ");
                            error_type.Append(exi.GetType());
                            error_first = false;
                        }
                        while ((exi = exi.InnerException) != null);
                        error_type.Append(")");
                    }

                    if (ex != null)
                    {
                        if (Short) msg += $" ({ex.Message})";
                        else msg += $"{Environment.NewLine}└ 메세지: {ex.Message}{Environment.NewLine}└ 코드: 0x{ex.HResult:X4} ({ex.HResult}) \r\n└ 종류: {ex.GetType()} {error_type}";
                    }
                    */
                }
                else
                {
                    sb.AppendLine();
                    if (ShowType) sb.AppendFormat("  {0}: {1}", Exception.GetType().FullName, Exception.Message);
                    else sb.AppendFormat("  {0}", Exception.Message);

                    if (ShowStacktrace && Exception.StackTrace != null)
                    {
                        sb.AppendLine();
                        string[] str = Exception.StackTrace.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                        sb.Append("  ").Append(str[0]);
                        for (int i = 1; i < str.Length; i++) sb.AppendLine().AppendFormat("  {0}", str[i]);
                    }
                }
            }

            return sb.ToString();
        }
    }
}
