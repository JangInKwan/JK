using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace JK.Base.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum val, string defaultValue = "")
        {
            Type type = val.GetType();
            MemberInfo[] memInfo = type.GetMember(val.ToString());

            if (memInfo != null && memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attrs != null && attrs.Length > 0)
                {
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }

            return val.ToString();
        }
    }
}