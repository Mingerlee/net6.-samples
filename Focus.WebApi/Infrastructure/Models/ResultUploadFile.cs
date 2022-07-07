using Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models
{
    public class ResultUploadFile
    {
        public ResultUploadFile()
        {
        }
        public ResultUploadFile(UploadFileStatus status)
        {
            Status=status;
            Error = status.Description();
        }

        public ResultUploadFile(UploadFileStatus status,string error)
        {
            Status = status;
            Error = error;
        }
        /// <summary>
        /// 文件名
        /// </summary>
        public string? FileName { get; set; }
        /// <summary>
        /// 文件大小
        /// </summary>
        public long FileSize { get; set; }
        /// <summary>
        /// 文件类型
        /// </summary>
        public string? ContentType { get; set; }
        /// <summary>
        /// 文件路径
        /// </summary>
        public string? FilePath { get; set; }
        /// <summary>
        /// 返回码，0为成功，非0失败
        /// </summary>
        public UploadFileStatus Status { get; set; }
        /// <summary>
        /// 错误信息
        /// </summary>
        public string? Error { get; set; }

    }
}
