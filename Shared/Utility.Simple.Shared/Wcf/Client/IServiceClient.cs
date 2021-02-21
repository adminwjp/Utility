#if true
//#if NET40 || NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48
using System.ServiceModel.Web;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Utility.Model;
using Utility.Enums;

namespace Utility.Wcf
{
    /// <summary>
    /// wcf 契约基类  契约接口(前提必须要有System.ServiceModel.ServiceContract注解),注意方法名不能相同 
    /// 不然 提示签名有多个这样的错误(要么在注解上改名称) 
    /// 支持 System.Threading.CancellationToken
    /// </summary>
    [ServiceContract]//wcf 契约标识 接口上必须有 否则wcf不支持
    public    interface  IServiceClient<Entity,Key>:IService<Entity,Key>
    //,System.ServiceModel.IClientChannel 
		where Entity:class
    {
        /// <summary>
        /// 添加实体类信息 
        /// (契约接口方法必须要有System.ServiceModel.OperationContract注解 支持web 
        /// 必须要有 System.ServiceModel.Web.WebGet：httpget System.ServiceModel.Web.WebInvoke:httppost)
        /// </summary>
        /// <param name="obj">实体类</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回添加结果,大于0 返回添加成功,小于等于0 返回添加失败 </return>
        [OperationContract(Name = "InsertAsync")]
        [WebInvoke(UriTemplate = "InsertAsync", Method = "POST",RequestFormat = WebMessageFormat.Json, 
        ResponseFormat = WebMessageFormat.Json,BodyStyle = WebMessageBodyStyle.Wrapped)]
        Task<int> InsertAsync(Entity obj, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// 修改实体类信息 
        /// (契约接口方法必须要有System.ServiceModel.OperationContract注解 支持web 
        /// 必须要有 System.ServiceModel.Web.WebGet：httpget System.ServiceModel.Web.WebInvoke:httppost)
        /// </summary>
        /// <param name="obj">实体类</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回修改结果,大于0 返回修改成功,小于等于0 返回修改失败 </return>
        [OperationContract(Name = "UpdateAsync")]
        [WebInvoke(UriTemplate = "UpdateAsync", Method = "POST", RequestFormat =WebMessageFormat.Json,
        ResponseFormat =WebMessageFormat.Json,BodyStyle = WebMessageBodyStyle.Wrapped)]
        Task<int> UpdateAsync(Entity obj, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// 根据id删除实体类信息 
        /// (契约接口方法必须要有System.ServiceModel.OperationContract注解 支持web 必须要有
        /// System.ServiceModel.Web.WebGet：httpget System.ServiceModel.Web.WebInvoke:httppost)
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回删除结果,大于0 返回删除成功,小于等于0 返回删除失败 </return>
        [OperationContract(Name = "DeleteAsync")]
        [WebGet(UriTemplate = "DeleteAsync", RequestFormat = WebMessageFormat.Json, ResponseFormat =WebMessageFormat.Json)]
        Task<int> DeleteAsync(Key id, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// 根据id删除实体类信息(多删除) 
        /// (契约接口方法必须要有System.ServiceModel.OperationContract注解 支持web 必须要有
        /// System.ServiceModel.Web.WebGet：httpget System.ServiceModel.Web.WebInvoke:httppost)
        /// </summary>
        /// <param name="ids">id</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回删除结果(多删除),大于0 返回删除成功(多删除),小于等于0 返回删除失败(多删除) </return>
        [OperationContract(Name = "DeleteListAsync")]
        [WebInvoke(UriTemplate = "DeleteListAsync", Method = "POST", RequestFormat = WebMessageFormat.Json,
        ResponseFormat = WebMessageFormat.Json,BodyStyle = WebMessageBodyStyle.Wrapped)]
        Task<int> DeleteListAsync(Key[] ids, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// 根据条件查询实体类数据集信息 
        /// (契约接口方法必须要有System.ServiceModel.OperationContract注解 支持web 必须要有
        /// System.ServiceModel.Web.WebGet：httpget System.ServiceModel.Web.WebInvoke:httppost)
        /// </summary>
        /// <param name="obj">实体类</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回实体类数据集信息 </return>
        [OperationContract(Name = "FindListAsync")]
        [WebInvoke(UriTemplate = "FindListAsync", Method = "POST", RequestFormat = WebMessageFormat.Json,
        ResponseFormat = WebMessageFormat.Json,BodyStyle = WebMessageBodyStyle.Wrapped)]
        Task<List<Entity>> FindListAsync(Entity obj, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// 根据条件查询实体类数据集数量信息 
        /// (契约接口方法必须要有System.ServiceModel.OperationContract注解 支持web 必须要有
        /// System.ServiceModel.Web.WebGet：httpget System.ServiceModel.Web.WebInvoke:httppost)
        /// </summary>
        /// <param name="obj">实体类</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回实体类数据集数量信息</return>
        [OperationContract(Name = "CountAsync")]
        [WebInvoke(UriTemplate = "CountAsync", Method = "POST", RequestFormat = WebMessageFormat.Json, 
        ResponseFormat = WebMessageFormat.Json,BodyStyle = WebMessageBodyStyle.Wrapped)]
        Task<int> CountAsync(Entity obj, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// 根据条件及分页查询实体类数据集信息
        /// (契约接口方法必须要有System.ServiceModel.OperationContract注解 支持web 必须要有
        /// System.ServiceModel.Web.WebGet：httpget System.ServiceModel.Web.WebInvoke:httppost)
        /// </summary>
        /// <param name="obj">实体类</param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回实体类数据集信息</return>
        [OperationContract(Name= "FindListByPageAsync")]
        [WebInvoke(UriTemplate = "FindListByPageAsync", Method = "POST", RequestFormat = WebMessageFormat.Json, 
        ResponseFormat = WebMessageFormat.Json,BodyStyle = WebMessageBodyStyle.Wrapped)]
        Task<List<Entity>> FindListByPageAsync(Entity obj,int page,int size, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>根据条件及分页查询实体类数据集信息和实体类数据集数量信息 (契约接口方法必须要有System.ServiceModel.OperationContract注解 支持web 必须要有 System.ServiceModel.Web.WebGet：httpget System.ServiceModel.Web.WebInvoke:httppost)</summary>
        /// <param name="obj">实体类</param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回实体类数据集信息和实体类数据集数量信息</return>
        [OperationContract(Name = "FindResultModelByPageAsync")]
        [WebInvoke(UriTemplate = "FindResultModelByPageAsync", Method = "POST", RequestFormat =WebMessageFormat.Json,
        ResponseFormat = WebMessageFormat.Json,BodyStyle = WebMessageBodyStyle.Wrapped)]
        Task<ResultModel<Entity>> FindResultModelByPageAsync(Entity obj,int page,int size, CancellationToken cancellationToken = default(CancellationToken));
    }

}
#endif