using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Enums;
namespace Infrastructure.Models
{
    /// <summary>
    /// 返回结果集（为了跟其他项目对接，统一为小写） 版本 : v.1.4
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ResultModel<T>
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 返回码
        /// </summary>
        public ResponseCode ErrorCode { get; set; }

        /// <summary>
        /// 返回结果
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string Msg { get; set; }

        public ResultModel()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        public ResultModel(T t)
        {
            Status = 1;
            ErrorCode = (int)ResponseCode.sys_success;
            Msg = "";
            Data = t;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="msg"></param>
        public ResultModel(ResponseCode code, string msg)
        {
            Status = 0;
            ErrorCode = code;
            Msg = msg;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="res"></param>
        /// <param name="ec"></param>
        /// <param name="msg"></param>
        public ResultModel(int status, ResponseCode ec, string msg)
        {
            Status = status;
            ErrorCode = ec;
            Msg = msg;
        }

    }
}
