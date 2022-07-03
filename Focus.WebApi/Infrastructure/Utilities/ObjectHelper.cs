using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Utilities
{
    public class ObjectHelper
    {
        /// <summary>
        /// 将object尝试转为指定对象
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static T ConvertObjToModel<T>(object data) where T : new()
        {
            if (data == null) return new T();
            // 定义集合    
            T result = new T();

            // 获得此模型的类型   
            Type type = typeof(T);
            string tempName = "";

            // 获得此模型的公共属性 
            PropertyInfo[] propertys = result.GetType().GetProperties();
            foreach (PropertyInfo pi in propertys)
            {
                tempName = pi.Name;  // 检查object是否包含此列    

                // 判断此属性是否有Setter      
                if (!pi.CanWrite) continue;

                try
                {
                    object value = GetPropertyValue(data, tempName);
                    if (value != DBNull.Value)
                    {
                        Type tempType = pi.PropertyType;
                        pi.SetValue(result, GetDataByType(value, tempType), null);

                    }
                }
                catch
                { }

            }

            return result;
        }

        //调用方法：

        /// <summary>
        /// 获取一个类指定的属性值
        /// </summary>
        /// <param name="info">object对象</param>
        /// <param name="field">属性名称</param>
        /// <returns></returns>
        public static object GetPropertyValue(object info, string field)
        {
            if (info == null) return null;
            Type t = info.GetType();
            IEnumerable<System.Reflection.PropertyInfo> property = from pi in t.GetProperties() where pi.Name.ToLower() == field.ToLower() select pi;
            return property.First().GetValue(info, null);
        }

        /// <summary>
        /// 将数据转为制定类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data1"></param>
        /// <returns></returns>
        public static object GetDataByType(object data1, Type itype, params object[] myparams)
        {
            object result = new object();
            try
            {
                if (itype == typeof(decimal))
                {
                    result = Convert.ToDecimal(data1);
                    if (myparams.Length > 0)
                    {
                        result = Convert.ToDecimal(Math.Round(Convert.ToDecimal(data1), Convert.ToInt32(myparams[0])));
                    }
                }
                else if (itype == typeof(double))
                {

                    if (myparams.Length > 0)
                    {
                        result = Convert.ToDouble(Math.Round(Convert.ToDouble(data1), Convert.ToInt32(myparams[0])));
                    }
                    else
                    {
                        result = double.Parse(Convert.ToDecimal(data1).ToString("0.00"));
                    }
                }
                else if (itype == typeof(Int32))
                {
                    result = Convert.ToInt32(data1);
                }
                else if (itype == typeof(DateTime))
                {
                    result = Convert.ToDateTime(data1);
                }
                else if (itype == typeof(Guid))
                {
                    result = new Guid(data1.ToString());
                }
                else if (itype == typeof(string))
                {
                    result = data1.ToString();
                }
            }
            catch
            {
                if (itype == typeof(decimal))
                {
                    result = 0;
                }
                else if (itype == typeof(double))
                {
                    result = 0;
                }
                else if (itype == typeof(Int32))
                {
                    result = 0;
                }
                else if (itype == typeof(DateTime))
                {
                    result = null;
                }
                else if (itype == typeof(Guid))
                {
                    result = Guid.Empty;
                }
                else if (itype == typeof(string))
                {
                    result = "";
                }
            }
            return result;
        }
    }
}
