using System.Reflection;
using System.Threading.Tasks;

namespace HS.Log.Logger
{
    /// <summary>
    /// Windows 이벤트 뷰어에 로그 기록
    /// </summary>
    public class LoggerWindowsEvent : ILogger
    {
        public string Name { get; set; }
        
        public LoggerWindowsEvent(string Name = null)
        {
            this.Name = Name;
        }

        public override Task WriteAsync(LogData Data)
        {
            Write(Data);
            return Task.CompletedTask;
        }

        public override void Write(LogData Data)
        {
            string message = Data.ToString();

            try
            {
                // 기본 로그 소스 설정
                string source = string.IsNullOrEmpty(Name) ? Assembly.GetEntryAssembly().GetName().Name : Name;
                string logName = "Application";

                // 로그 소스가 없으면 생성
                if (!System.Diagnostics.EventLog.SourceExists(source))
                {
                    System.Diagnostics.EventLog.CreateEventSource(source, logName);
                }

                // 로그 작성
                System.Diagnostics.EventLog.WriteEntry(
                    source,
                    message,
                    System.Diagnostics.EventLogEntryType.Information
                );
            }
            catch
            {
                // EventLog 작성 실패 시 무시 (권한 문제 등)
            }
        }

        public override void Dispose()
        {
            // 특별한 자원 관리 필요 없음
        }
    }
}