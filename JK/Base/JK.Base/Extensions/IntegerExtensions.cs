using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JK.Base.Extensions
{
    public static class IntegerExtensions
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

        /// <summary>
        /// string to long value
        /// </summary>
        /// <param name="val">변경 값</param>
        /// <param name="defaultVal">디폴트 값</param>
        /// <returns></returns>
        public static long ToLong(this string val, int defaultVal = 0)
        {
            try
            {
                long result = 0;

                if (long.TryParse(val, out result))
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

        /// <summary>
        /// string to decimal decimal
        /// </summary>
        /// <param name="val">변경 값</param>
        /// <param name="defaultVal">디폴트 값</param>
        /// <returns></returns>
        public static decimal ToDecimal(this string val, int defaultVal = 0)
        {
            try
            {
                decimal result = 0;

                if (decimal.TryParse(val, out result))
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

        /// <summary>
        /// string to double double
        /// </summary>
        /// <param name="val">변경 값</param>
        /// <param name="defaultVal">디폴트 값</param>
        /// <returns></returns>
        public static double ToDouble(this string val, int defaultVal = 0)
        {
            try
            {
                double result = 0;

                if (double.TryParse(val, out result))
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
