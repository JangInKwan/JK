using JK.Base.Logs;
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
    ///  Xml 형식 Config 파일
    /// </summary>
    public abstract class XmlConfig : IConfig
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
        public string ConfigName { get; protected set; } = "Config.xml";

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
                        var xmlStr = File.ReadAllText(path);
                        XmlSerializer xmlSerializer = new XmlSerializer(new XmlReflectionImporter().ImportTypeMapping(this.GetType(), null, null));

                        var config = this;

                        using (TextReader reader = new StringReader(xmlStr))
                        {
                            config = (XmlConfig)xmlSerializer.Deserialize(reader);
                        }
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
                        XmlSerializer reqserializer = new XmlSerializer(new XmlReflectionImporter().ImportTypeMapping(this.GetType(), null, null));

                        using (StringWriter writer = new StringWriter())
                        {
                            reqserializer.Serialize(writer, this);
                            File.WriteAllText(path, writer.ToString(), Encoding.UTF8);
                        }
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

