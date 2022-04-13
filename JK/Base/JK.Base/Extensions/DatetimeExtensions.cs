using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JK.Base.Extensions
{
    public static class DatetimeExtensions
    {
        /// <summary>
        /// datetime to string (yyyy-MM-dd HH-mm-ss)
        /// </summary>
        /// <param name="val">int</param>
        /// <param name="defaultValue">default value</param>
        /// <returns></returns>
        public static string ToDefaultFormet(this DateTime val, string defaultValue = "")
        {
            string result = defaultValue;

            try
            {
                result = val.ToString("yyyy-MM-dd HH:mm:ss");
            }
            catch
            {
                result = defaultValue;
            }

            return result;

        }

        /// <summary>
        /// datetime to string (yyyy-MM-dd HH-mm-ss)
        /// </summary>
        /// <param name="val">int</param>
        /// <param name="defaultValue">default value</param>
        /// <returns></returns>
        public static string ToDayFormet(this DateTime val, string defaultValue = "")
        {
            string result = defaultValue;

            try
            {
                result = val.ToString("yyyy-MM-dd");
            }
            catch
            {
                result = defaultValue;
            }

            return result;

        }

        /// <summary>
        /// datetime to string (yyyy-MM-dd HH-mm-ss)
        /// </summary>
        /// <param name="val">int</param>
        /// <param name="defaultValue">default value</param>
        /// <returns></returns>
        public static string ToDefaultFormet(this DateTime? val, string defaultValue = "")
        {
            string result = defaultValue;

            try
            {
                if (val == null)
                    result = defaultValue;
                else
                    result = val.Value.ToDefaultFormet(defaultValue);
            }
            catch
            {
                result = defaultValue;
            }

            return result;
        }

        /// <summary>
        /// datetime to string (yyyy-MM-dd HH-mm-ss)
        /// </summary>
        /// <param name="val">int</param>
        /// <param name="defaultValue">default value</param>
        /// <returns></returns>
        public static string ToDayFormet(this DateTime? val, string defaultValue = "")
        {
            string result = defaultValue;

            try
            {
                if (val == null)
                    result = defaultValue;
                else
                    result = val.Value.ToDayFormet(defaultValue);
            }
            catch
            {
                result = defaultValue;
            }

            return result;
        }
    }
}
