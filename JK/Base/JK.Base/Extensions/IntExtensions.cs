using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JK.Base.Extensions
{
    public static class IntExtensions
    {
        /// <summary>
        /// string to int value
        /// </summary>
        /// <param name="val">변경 값</param>
        /// <param name="defaultVal">디폴트 값</param>
        /// <returns></returns>
        public static int Toint(this string val, int defaultVal = 0)
        {
            try
            {
                int result = 0;

                if (int.TryParse(val, out result))
                {
                    return result;
                }
                else
                {
                    return defaultVal;
                }
            }
            catch
            {
                return defaultVal;
            }
        }
    }
}
