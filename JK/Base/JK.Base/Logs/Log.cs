using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace JK.Base.Logs
{
    public static class Log
    {
        /// 로그 디렉토리 path
        private static string _logRootDir = $"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}Log";
        /// 로그 파일이름
        private static string _fileName = Assembly.GetEntryAssembly().GetName().Name;
        /// 오늘날짜
        private static string _nowDate = DateTime.MinValue.ToString("yyyyMMdd");
        /// 로그 만료일 (일 보관)
        private static int _expireDate = 90;
        // tackTraceLog 사용 여부
        private static bool isUseStackTraceLog = false;
        // Log Level
        private static LogLevel LogLevel = LogLevel.Error;
        /// lock
        private static object _lock = new object();

        /// <summary>
        /// 로그 설정
        /// </summary>
        /// <param name="isUseWriteExceptionEvent">모든 Exception 로그 Write 사용 여부</param>
        /// <param name="expireDate">로그 만료일 (일 보관) 기본 90</param>
        /// <param name="logRootDir">로그 디렉토리 path</param>
        /// <param name="fileName">로그 파일이름</param>
        public static void SetConfig(bool isUseWriteExceptionEvent = false, int expireDate = 90, string logRootDir = "", string fileName = "")
        {
            WriteExceptionEvent(isUseWriteExceptionEvent);

            if (expireDate > 0)
                _expireDate = expireDate;

            if (!string.IsNullOrWhiteSpace(logRootDir))
                _logRootDir = logRootDir;

            if (!string.IsNullOrWhiteSpace(fileName))
                _fileName = fileName;
        }

        /// <summary>
        /// 모든 Exception 로그 Write 사용 
        /// </summary>
        /// <param name="isUseWriteExceptionEvent">모든 Exception 로그 Write 사용 여부</param>
        private static void WriteExceptionEvent(bool isUseWriteExceptionEvent = false)
        {
            if (!isUseStackTraceLog && isUseWriteExceptionEvent)
            {
                isUseStackTraceLog = true;

                AppDomain.CurrentDomain.UnhandledException += (s, e) =>
                {
                    Log.e("UnhandledException", e.ExceptionObject.ToString(), Environment.StackTrace);
                };
                AppDomain.CurrentDomain.FirstChanceException += (s, e) =>
                {
                    Log.e("FirstChanceException", e.Exception.Message, Environment.StackTrace);
                };
                TaskScheduler.UnobservedTaskException += (s, e) =>
                {
                    Log.e("UnobservedTaskException", e.Exception.ToString(), Environment.StackTrace);
                    e.SetObserved();
                };
            }
        }

        #region 로그 종류
        /// <summary>
        /// Debug 로그
        /// </summary>
        /// <param name="log">로그 본문</param>
        //[System.Diagnostics.Conditional("DEBUG")]
        public static void d(string log)
        {
            if (LogLevel >= LogLevel.Debug)
            {
                WriteLog($"Debug : {log}");
            }
        }

        /// <summary>
        /// Information 로그
        /// </summary>
        /// <param name="log">로그 본문</param>
        public static void i(string log)
        {
            if (LogLevel >= LogLevel.Info)
            {
                WriteLog($"Info : {log}");
            }
        }

        /// <summary>
        /// Warning 로그
        /// </summary>
        /// <param name="log">로그 본문</param>
        public static void w(string log)
        {
            if (LogLevel >= LogLevel.Warning)
            {
                WriteLog($"Warning : {log}");
            }
        }

        /// <summary>
        /// Error information 로그
        /// </summary>
        /// <param name="log">로그 본문</param>
        /// <param name="e">Exception</param>
        public static void ei(Exception e, string log = "")
        {
            if (LogLevel >= LogLevel.Error)
            {
                if (string.IsNullOrWhiteSpace(log))
                {
                    WriteLog($"Error Info ({log}) : {e.Message} {Environment.NewLine} {e.StackTrace}");
                }
                else
                {
                    WriteLog($"Error Info : {log} {e.Message} {Environment.NewLine} {e.StackTrace}");
                }
            }
        }

        /// <summary>
        /// Error 로그
        /// </summary>
        /// <param name="kind">Error 종류</param>
        /// <param name="log">로그 본문</param>
        /// <param name="tackTrace">SackTrace Log</param>
        private static void e(string kind, string log, string tackTrace)
        {
            if (LogLevel >= LogLevel.Error)
            {
                WriteLog($"Error : {kind} {log} {Environment.NewLine} {tackTrace}");
            }
        }
        #endregion 로그 종류

        #region 로그 쓰기, 삭제
        /// <summary>
        /// 로그 쓰기
        /// </summary>
        /// <param name="log">로그내용</param>
        private static void WriteLog(string log)
        {
            try
            {
                DeleteExpiredLog();

                var info = new FileInfo(Path.Combine(_logRootDir, $"{_fileName}_{_nowDate}.txt"));

                if (!info.Directory.Exists)
                    info.Directory.Create();

                lock (_lock)
                {
                    using (var sw = new StreamWriter(info.FullName, true))
                    {
                        sw.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss")}] {log}");
                    }
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// 만료된 로그 삭제
        /// </summary>
        private static void DeleteExpiredLog()
        {
            try
            {
                var diffDate = DateTime.Now.ToString("yyyyMMdd");

                if (_nowDate == diffDate)
                {
                    return;
                }

                _nowDate = diffDate;

                var info = new DirectoryInfo(Path.Combine(_logRootDir));

                if (!info.Exists)
                    info.Create();

                var logFormat = _fileName + @"_[0-9]{8}.txt";

                lock (_lock)
                {
                    DateTime nowDate = DateTime.MinValue;
                    DateTime.TryParseExact(_nowDate, "yyyyMMdd", null, DateTimeStyles.None, out nowDate);

                    foreach (var item in info.GetFiles())
                    {
                        var name = item.Name;

                        if (Regex.IsMatch(name, logFormat))
                        {
                            DateTime date = DateTime.MinValue;
                            DateTime.TryParseExact(name.Substring(name.IndexOf('_') + 1, 8), "yyyyMMdd", null, DateTimeStyles.None, out date);

                            if (date == DateTime.MinValue)
                                continue;

                            if (nowDate > date.AddDays(_expireDate))
                            {
                                item.Delete();
                            }
                        }
                    }
                }
            }
            catch
            {
            }
        }
        #endregion 로그 쓰기, 삭제
    }
}
