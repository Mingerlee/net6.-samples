using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models
{
    /// <summary>
    /// 合并表头单元格参数集合
    /// </summary>
    public class ExcelTableHeader
    {
        /// <summary>
        /// 顺序
        /// </summary>
        public int TopColIndex { get; set; }
        /// <summary>
        /// 合并列头名称
        /// </summary>
        public string HeaderName { get; set; }
        /// <summary>
        /// 合并几个单元格
        /// </summary>
        public int SecondColCount { get; set; }
    }
}
