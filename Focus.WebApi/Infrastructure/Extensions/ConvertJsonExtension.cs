using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Models
{
    public static partial class ConvertJsonExtension
    {
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="formatDate"></param>
        /// <returns></returns>
        public static string Serialize(this object obj, JsonSerializerSettings formatDate = null)
        {
            if (obj == null) return null;
            formatDate = formatDate ?? new JsonSerializerSettings
            {
                DateFormatString = "yyyy-MM-dd HH:mm:ss"
            };

            return JsonConvert.SerializeObject(obj, formatDate);
        }
        public static T DeserializeObject<T>(this string entityString)
        {
            if (string.IsNullOrEmpty(entityString))
            {
                return default(T);
            }
            if (entityString == "{}")
            {
                entityString = "[]";
            }
            return JsonConvert.DeserializeObject<T>(entityString);
        }
    }
}
