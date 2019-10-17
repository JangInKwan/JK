using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JK.Base.Configs
{
    interface IConfig
    {
        /// <summary>
        /// Config 파일 Load
        /// </summary>
        void Load();
        /// <summary>
        /// Config Save
        /// </summary>
        void Save();
    }
}
