using JK.Base.Logs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JK.Base.Configs
{
    /// <summary>
    /// Json 형식 Config 파일
    /// </summary>
    public abstract class JsonConfig : IConfig
    {
        // lock
        private object _lock = new object();

        /// <summary>
        /// Config 폴더 이름
        /// </summary>
        public string ConfigDir { get; protected set; } = "Config";
        /// <summary>
        /// Config 파일 이름
        /// </summary>
        public string ConfigName { get; protected set; } = "Config.json";

        
        /// <summary>
        /// Config 파일 Load
        /// </summary>
        public void Load()
        {
            try
            {
                var path = new FileInfo(Path.Combine(Directory.GetCurrentDirectory(), ConfigDir, ConfigName)).FullName;

                if (path == null || File.Exists(path))
                {
                    lock (_lock)
                    {
                        var str = File.ReadAllText(path);
                        JsonConvert.PopulateObject(str, this);
                    }
                }
                else
                {
                    Log.i("Config Load File is Not Found");
                }
            }
            catch (Exception e)
            {
                Log.ei("Config Load Exception", e);
            }
        }

        /// <summary>
        /// Config Save
        /// </summary>
        public void Save()
        {
            try
            {
                var path = new FileInfo(Path.Combine(Directory.GetCurrentDirectory(), ConfigDir, ConfigName)).FullName;

                if (File.Exists(path))
                {
                    lock (_lock)
                    {
                        var str = JsonConvert.SerializeObject(this, Formatting.Indented);
                        File.WriteAllText(path, str, Encoding.UTF8);
                    }
                }
            }
            catch (Exception e)
            {
                Log.ei("Config Save Exception", e);
            }
        }
    }
}
