using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JK.Base.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// int to string value
        /// </summary>
        /// <param name="val">변경 값</param>
        /// <param name="defaultVal">디폴트 값</param>
        /// <returns></returns>
        public static string ToInt(this string val, string defaultVal = "0")
        {
            try
            {
                int result = 0;

                if (int.TryParse(val, out result))
                {
                    return result.ToString();
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

        /// <summary>
        /// int to string value
        /// </summary>
        /// <param name="val">변경 값</param>
        /// <param name="defaultVal">디폴트 값</param>
        /// <param name="format">format</param>
        /// <returns></returns>
        public static string ToIntFormet(this string val, string format, string defaultVal = "0")
        {
            try
            {
                int result = 0;

                if (int.TryParse(val, out result))
                {
                    return string.IsNullOrWhiteSpace(format) ? result.ToString() : result.ToString(format);
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
