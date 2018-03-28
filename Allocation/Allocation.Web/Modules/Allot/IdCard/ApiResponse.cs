using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Allocation.Allot.IdCard
{
    public class ApiResponse
    {
        /// <summary>
        /// 是否查询成功
        /// 0：查询失败 ， 1：查询成功
        /// </summary>
        public int Isok { get; set; }
        public int Code { get; set; }
        public ApiResponseData Data { get; set; }
    }

    public class ApiResponseData
    {
        /// <summary>
        /// 是否符合身份证号码格式
        /// 0：符合 ， -1：不符合
        /// </summary>
        public int Err { get; set; }
        /// <summary>
        /// unicode格式身份证所在地 （err:-1时无此结果）
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 性别（err:-1时无此结果）
        /// M：男性 ， F：女性
        /// </summary>
        public string Sex { get; set; }
        /// <summary>
        /// 生日信息（err:-1时无此结果）
        /// </summary>
        public string Birthday { get; set; }
    }
}