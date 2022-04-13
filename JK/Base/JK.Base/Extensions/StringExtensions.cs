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

        /// <summary>
        /// long to string value
        /// </summary>
        /// <param name="val">변경 값</param>
        /// <param name="defaultVal">디폴트 값</param>
        /// <returns></returns>
        public static string ToLong(this string val, string defaultVal = "0")
        {
            try
            {
                long result = 0;

                if (long.TryParse(val, out result))
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
        /// long to string value
        /// </summary>
        /// <param name="val">변경 값</param>
        /// <param name="defaultVal">디폴트 값</param>
        /// <param name="format">format</param>
        /// <returns></returns>
        public static string ToLongFormet(this string val, string format, string defaultVal = "0")
        {
            try
            {
                long result = 0;

                if (long.TryParse(val, out result))
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

        /// <summary>
        /// decimal to string value
        /// </summary>
        /// <param name="val">변경 값</param>
        /// <param name="defaultVal">디폴트 값</param>
        /// <returns></returns>
        public static string ToDecimal(this string val, string defaultVal = "0")
        {
            try
            {
                decimal result = 0;

                if (decimal.TryParse(val, out result))
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
        /// decimal to string value
        /// </summary>
        /// <param name="val">변경 값</param>
        /// <param name="defaultVal">디폴트 값</param>
        /// <param name="format">format</param>
        /// <returns></returns>
        public static string ToIntDecimal(this string val, string format, string defaultVal = "0")
        {
            try
            {
                decimal result = 0;

                if (decimal.TryParse(val, out result))
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

        /// <summary>
        /// int to string value
        /// </summary>
        /// <param name="val">변경 값</param>
        /// <param name="defaultVal">디폴트 값</param>
        /// <returns></returns>
        public static string ToDouble(this string val, string defaultVal = "0")
        {
            try
            {
                double result = 0;

                if (double.TryParse(val, out result))
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
        public static string ToDoubleFormet(this string val, string format, string defaultVal = "0")
        {
            try
            {
                double result = 0;

                if (double.TryParse(val, out result))
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
