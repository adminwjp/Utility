using Utility.Descs;

namespace Utility.Enums
{
    /// <summary>返回码 及描述 </summary>
    public enum Code
    {
        /// <summary>
        /// None
        /// </summary>
        None,
        /// <summary>
        /// 成功
        /// </summary>
        [Desc(ChineseDesc = "成功", EnglishDesc = "sucess")]
        Success = 20000,
        /// <summary>
        /// 添加成功
        /// </summary>
        [Desc(ChineseDesc = "添加成功", EnglishDesc = "add sucess")]
        AddSuccess = 20001,
        /// <summary>
        /// 编辑成功
        /// </summary>
        [Desc(ChineseDesc = "编辑成功", EnglishDesc = "modify sucess")]
        ModifySuccess = 20002,
        /// <summary>
        /// 删除成功
        /// </summary>
        [Desc(ChineseDesc = "删除成功", EnglishDesc = "delete sucess")]
        DeleteSuccess = 20003,
        /// <summary>
        /// 查询成功
        /// </summary>
        [Desc(ChineseDesc = "查询成功", EnglishDesc = "query sucess")]
        QuerySuccess = 20004,
        /// <summary>
        /// 登录成功
        /// </summary>
        [Desc(ChineseDesc = "登录成功", EnglishDesc = "login sucess")]
        LoginSuccess = 20005,
       
        /// <summary>
        /// 上传文件成功
        /// </summary>
        [Desc(ChineseDesc = "上传文件成功", EnglishDesc = "upload file sucess")]
        UploadFileSuccess = 20006,
        /// <summary>
        /// 操作成功
        /// </summary>
        [Desc(ChineseDesc = "操作成功", EnglishDesc = "operator sucess")]
        OperatorSuccess = 20007,
        /// <summary>
        /// 修改状态成功
        /// </summary>
        [Desc(ChineseDesc = "修改状态成功", EnglishDesc = "modify state sucess")]
         ModifyStateSuccess = 20008,
        /// <summary>
        /// 发送成功
        /// </summary>
        [Desc(ChineseDesc = "发送成功", EnglishDesc = "send success")]
        SendSucces = 20009,
        /// <summary>
        /// 发送成功
        /// </summary>
        [Desc(ChineseDesc = "注册成功", EnglishDesc = "register success")]
        RegisterSucces = 20010,
        /// <summary>
        /// 退出成功
        /// </summary>
        [Desc(ChineseDesc = "退出成功", EnglishDesc = "logout sucess")]
        LogoutSuccess = 20011,

        /// <summary>
        /// 失败
        /// </summary>
        [Desc(ChineseDesc = "失败", EnglishDesc = "fail")]
        Fail = 40000,
        /// <summary>
        /// 系统繁忙
        /// </summary>
        [Desc(ChineseDesc = "系统繁忙", EnglishDesc = "system error")]
        Error = 50000,

        /// <summary>
        /// 参数错误
        /// </summary>
        [Desc(ChineseDesc = "参数错误", EnglishDesc = "param  error")]
        ParamError = 40001,
        /// <summary>
        /// 参数不能为空
        /// </summary>
        [Desc(ChineseDesc = "参数不能为空", EnglishDesc = "param is not null")]
        ParamNotNull = 40002,
        /// <summary>
        /// 参数格式错误
        /// </summary>
        [Desc(ChineseDesc = "参数格式错误", EnglishDesc = "param format error")]
        ParamFormatError = 40003,
        /// <summary>
        /// 不支持的错误
        /// </summary>
        [Desc(ChineseDesc = "不支持的错误", EnglishDesc = "not support error")]
        NotSupportEroor = 40004,
        /// <summary>
        /// 存在
        /// </summary>
        [Desc(ChineseDesc = "存在", EnglishDesc = "exists")]
        Exxists = 40005,
        /// <summary>
        /// 手机号已存在
        /// </summary>
        [Desc(ChineseDesc = "手机号已存在", EnglishDesc = "phone exists")]
        PhoneExists = 40006,
        /// <summary>
        /// 邮箱已存在
        /// </summary>
        [Desc(ChineseDesc = "邮箱已存在", EnglishDesc = "email exists")]
        EmailExists = 40007,
        /// <summary>
        /// 账号错误
        /// </summary>
        [Desc(ChineseDesc = "账号错误", EnglishDesc = "account error")]
        AccountError = 40008,
        /// <summary>
        /// 密码不一致
        /// </summary>
        [Desc(ChineseDesc = "密码不一致", EnglishDesc = "password not eq")]
        PasswordNotEq = 40009,
        /// <summary>
        /// 账号不存在
        /// </summary>
        [Desc(ChineseDesc = "账号不存在", EnglishDesc = "account not exists")]
        AccountNotExists = 40010,
        /// <summary>
        /// 账号存在
        /// </summary>
        [Desc(ChineseDesc = "账号存在", EnglishDesc = "account exists")]
        AccountExists = 40011,
        /// <summary>
        /// 发送失败
        /// </summary>
        [Desc(ChineseDesc = "发送失败", EnglishDesc = "send fail")]
        SendFail = 40012,

