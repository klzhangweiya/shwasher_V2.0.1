namespace ShwasherSys
{
    /// <summary>
    /// 返回API的json对象
    /// </summary>
    public class ResponsResult
    {
        public ResponsResult()
        {
            Success = true;
            //HttpStatusCode = HttpStatusCode.OK;
        }

        public ResponsResult(bool success, string msg = "")
        {
            Success = success;
            //HttpStatusCode = httpStatusCode ?? HttpStatusCode.InternalServerError;
            Message = msg;
        }

        public ResponsResult(string msg, int? resultCode = null, bool success = false)
        {
            Success = success;
            Message = msg;
            ResultCode = resultCode;
        }
        public ResponsResult(object result, string msg = null)
        {
            Success = true;
            Message = msg;
            Result = result;
        }

        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get; set; }

        ///// <summary>
        ///// 状态码
        ///// </summary>
        //public HttpStatusCode HttpStatusCode { get; set; }

        /// <summary>
        /// 成功时返回的数据
        /// </summary>
        public object Result { get; set; }

        /// <summary>
        /// 错误代码
        /// </summary>
        public int? ResultCode { get; set; }

        /// <summary>
        /// 错误信息/成功信息
        /// </summary>
        public string Message { get; set; }

        ///// <summary>
        ///// 错误描述
        ///// </summary>
        //public string Errordesc { get; set; }
    }

}
