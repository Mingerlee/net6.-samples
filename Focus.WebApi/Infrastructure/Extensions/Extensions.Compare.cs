using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models
{
    public static partial class Extensions
    {
        /// <summary>
        /// 比较两个实体值是否有修改过
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="compareModel">对比的model实体</param>
        /// <param name="isAttr">是否有CompareValueAttribute特性标记</param>
        /// <returns></returns>
        public static List<Results> CompareModel<T>(this T t, T compareModel, bool isAttr = false)
        {
            List<Results> data = new List<Results>();
            if (t == null || compareModel == null)
            {
                return data;
            }
            PropertyInfo[] newProperties = t.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            PropertyInfo[] oldProperties = compareModel.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            if (newProperties.Length <= 0 || oldProperties.Length <= 0)
            {
                return data;
            }
            var oldFieldLst = new Dictionary<string, string>();
            foreach (PropertyInfo item in oldProperties)
            {
                if (isAttr)
                {
                    bool hasFieldAttribute = item.CustomAttributes.Any(t => t.AttributeType == typeof(CompareValueAttribute));
                    if (!hasFieldAttribute)
                        continue;
                }
                string name = item.Name;//实体类字段名称
                string value = item.GetValue(compareModel, null)?.ToString() ?? "";//该字段的值
                if (item.PropertyType.IsValueType || item.PropertyType.Name.StartsWith("String"))
                {
                    oldFieldLst.Add(name, value);//在此可转换value的类型
                }
            }
            foreach (PropertyInfo item in newProperties)
            {
                if (isAttr)
                {
                    bool hasFieldAttribute = item.CustomAttributes.Any(t => t.AttributeType == typeof(CompareValueAttribute));
                    if (!hasFieldAttribute)
                        continue;
                }
                string value = item.GetValue(t, null)?.ToString() ?? "";//该字段的值
                string filedName = item.Name;//实体类字段名称;
                if (oldFieldLst.ContainsKey(filedName))
                {
                    string olddata = oldFieldLst[filedName];
                    if (olddata != value)
                    {
                        data.Add(new Results { FiledName = filedName, FiledOldValue = olddata, FiledNewValue = value, IsModify = true });
                    }
                }
                else
                {
                    data.Add(new Results { FiledName = filedName, FiledNewValue = value, IsAdded = true });
                }

            }
            return data;
        }
        /// <summary>
        /// Model实体是否有修改
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="compareModel">对比的model实体</param>
        /// <param name="isAttr">是否有CompareValueAttribute特性标记</param>
        /// <returns></returns>
        public static bool IsModify<T>(this T t, T compareModel, bool isAttr = false)
        {
            return CompareModel(t, compareModel, isAttr).Count > 0;
        }
    }
}
