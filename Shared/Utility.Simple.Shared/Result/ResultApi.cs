using System.Collections.Generic;
using C = Utility.Enums;

namespace Utility
{

    /// <summary>
    /// 通用返回结果
    /// </summary>
    public class Result
    {
        /// <summary>
        /// 返回结果
        /// </summary>
        protected Result()
        {
            this.Clear();
        }
        private static readonly object obj = new object();//锁
        private static Result result;//结果

        /// <summary>
        ///单例 懒汉式
        /// </summary>
        public static Result Instance
        {
            get
            {
                if (result == null)
                {
                    lock (obj)
                    {
                        if (result == null)
                        {
                            result = new Result();
                        }
                    }
                }
                return result;
            }
        }

        /// <summary>
        /// 创建 对象
        /// </summary>
        /// <returns></returns>
        public static Result Create()
        {
            return new Result();
        }

        /// <summary>
        /// 成功 输出
        /// </summary>
        public static   Result Successed
        {
            get
            {
              return Create().SetCode((int)C.Code.Success).SetMessage(C.Code.Success.ToString()).SetSuccess(true);
            }
        }

        /// <summary>
        /// 失败 输出
        /// </summary>
        public static Result Fail
        {
            get
            {
                return Create().SetCode((int)C.Code.Fail).SetMessage(C.Code.Fail.ToString());
            }
        }

        /// <summary>
        /// 错误 输出
        /// </summary>
        public static Result Errror
        {
            get
            {
                return Create().SetCode((int)C.Code.Error).SetMessage(C.Code.Error.ToString());
            }
        }

        /// <summary>
        /// 错误码
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public Result SetCode(int code)
        {
            this.code = code;
            return this;
        }

        /// <summary>
        /// 提示
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public Result SetMessage(string message)
        {
            this.message = message;
            return this;
        }
       
        /// <summary>
        ///设置 是否成功
        /// </summary>
        /// <param name="success"></param>
        /// <returns></returns>
        public Result SetSuccess(bool success)
        {
            this.success = success;
            return this;
        }

        /// <summary>
        /// 设置错误信息
        /// </summary>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public Result SetErrorMsg(string errorMsg)
        {
            this.errorMsg = errorMsg;
            return this;
        }

        /// <summary>
        /// 设置 data
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
#if !(NET20 || NET30 || NET35 || NET10 || NET11)
        public Result SetData(dynamic data)
#else
        public Result SetData(object data)
#endif
        {
            this.data = data;
            return this;
        }

        /// <summary>
        /// 清空 数据
        /// </summary>
        /// <returns></returns>
        public Result Clear()
        {
            this.code = 0;
            this.message = string.Empty;
            this.success = false;
            this.errorMsg = string.Empty;
            this.data = null;
            return this;
        }
        private int code;
        private string message;
        private bool success;
        private string errorMsg;
#if !(NET20 || NET30 || NET35 || NET10 || NET11)
        private dynamic data;
#else
        private object data;
#endif
        /// <summary>状态码 40000 失败 20000成功 50000 异常</summary>
        public int Code { get { return this.code; } set { this.code = value; } }
        /// <summary>返回结果描述信息</summary>
        public string Message { get { return this.message; } set { this.message = value; } } 
        /// <summary>操作结果，默认失败</summary>
        public bool Success { get { return this.success; } set { this.success = value; } }
        /// <summary>错误信息</summary>
        public string ErrorMsg { get { return this.errorMsg; } set { this.errorMsg = value; } }
        /// <summary>操作结果，默认失败</summary>
#if !(NET20 || NET30 || NET35 || NET10 || NET11)
        public dynamic Data { get { return this.data; } set { this.data = value; } }
#else
         public object Data { get { return this.data; } set { this.data = value; } }
#endif
    }

    /// <summary>
    /// api返回结果
    /// </summary>
    public class ResultApi : Dictionary<string, object>
    {
        /// <summary>
        /// 私有无法更新
        /// </summary>
        private ResultApi()
        {

        }
        //readonly static ResultApi resultApi = new ResultApi();

        /// <summary>
        /// 创建 对象
        /// </summary>
        /// <param name="success"></param>
        /// <returns></returns>
        public static ResultApi Create(bool success = true) => success ? Success : Fail;

        /// <summary>
        /// 成功 输出
        /// </summary>
        public static ResultApi Success {
            get
            {
                ResultApi resultApi = New(C.Code.Success);
                return resultApi;
            }
        }

        /// <summary>
        /// new object()
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private static ResultApi New(C.Code code)
        {
            ResultApi resultApi = new ResultApi();
            resultApi.Add("note", code.ToString());
            resultApi.Add("code", (int)code);
            return resultApi;
        }

        /// <summary>
        /// 失败 输出
        /// </summary>
        public static  ResultApi     Fail
        {
            get
            {
                ResultApi resultApi = New(C.Code.Fail);
                return resultApi;
            }
        }

        /// <summary>
        /// 异常  输出
        /// </summary>
        public static ResultApi Exception
        {
            get
            {
                ResultApi resultApi = New(C.Code.Error);
                return resultApi;
            }
        }

        /// <summary>
        /// 是否成功
        /// </summary>
        public  bool Succeed
        {
            get
            {
                if (this.ContainsKey("code"))
                {
                    return ((int)this["code"]) == 200;
                }
                else
                {
                    return false;
                }
            }
        }
    
        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public ResultApi Put(string key, object val)
        {
            if (this.ContainsKey(key))
            {
                this[key] = val;
            }
            else
            {
                this.Add(key, val);
            }
            return this;
        }
        
        /// <summary>
        /// 返回 json 信息
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Json.JsonHelper.ToJson(this);
        }
    }
}
