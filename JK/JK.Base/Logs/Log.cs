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
        // 로그 디렉토리 dlfma
        private const string CONFIG_DIR = "Log";
        // 로그 파일이름
        private const string CONFIG_NAME_FORMAT = "{0}_";
        // StackTrace 로그 파일이름
        private const string CONFIG_NAME_FORMAT_STACKTRACE = "{0}_StackTrace_";


        // 오늘날짜
        private static DateTime _nowDate = DateTime.MinValue;
        // Assembly Name
        private static string _assemblyName = Assembly.GetEntryAssembly().GetName().Name;
        // 로그 만료일 (일 보관)
        private static int _expireDate = 90;
        // lock
        private static object _lock = new object();


        private static bool inUseStackTraceLog = false;
        /// <summary>
        /// StackTrace 로그 사용
        /// </summary>
        public static void UseStackTraceLog()
        {
            if (!inUseStackTraceLog)
            {
                inUseStackTraceLog = true;

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

        #region Kind Log
        /// <summary>
        /// Debug 로그
        /// </summary>
        /// <param name="log">로그 본문</param>
        [System.Diagnostics.Conditional("DEBUG")]
        public static void d(string log)
        {
            WriteLog($"Debug - {log}");
        }

        /// <summary>
        /// Information 로그
        /// </summary>
        /// <param name="log">로그 본문</param>
        public static void i(string log)
        {
            WriteLog($"Info - {log}");
        }

        /// <summary>
        /// Warning 로그
        /// </summary>
        /// <param name="log">로그 본문</param>
        public static void w(string log)
        {
            WriteLog($"Warning - {log}");
        }

        /// <summary>
        /// Error information 로그
        /// </summary>
        /// <param name="log">로그 본문</param>
        /// <param name="e">Exception</param>
        public static void ei(string log, Exception e)
        {
            WriteLog($"Error Info - {log} : {e.Message}");
        }

        /// <summary>
        /// Error 로그
        /// </summary>
        /// <param name="kind">Error 종류</param>
        /// <param name="log">로그 본문</param>
        /// <param name="tackTrace">SackTrace Log</param>
        private static void e(string kind, string log, string tackTrace)
        {
            WriteLog($"Error - {kind} : {log}");
            WriteStackTraceLog($"Error - {kind} : {log}", tackTrace);
        }
        #endregion Kind Log

        #region Write, Delete Log 
        /// <summary>
        /// 로그 쓰기
        /// </summary>
        /// <param name="log"></param>
        private static void WriteLog(string log)
        {
            try
            {
                DeleteExpiredLog();

                var info = new FileInfo(Path.Combine(Directory.GetCurrentDirectory(), CONFIG_DIR, $"{string.Format(CONFIG_NAME_FORMAT, _assemblyName)}{_nowDate}.txt"));

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
            catch { }
        }

        /// <summary>
        /// StackTrace 로그 쓰기
        /// </summary>
        /// <param name="log"></param>
        /// <param name="tackTrace"></param>
        private static void WriteStackTraceLog(string log, string tackTrace)
        {
            try
            {
                DeleteExpiredLog();

                var info = new FileInfo(Path.Combine(Directory.GetCurrentDirectory(), CONFIG_DIR, $"{string.Format(CONFIG_NAME_FORMAT_STACKTRACE, _assemblyName)}{_nowDate}.txt"));

                if (!info.Directory.Exists)
                    info.Directory.Create();

                lock (_lock)
                {
                    using (var sw = new StreamWriter(info.FullName, true))
                    {
                        sw.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss")}] {log}");
                        sw.WriteLine(tackTrace);
                    }
                }
            }
            catch { }
        }

        /// <summary>
        /// 만료된 로그 삭제
        /// </summary>
        private static void DeleteExpiredLog()
        {
            try
            {
                var diffDate = DateTime.Now;

                if (_nowDate == diffDate)
                {
                    return;
                }

                _nowDate = diffDate;

                var info = new DirectoryInfo(Path.Combine(Directory.GetCurrentDirectory(), CONFIG_DIR));

                if (!info.Exists)
                    info.Create();

                var logFormat = string.Format(CONFIG_NAME_FORMAT, _assemblyName) + @"[0-9]{8}.txt";
                var logStackFormat = string.Format(CONFIG_NAME_FORMAT_STACKTRACE, _assemblyName) + @"[0-9]{8}.txt";

                lock (_lock)
                {
                    foreach (var item in info.GetFiles())
                    {
                        var name = item.Name;

                        if (Regex.IsMatch(name, logFormat) || Regex.IsMatch(name, logStackFormat))
                        {
                            DateTime date = DateTime.MinValue;

                            DateTime.TryParseExact(name.Substring(name.IndexOf('_') + 1, 8), "yyyyMMdd", null, DateTimeStyles.None, out date);

                            if (date == DateTime.MinValue)
                                continue;

                            if (_nowDate > date.AddDays(_expireDate))
                            {
                                item.Delete();
                            }
                        }
                    }
                }
            }
            catch { }
        }
        #endregion File Write, Delete
    }
}
