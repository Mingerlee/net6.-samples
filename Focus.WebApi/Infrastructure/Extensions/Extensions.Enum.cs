using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Infrastructure.Models
{
    public static partial class Extensions
    {
        /// <summary>
        /// 获取枚举值的描述信息
        /// </summary>
        /// <param name="source">枚举值</param>
        /// <returns></returns>
        public static string GetDescription(this Enum source)
        {
            Type attributeType = typeof(DescriptionAttribute);
            FieldInfo[] fields = source.GetType().GetFields();
            string str = string.Empty;
            foreach (FieldInfo info in fields)
            {
                if (info.FieldType.IsEnum && info.Name.Equals(source.ToString()))
                {
                    object[] customAttributes = info.GetCustomAttributes(attributeType, true);
                    if (customAttributes.Length > 0)
                    {
                        DescriptionAttribute attribute = (DescriptionAttribute)customAttributes[0];
                        return attribute.Description;
                    }
                    return info.Name;
                }
            }
            return str;
        }

        /// <summary>
        /// 获取枚举值的描述信息
        /// </summary>
        /// <param name="source">枚举值</param>
        /// <param name="enumType">Type,该参数的格式为typeof(需要读的枚举类型)</param>
        /// <returns></returns>
        public static string GetDescription(this Enum source, Type enumType)
        {
            Type attributeType = typeof(DescriptionAttribute);
            FieldInfo[] fields = enumType.GetFields();
            string str = string.Empty;
            foreach (FieldInfo info in fields)
            {
                if (info.FieldType.IsEnum && info.Name.Equals(source.ToString()))
                {
                    object[] customAttributes = info.GetCustomAttributes(attributeType, true);
                    if (customAttributes.Length > 0)
                    {
                        DescriptionAttribute attribute = (DescriptionAttribute)customAttributes[0];
                        return attribute.Description;
                    }
                    return info.Name;
                }
            }
            return str;
        }

        /// <summary>
        /// 将枚举转换为数字
        /// </summary>
        /// <param name="source">枚举值</param>
        /// <returns></returns>
        public static int ToInt32(this Enum source)
        {
            return Convert.ToInt32(source);
        }

        /// <summary>
        /// 获取枚举值的描述信息
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Description(this System.Enum value)
        {
            try
            {
                var field = value.GetType().GetField(value.ToString());
                var attributes = field.GetCustomAttributes(false);
                dynamic displayAttribute = null;

                if (attributes.Any())
                {
                    displayAttribute = attributes.ElementAt(0);
                }
                return displayAttribute?.Description ?? "Description Not Found";
            }
            catch
            {
                return string.Empty;
            }
        }

    }
}
