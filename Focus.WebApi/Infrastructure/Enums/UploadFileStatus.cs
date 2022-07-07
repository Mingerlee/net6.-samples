using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Enums
{
    public enum UploadFileStatus
    {
        [Description("上传文件失败。")]
        Failure,
        [Description("上传文件成功。")]
        Success,
        [Description("文件为空。")]
        NotFound
    }
}
