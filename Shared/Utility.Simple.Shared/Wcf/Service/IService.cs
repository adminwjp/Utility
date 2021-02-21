#if true
//#if NET40 || NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48


namespace Utility.Wcf
{
    /// <summary>
    /// wcf 实体信息 契约接口(前提必须要有System.ServiceModel.ServiceContract注解),注意方法名不能相同 不然 提示签名有多个这样的错误(要么在注解上改名称)
    /// 同步 OperationContract 方法“Update”与基于任务的异步 OperationContract 方法“UpdateAsync”匹配 最好都给签名
    /// wcf 不支持 System.Threading.CancellationToken  task 一直假死 不调可以
    /// </summary>
    [System.ServiceModel.ServiceContract]//wcf 契约标识 接口上必须有 否则wcf不支持
    public    interface IService<Entity,Key> where Entity:class
    {
         /// <summary>添加实体类信息 (契约接口方法必须要有System.ServiceModel.OperationContract注解 支持web 必须要有 System.ServiceModel.Web.WebGet：httpget System.ServiceModel.Web.WebInvoke:httppost)</summary>
        /// <param name="obj">实体类</param>
        ///<return>返回添加结果,大于0 返回添加成功,小于等于0 返回添加失败 </return>
        [System.ServiceModel.OperationContract(Name = "Insert")]
        [System.ServiceModel.Web.WebInvoke(UriTemplate = "Insert", Method = "POST", RequestFormat = System.ServiceModel.Web.WebMessageFormat.Json, ResponseFormat = System.ServiceModel.Web.WebMessageFormat.Json,BodyStyle = System.ServiceModel.Web.WebMessageBodyStyle.Wrapped)]
        int Insert(Entity obj);

        /// <summary>修改实体类信息 (契约接口方法必须要有System.ServiceModel.OperationContract注解 支持web 必须要有 System.ServiceModel.Web.WebGet：httpget System.ServiceModel.Web.WebInvoke:httppost)</summary>
        /// <param name="obj">实体类</param>
        ///<return>返回修改结果,大于0 返回修改成功,小于等于0 返回修改失败 </return>
        [System.ServiceModel.OperationContract(Name = "Update")]
        [System.ServiceModel.Web.WebInvoke(UriTemplate = "Update", Method = "POST", RequestFormat = System.ServiceModel.Web.WebMessageFormat.Json, ResponseFormat = System.ServiceModel.Web.WebMessageFormat.Json,BodyStyle = System.ServiceModel.Web.WebMessageBodyStyle.Wrapped)]
        int Update(Entity obj);

        /// <summary>根据id删除实体类信息 (契约接口方法必须要有System.ServiceModel.OperationContract注解 支持web 必须要有 System.ServiceModel.Web.WebGet：httpget System.ServiceModel.Web.WebInvoke:httppost)</summary>
        /// <param name="id">id</param>
        ///<return>返回删除结果,大于0 返回删除成功,小于等于0 返回删除失败 </return>
        [System.ServiceModel.OperationContract(Name = "Delete")]
        [System.ServiceModel.Web.WebGet(UriTemplate = "Delete", RequestFormat = System.ServiceModel.Web.WebMessageFormat.Json, ResponseFormat = System.ServiceModel.Web.WebMessageFormat.Json)]
        int Delete(Key id);

        /// <summary>根据id删除实体类信息(多删除) (契约接口方法必须要有System.ServiceModel.OperationContract注解 支持web 必须要有 System.ServiceModel.Web.WebGet：httpget System.ServiceModel.Web.WebInvoke:httppost)</summary>
        /// <param name="ids">id</param>
        ///<return>返回删除结果(多删除),大于0 返回删除成功(多删除),小于等于0 返回删除失败(多删除) </return>
        [System.ServiceModel.OperationContract(Name = "DeleteList")]
        [System.ServiceModel.Web.WebInvoke(UriTemplate = "DeleteList", Method = "POST", RequestFormat = System.ServiceModel.Web.WebMessageFormat.Json, ResponseFormat = System.ServiceModel.Web.WebMessageFormat.Json,BodyStyle = System.ServiceModel.Web.WebMessageBodyStyle.Wrapped)]
        int DeleteList(Key[] ids);

