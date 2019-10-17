using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JK.Base.HttpClinets
{
    public enum ContentTypeEnum
    {
        Html,
        Xml,
        Json,
    }


    public static class ContentTypeEnumExtensions
    {
        public static string ToHeaderString(this ContentTypeEnum value)
        {
            string result = string.Empty;

            switch (value)
            {

                case ContentTypeEnum.Html:
                    result = "text/html";
                    break;
                case ContentTypeEnum.Xml:
                    result = "text/xml";
                    break;
                case ContentTypeEnum.Json:
                default:
                    result = "application/json";
                    break;
            }

            return result;
        }
    }
}
