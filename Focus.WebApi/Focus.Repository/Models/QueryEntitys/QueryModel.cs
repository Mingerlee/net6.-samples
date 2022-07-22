using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Focus.Repository.Models
{
    public class QueryModel
    {
        /// <summary>
        /// 当前页码
        /// </summary>
        public int CurrentPageIndex { get; set; }

        /// <summary>
        /// 每页显示的行数
        /// </summary>
        public int PageRows { get; set; }

        /// <summary>
        /// 排序sql
        /// </summary>
        public string? OrderString { get; set; }

        /// <summary>
        /// 排序字段集合
        /// </summary>
        public List<OrderModel>? OrderModels { get; set; }

        /// <summary>
        /// 汇总列
        /// </summary>
        public List<string>? SumColumns { get; set; }

        /// <summary>
        /// 分组列
        /// </summary>
        public List<string>? GroupColumns { get; set; }
    }

    public class OrderModel
    {
        /// <summary>
        /// 排序String
        /// </summary>
        public string? OrderString { get; set; }

        /// <summary>
        /// 是否正序
        /// </summary>
        public bool IsAsc { get; set; }
    }

    public class QueryParam
    {
        /// <summary>
        /// 获取查询条件
        /// </summary>
        /// <returns></returns>
        public static string GetWhereString<T>(T obj, bool hasWhere = false, bool trim = true) where T : QueryModel
        {
            string querywhere = "";

            Type t = typeof(T);
            foreach (PropertyInfo pi in t.GetProperties())
            {
                object? paraValue = pi.GetValue(obj, null);

                //多选后台定义用数组接收
                if (pi.PropertyType == typeof(string[]))
                {
                    string[]? lst = (string[]?)paraValue;
                    paraValue = null;
                    if (lst != null)
                    {
                        if (lst.Length > 0)
                        {
                            for (int i = 0; i < lst.Length; i++)
                            {
                                paraValue += lst[i] + ",";
                            }
                            paraValue = paraValue.ToString().TrimEnd(',');
                        }
                    }
                }

                if (paraValue != null && (!string.IsNullOrEmpty(paraValue.ToString())))
                {
                    var customAttribute = pi.GetCustomAttributes(typeof(SearchColumnAttribute), false);

                    //判断类型是否为枚举，若为枚举，则取枚举对应的值
                    Type[] types = pi.PropertyType.GetGenericArguments();
                    if (types.Length > 0)
                    {
                        if (types[0].IsEnum)
                        {
                            Array ary = Enum.GetValues(types[0]);
                            int index = 0;
                            foreach (int i in ary)
                            {
                                if (ary.GetValue(index)?.ToString() == Convert.ToString(paraValue))
                                {
                                    paraValue = i;
                                    break;
                                }
                                index++;
                            }
                        }
                    }

                    if (customAttribute.Count() == 1)
                    {
                        string strform = "";
                        string? realColumnName = pi.Name;
                        if (!string.IsNullOrEmpty((customAttribute[0] as SearchColumnAttribute)?.RealColumnName))
                        {
                            realColumnName = (customAttribute[0] as SearchColumnAttribute)?.RealColumnName;
                        }
                        OperaSearch condition = (customAttribute[0] as SearchColumnAttribute).OpSerach;
                        string[]? multLike = (customAttribute[0] as SearchColumnAttribute)?.MultLike;
                        if (multLike != null && multLike.Length > 0)
                        {
                            strform += " and (";
                            foreach (string prop in multLike)
                            {
                                strform += prop + " like '%{0}%' or ";
                            }
                            strform = strform.Substring(0, strform.Length - 4) + ")";
                        }
                        else
                        {
                            switch (condition)
                            {
                                case OperaSearch.like: strform = " and {0} like '%{1}%'"; break;
                                case OperaSearch.lefLike: strform = " and {0} like '{1}%'"; break;
                                case OperaSearch.rightLike: strform = " and {0} like '%{1}'"; break;
                                case OperaSearch.等于: strform = " and {0} = '{1}'"; break;
                                case OperaSearch.不等于: strform = " and {0} <> '{1}'"; break;
                                case OperaSearch.大于: strform = " and {0} > '{1}'"; break;
                                case OperaSearch.大于等于: strform = " and {0} >= '{1}'"; break;
                                case OperaSearch.小于: strform = " and {0} < '{1}'"; break;
                                case OperaSearch.小于等于: strform = " and {0} <= '{1}'"; break;
                                case OperaSearch.包含: strform = " and {0} in ('{1}')"; break;
                                case OperaSearch.不包含: strform = " and {0} not in ('{1}')"; break;
                                default: break;
                            }
                        }
                        var DoesValues = (customAttribute[0] as SearchColumnAttribute).DoesValues == null ? "" : (customAttribute[0] as SearchColumnAttribute).DoesValues;

                        if (string.IsNullOrEmpty(DoesValues) || (!string.IsNullOrEmpty(DoesValues) && !DoesValues.Contains(paraValue.ToString())))
                        {
                            if (pi.PropertyType == typeof(DateTime))
                            {
                                DateTime dateTime = (DateTime)paraValue;
                                if (dateTime.Year > 1900)
                                {
                                    if (trim)
                                        querywhere += string.Format(strform.Replace("'", ""), realColumnName, "'" + paraValue.ToString().Trim() + "'");
                                    else
                                        querywhere += string.Format(strform.Replace("'", ""), realColumnName, "'" + paraValue.ToString() + "'");
                                }
                            }
                            else
                            {
                                string? stringValue = Convert.ToString(paraValue);
                                if ((customAttribute[0] as SearchColumnAttribute).OrdinalIgnoreCase)
                                {
                                    realColumnName = " UPPER(" + realColumnName.Trim() + ") ";
                                    stringValue = stringValue.ToUpper();
                                }

                                if (condition == OperaSearch.包含 || condition == OperaSearch.不包含)
                                {
                                    stringValue = stringValue.Replace(",", "','");
                                }
                                if (multLike != null && multLike.Length > 0)
                                {
                                    querywhere += string.Format(strform, stringValue.Trim());
                                }
                                else
                                {
                                    if (trim)
                                        querywhere += string.Format(strform, realColumnName.Trim(), stringValue.Trim());
                                    else
                                        querywhere += string.Format(strform, realColumnName.Trim(), stringValue);
                                }
                            }
                        }
                    }
                }
            }

            if (hasWhere)
                return querywhere;
            else
                return " where 1=1 " + querywhere;
        }

        /// <summary>
        /// 获取查询条件（包括JqGrid传过来的查询条件）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="trim"></param>
        /// <returns></returns>
        public static QueryCondition GetQueryCondition<T>(T obj, bool trim = true) where T : QueryModel
        {
            string whereString = GetWhereString(obj, true, trim);
            QueryCondition condition = new QueryCondition()
            {
                WhereString = whereString,
                CurrentPageIndex = obj.CurrentPageIndex,
                PageRows = obj.PageRows,
                SumColumns = obj.SumColumns
            };
            string strOrder = string.Empty;
            if (obj.OrderModels != null && obj.OrderModels.Count > 0)
            {
                foreach (OrderModel orderModel in obj.OrderModels)
                {
                    strOrder += orderModel.OrderString + (orderModel.IsAsc ? " asc," : " desc,");
                }
            }
            if (!string.IsNullOrEmpty(strOrder))
            {
                condition.OrderString = " order by " + strOrder.Trim(',');
                condition.WhereString = condition.WhereString + " order by " + strOrder.Trim(',');
            }

            return condition;
        }
    }

    public class QueryCondition
    {
        /// <summary>
        /// where 条件
        /// </summary>
        public string? WhereString { get; set; }

        /// <summary>
        /// 当前页码
        /// </summary>
        public int CurrentPageIndex { get; set; }

        /// <summary>
        /// 每页显示的行数
        /// </summary>
        public int PageRows { get; set; }

        /// <summary>
        /// 排序String
        /// </summary>
        public string? OrderString { get; set; }

        /// <summary>
        /// 汇总列
        /// </summary>
        public List<string>? SumColumns { get; set; }
    }
}