         /// <summary>根据条件查询实体类数据集信息 (契约接口方法必须要有System.ServiceModel.OperationContract注解 支持web 必须要有 System.ServiceModel.Web.WebGet：httpget System.ServiceModel.Web.WebInvoke:httppost)</summary>
        /// <param name="obj">实体类</param>
        ///<return>返回实体类数据集信息 </return>
        [System.ServiceModel.OperationContract(Name = "FindList")]
        [System.ServiceModel.Web.WebInvoke(UriTemplate = "FindList", Method = "POST", RequestFormat = System.ServiceModel.Web.WebMessageFormat.Json, ResponseFormat = System.ServiceModel.Web.WebMessageFormat.Json,BodyStyle = System.ServiceModel.Web.WebMessageBodyStyle.Wrapped)]
        System.Collections.Generic.List<Entity> FindList(Entity obj);

        /// <summary>根据条件查询实体类数据集数量信息 (契约接口方法必须要有System.ServiceModel.OperationContract注解 支持web 必须要有 System.ServiceModel.Web.WebGet：httpget System.ServiceModel.Web.WebInvoke:httppost)</summary>
        /// <param name="obj">实体类</param>
        ///<return>返回实体类数据集数量信息</return>
        [System.ServiceModel.OperationContract(Name = "Count")]
        [System.ServiceModel.Web.WebInvoke(UriTemplate = "Count", Method = "POST", RequestFormat = System.ServiceModel.Web.WebMessageFormat.Json, ResponseFormat = System.ServiceModel.Web.WebMessageFormat.Json,BodyStyle = System.ServiceModel.Web.WebMessageBodyStyle.Wrapped)]
        int Count(Entity obj);

        /// <summary>根据条件及分页查询实体类数据集信息 (契约接口方法必须要有System.ServiceModel.OperationContract注解 支持web 必须要有 System.ServiceModel.Web.WebGet：httpget System.ServiceModel.Web.WebInvoke:httppost)</summary>
        /// <param name="obj">实体类</param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        ///<return>返回实体类数据集信息</return>
        [System.ServiceModel.OperationContract]
        [System.ServiceModel.Web.WebInvoke(UriTemplate = "FindListByPage", Method = "POST", RequestFormat = System.ServiceModel.Web.WebMessageFormat.Json, ResponseFormat = System.ServiceModel.Web.WebMessageFormat.Json,BodyStyle = System.ServiceModel.Web.WebMessageBodyStyle.Wrapped)]
        System.Collections.Generic.List<Entity> FindListByPage(Entity obj,int page,int size);

        /// <summary>根据条件及分页查询实体类数据集信息和实体类数据集数量信息 (契约接口方法必须要有System.ServiceModel.OperationContract注解 支持web 必须要有 System.ServiceModel.Web.WebGet：httpget System.ServiceModel.Web.WebInvoke:httppost)</summary>
        /// <param name="obj">实体类</param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        ///<return>返回实体类数据集信息和实体类数据集数量信息</return>
        [System.ServiceModel.OperationContract(Name = "FindResultModelByPage")]
        [System.ServiceModel.Web.WebInvoke(UriTemplate = "FindResultModelByPage", Method = "POST", RequestFormat = System.ServiceModel.Web.WebMessageFormat.Json, ResponseFormat = System.ServiceModel.Web.WebMessageFormat.Json,BodyStyle = System.ServiceModel.Web.WebMessageBodyStyle.Wrapped)]
        Utility.Model.ResultModel<Entity> FindResultModelByPage(Entity obj,int page,int size);
    }

}
#endif