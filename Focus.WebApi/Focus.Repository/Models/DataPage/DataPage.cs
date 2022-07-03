using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Focus.Repository.Models
{
    public class DataPage
    {
        /// <summary>
        /// 当前页
        /// </summary>
        public int CurrentPage { get; set; }
        /// <summary>
        /// 下一页页码
        /// </summary>
        public int? NextPage { get; set; }
        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPages { get; set; }
        /// <summary>
        /// 总条目数
        /// </summary>
        public int TotalItems { get; set; }
        /// <summary>
        /// 每页条目数
        /// </summary>
        public int PerPageItems { get; set; }

        /// <summary>
        /// 汇总列
        /// </summary>
        public List<ColumnSum> Sums { get; set; }
    }
    /// <summary>
    /// 查询结果为<c>List</c>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ListPage<T> : DataPage
    {
        /// <summary>
        /// 查询结果
        /// </summary>
        public List<T> data { get; set; }
    }

    public class TablePage : DataPage
    {
        /// <summary>
        /// 查询结果
        /// </summary>
        public System.Data.DataTable Items { get; set; }
    }

    public class ColumnSum 
    {
        public string ColName { get; set; }
        public Decimal SumValue { get; set; }
    }
}
