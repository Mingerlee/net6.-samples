using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Infrastructure.Enums
{
    public enum EnumPlatformType : byte
    {
        /// <summary>
        /// 全部
        /// </summary>
        [Description("全部")]
        All = 0,
        /// <summary>
        /// PC
        /// </summary>
        [Description("PC")]
        PC = 1,
        /// <summary>
        /// WAP
        /// </summary>
        [Description("WAP")]
        WAP = 2,
        /// <summary>
        /// IOS
        /// </summary>
        [Description("IOS")]
        IOS = 3,
        /// <summary>
        /// ANDROID
        /// </summary>
        [Description("ANDROID")]
        ANDROID = 4
    }
}
