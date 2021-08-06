using JK.Base.Logs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace JK.Base.Configs
{
    /// <summary>
    /// Config 파일
    /// </summary>
    public abstract class ConfigBase<T> where T : ConfigBase<T>
    {
        // lock
        private object _lock = new object();
        /// <summary>
        /// Config 폴더 이름
        /// </summary>
        private string ConfigDir = string.Empty;
        /// <summary>
        /// Config 파일 이름
        /// </summary>
        private string ConfigFile = string.Empty;
        /// <summary>
        /// Config 타입
        /// </summary>
        public ConfigType ConfigType = ConfigType.JSON;

        private static readonly Lazy<T> _Inc = new Lazy<T>(() => CreateInstanceOfT());

        public static T Inc { get { return _Inc.Value; } }

        private static T CreateInstanceOfT()
        {
            var config = Activator.CreateInstance(typeof(T), true) as T;
            config.Load();

            return config;
        }

        public ConfigBase(string configDir = "Config", string configFile = "Config.json", ConfigType configType = ConfigType.JSON)
        {
            this.ConfigDir = configDir;
            this.ConfigFile = configFile;
            this.ConfigType = configType;
        }

        /// <summary>
        /// Config 파일 Load
        /// </summary>
        public void Load()
        {
            try
            {
                var path = new FileInfo(Path.Combine(Directory.GetCurrentDirectory(), ConfigDir, ConfigFile)).FullName;

                if (path == null || File.Exists(path))
                {
                    lock (_lock)
                    {
                        var data = File.ReadAllText(path);

                        if (this.ConfigType == ConfigType.XML)
                        {
                            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
                            var config = this;

                            using (TextReader reader = new StringReader(data))
                            {
                                config = (T)xmlSerializer.Deserialize(reader);
                            }
                        }
                        else
                        {
                            JsonConvert.PopulateObject(data, this);
                        }
                    }
                }
                else
                {
                    Log.i("Config Load File is Not Found");
                    Save();
                }
            }
            catch (Exception e)
            {
                Log.ei(e, "Config Load Exception");
            }
        }

        /// <summary>
        /// Config 파일 Save
        /// </summary>
        public void Save()
        {
            try
            {
                var path = new FileInfo(Path.Combine(Directory.GetCurrentDirectory(), ConfigDir, ConfigFile));

                lock (_lock)
                {
                    if (!path.Directory.Exists)
                        path.Directory.Create();


                    if (this.ConfigType == ConfigType.XML)
                    {
                        XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));

                        using (StringWriter writer = new StringWriter())
                        {
                            xmlSerializer.Serialize(writer, this);
                            File.WriteAllText(path.FullName, writer.ToString(), Encoding.UTF8);
                        }
                    }
                    else
                    {
                        var str = JsonConvert.SerializeObject(this, Formatting.Indented);
                        File.WriteAllText(path.FullName, str, Encoding.UTF8);
                    }
                }
            }
            catch (Exception e)
            {
                Log.ei(e, "Config Save Exception");
            }
        }
    }
}