        /// <summary>
        /// 认证失败
        /// </summary>
        [Desc(ChineseDesc = "认证失败", EnglishDesc = "auth fail")]
        AuthFail = 40003,
        /// <summary>
        /// 登录失败
        /// </summary>
        [Desc(ChineseDesc = "登录失败", EnglishDesc = "login fail")]
        LoginFail = 40013,
        /// <summary>
        /// id 找不到
        /// </summary>
        [Desc(ChineseDesc = "id 找不到", EnglishDesc = "id not found")]
        IdNotFound = 40014,
        /// <summary>
        /// 级联删除失败
        /// </summary>
        [Desc(ChineseDesc = "级联删除失败", EnglishDesc = "cascde delete fail")]
        CascadeDeleteFail = 40015,
        /// <summary>
        /// 上传文件失败
        /// </summary>
        [Desc(ChineseDesc = "上传文件失败", EnglishDesc = "upload file fail")]
        UploadFileFail = 40016,
        /// <summary>
        /// 不支持的操作
        /// </summary>
        [Desc(ChineseDesc = "不支持的操作", EnglishDesc = "not support operator")]
        NotSupport = 40017,
        /// <summary>
        /// 操作失败
        /// </summary>
        [Desc(ChineseDesc = "操作失败", EnglishDesc = "operator fail")]
        OperatorFail = 40018,
        /// <summary>
        /// token不能为空
        /// </summary>
        [Desc(ChineseDesc = "token不能为空", EnglishDesc = "token not null")]
        TokenNotNull = 40019,
        /// <summary>
        /// token失效
        /// </summary>
        [Desc(ChineseDesc = "token失效", EnglishDesc = "token expires")]
        TokenExpires = 40020,
        /// <summary>
        /// 删除失败
        /// </summary>
        [Desc(ChineseDesc = "删除失败", EnglishDesc = "delete fail")]
        DeleteFail = 40021,
        /// <summary>
        /// 添加失败
        /// </summary>
        [Desc(ChineseDesc = "添加失败", EnglishDesc = "add fail")]
        AddFail = 40022,
        /// <summary>
        /// 修改失败
        /// </summary>
        [Desc(ChineseDesc = "修改失败", EnglishDesc = "modify fail")]
        ModifyFail = 40023,
        /// <summary>
        /// 修改状态失败
        /// </summary>
        [Desc(ChineseDesc = "修改状态失败", EnglishDesc = "modify state fail")]
        ModifyStateFail = 40024,
        /// <summary>
        /// 查询失败
        /// </summary>
        [Desc(ChineseDesc = "查询失败", EnglishDesc = "query fail")]
        QueryFail = 40025,

        ///不支持的操作
        [Desc(ChineseDesc = "不支持的操作", EnglishDesc = "not support operator")]
        NotSupportOperator = 40026,
        /// <summary>
        /// 不存在
        /// </summary>
        [Desc(ChineseDesc = "不存在", EnglishDesc = "not exists")]
        NotExists = 40027,
        /// <summary>
        /// 发送成功
        /// </summary>
        [Desc(ChineseDesc = "注册失败", EnglishDesc = "register fail")]
        RegisterFail = 40028,
        /// <summary>
        /// 登录失败
        /// </summary>
        [Desc(ChineseDesc = "退出失败", EnglishDesc = "logout fail")]
        LogoutFail = 40029,

    }
}
