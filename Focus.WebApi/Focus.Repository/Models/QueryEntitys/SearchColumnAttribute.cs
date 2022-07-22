using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Focus.Repository.Models
{
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = true)]
    public class SearchColumnAttribute : Attribute
    {
        public SearchColumnAttribute()
        {
            OrdinalIgnoreCase = false;
        }

        /// <summary>
        /// 查询条件 = like  ....等
        /// </summary>
        public OperaSearch OpSerach { get; set; }
        /// <summary>
        /// 查询条件真实列名
        /// </summary>
        public string? RealColumnName { get; set; }

        /// <summary>
        /// 多项模糊匹配
        /// </summary>
        public string[]? MultLike { get; set; }
        /// <summary>
        /// 不进行查询的值
        /// </summary>
        public string? DoesValues { get; set; }
        /// <summary>
        /// 查询忽略大小写
        /// </summary>
        public bool OrdinalIgnoreCase { get; set; }
    }

    /// <summary>
    /// 查询条件枚举
    /// </summary>
    public enum OperaSearch
    {
        等于,
        不等于,
        like,
        lefLike,
        rightLike,
        小于,
        小于等于,
        大于,
        大于等于,
        包含,
        不包含
    }
}
