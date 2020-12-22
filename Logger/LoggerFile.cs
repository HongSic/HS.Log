using HS.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HS.Log.Logger
{
    public class LoggerFile : ILogger
    {
        internal static Queue<LogData> Log = new Queue<LogData>();

        private readonly Thread th_file = null;
        private readonly ThreadStart th_file_start = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Directory">로그 작성 디렉터리 (절대경로)</param>
        /// <param name="Buffer">로그 최대 버퍼</param>
        /// <param name="Split">로그 파일 분할 방법</param>
        public LoggerFile(string Directory = null, uint Buffer = 30, LogSplit Split = LogSplit.DATE)
        {
            this.LogDirectory = string.IsNullOrWhiteSpace(Directory) ? StringUtils.GetExcutePath() + "\\Logs" : Directory;
            this.Buffer = Buffer;
            this.Split = Split;

            th_file_start = new ThreadStart(LOOP);
            th_file = new Thread(th_file_start);
            th_file.Start();
            //try { if (th_file != null) th_file.Abort(); } catch (Exception ex) { Console.WriteLine(new LogData(ex, "LoggerFile::Init_Thread_File() 오류발생!!")); }
            //try { th_file = new Thread(th_file_start); th_file.Start(); } catch (Exception ex) { Console.WriteLine(new LogData(ex, "LoggerFile::Init_Thread_File() 오류발생!!")); }
        }

        #region Properties
        private bool _Writing = true;
        private uint _Buffer = 30;

        /// <summary>
        /// 로그 출력(작성) 여부
        /// </summary>
        public bool Writing { get { return _Writing; } set { _Writing = value; } }
        /// <summary>
        /// 로그 최대 버퍼
        /// </summary>
        public uint Buffer { get { return _Buffer; } set { _Buffer = Math.Max(1, value); } }
        /// <summary>
        /// 로그 파일 분할 방법
        /// </summary>
        public LogSplit Split { get; set; }
        /// <summary>
        /// 로그 디렉터리 절대 경로
        /// </summary>
        public string LogDirectory { get; set; }
        #endregion

        #region Implement
        public override void Write(LogData Data) => Log.Enqueue(Data);
        public override Task WriteAsync(LogData Data)
        {
            Write(Data);
            return Task.CompletedTask;
        }
        #endregion

        #region Override
        public override void Dispose()
        {
            try { if (th_file != null) th_file.Abort(); } catch (Exception ex) { Console.WriteLine(new LogData(ex, "LoggerFile::Dispose() 오류발생!!")); }
            try { if (fs != null) fs.Close(); } catch { }
        }
        ~LoggerFile() { Dispose(); }
        #endregion

        #region Loop
        FileStream fs = null;

        private void LOOP()
        {
            string path_bf = null;
            StreamWriter sw = null;
            try
            {
                while (true)
                {
                    while (Log.Count >= Buffer)
                    {
                        uint cnt = (uint)Log.Count - Buffer;
                        while (cnt-- >= 0)
                        {
                            var data = Log.Dequeue();
                            string path = GetLogPath(data.Module);
                            if (path != path_bf)
                            {
                                try { if (fs != null) fs.Close(); } catch { }
                                try { if (sw != null) sw.Close(); } catch { }
                                try
                                {
                                    fs = new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.Read);
                                    sw = new StreamWriter(fs);
                                    path_bf = path;
                                }
                                catch (Exception ex) { Console.WriteLine(new LogData(ex)); Thread.Sleep(1000); cnt++; continue; }
                            }

                            try { sw.WriteLine(data.ToString()); }
                            catch (Exception ex) { Console.WriteLine(new LogData(ex)); Thread.Sleep(1000); cnt++; continue; }
                        }
                        sw.Flush();
                        fs.Flush();
                    }
                    Thread.Sleep(1);
                }
            }
            catch (Exception ex) { Console.WriteLine(new LogData(ex)); }
        }
        #endregion

        #region Utils
        /// <summary>
        /// 로그 파일 경로 가져오기
        /// </summary>
        private string GetLogPath(string AssemblyName = null)
        {
            if (!Directory.Exists(LogDirectory)) Directory.CreateDirectory(LogDirectory);

            if (Split == LogSplit.DATE) return StringUtils.PathMaker(LogDirectory, DateTime.Now.DatetimeToString(false) + ".log");
            else if (Split == LogSplit.ASSEMBLY) return StringUtils.PathMaker(LogDirectory, AssemblyName + ".log");
            else return StringUtils.PathMaker(LogDirectory, "Log.log");
        }
        #endregion

        #region Enums
        public enum LogSplit
        {
            NONE,
            DATE,
            ASSEMBLY
        }
        #endregion
    }
}