#if true
//#if NET40 || NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading;
using System.Threading.Tasks;
using Utility.Enums;
using Utility.Model;
using Utility.Response;

namespace Utility.Wcf
{
    /// <summary>
    /// wcf 契约基类  契约接口(前提必须要有System.ServiceModel.ServiceContract注解),注意方法名不能相同 
    /// 不然 提示签名有多个这样的错误(要么在注解上改名称) 
    /// 支持 System.Threading.CancellationToken
    /// </summary>
    [ServiceContract]//wcf 契约标识 接口上必须有 否则wcf不支持
   public class ServiceApiClient<Service, Entity, Key> : ClientBase<IServiceApi<Entity, Key>>,IServiceApiClient<Entity,Key> 
		where Service: class, IServiceApi<Entity,Key>
	    where Entity : class, new() 
	 
    {
        /// <summary>
        /// 
        /// </summary>
        public ServiceApiClient()       
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="endpointConfigurationName"></param>
        public ServiceApiClient(string endpointConfigurationName):base(endpointConfigurationName)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="endpointConfigurationName"></param>
        /// <param name="remoteAddress"></param>
        public ServiceApiClient(string endpointConfigurationName, EndpointAddress remoteAddress):base(endpointConfigurationName, remoteAddress)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="endpointConfigurationName"></param>
        /// <param name="remoteAddress"></param>

        public ServiceApiClient(string endpointConfigurationName, string remoteAddress):base(endpointConfigurationName, remoteAddress)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="binding"></param>
        /// <param name="remoteAddress"></param>
        public ServiceApiClient(Binding binding, EndpointAddress remoteAddress):base(binding, remoteAddress)
        {

        }
        /// <summary>添加实体类信息 (契约接口方法必须要有System.ServiceModel.OperationContract注解 支持web 必须要有 System.ServiceModel.Web.WebGet：httpget System.ServiceModel.Web.WebInvoke:httppost)</summary>
        /// <param name="obj">实体类</param>
        /// <param name="language"></param>
        ///<return>返回添加结果,大于0 返回添加成功,小于等于0 返回添加失败 </return>
        public virtual ResponseApi Insert(Entity obj,Language language= Language.Chinese)
        {
            return base.Channel.Insert(obj,language);
        }

        /// <summary>修改实体类信息 (契约接口方法必须要有System.ServiceModel.OperationContract注解 支持web 必须要有 System.ServiceModel.Web.WebGet：httpget System.ServiceModel.Web.WebInvoke:httppost)</summary>
        /// <param name="obj">实体类</param>
        /// <param name="language"></param>
        ///<return>返回修改结果,大于0 返回修改成功,小于等于0 返回修改失败 </return>
        public virtual ResponseApi Update(Entity obj,Language language= Language.Chinese) 
        {
            return base.Channel.Update(obj,language);
        }

        /// <summary>根据id删除实体类信息 (契约接口方法必须要有System.ServiceModel.OperationContract注解 支持web 必须要有 System.ServiceModel.Web.WebGet：httpget System.ServiceModel.Web.WebInvoke:httppost)</summary>
        /// <param name="id">id</param>
        /// <param name="language"></param>
        ///<return>返回删除结果,大于0 返回删除成功,小于等于0 返回删除失败 </return>
        public virtual ResponseApi Delete(Key id,Language language= Language.Chinese)
        {
            return base.Channel.Delete(id,language);
        }

        /// <summary>根据id删除实体类信息(多删除) (契约接口方法必须要有System.ServiceModel.OperationContract注解 支持web 必须要有 System.ServiceModel.Web.WebGet：httpget System.ServiceModel.Web.WebInvoke:httppost)</summary>
        /// <param name="ids">id</param>
        /// <param name="language"></param>
        ///<return>返回删除结果(多删除),大于0 返回删除成功(多删除),小于等于0 返回删除失败(多删除) </return>
        public virtual ResponseApi DeleteList(Key[] ids,Language language= Language.Chinese)
        {
            return base.Channel.DeleteList(ids,language);
        }

        /// <summary>根据条件查询实体类数据集信息 (契约接口方法必须要有System.ServiceModel.OperationContract注解 支持web 必须要有 System.ServiceModel.Web.WebGet：httpget System.ServiceModel.Web.WebInvoke:httppost)</summary>
        /// <param name="obj">实体类</param>
        /// <param name="language"></param>
        ///<return>返回实体类数据集信息 </return>
        public virtual ResponseApi<List<Entity>> FindList(Entity obj,Language language= Language.Chinese)
        {
            return base.Channel.FindList(obj,language);
        }

        /// <summary>根据条件查询实体类数据集数量信息 (契约接口方法必须要有System.ServiceModel.OperationContract注解 支持web 必须要有 System.ServiceModel.Web.WebGet：httpget System.ServiceModel.Web.WebInvoke:httppost)</summary>
        /// <param name="obj">实体类</param>
        /// <param name="language"></param>
        ///<return>返回实体类数据集数量信息</return>
        public virtual ResponseApi<int> Count(Entity obj,Language language= Language.Chinese)
        {
            return base.Channel.Count(obj,language);
        }

        /// <summary>根据条件及分页查询实体类数据集信息 (契约接口方法必须要有System.ServiceModel.OperationContract注解 支持web 必须要有 System.ServiceModel.Web.WebGet：httpget System.ServiceModel.Web.WebInvoke:httppost)</summary>
        /// <param name="obj">实体类</param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        /// <param name="language"></param>
        ///<return>返回实体类数据集信息</return>
        public virtual ResponseApi<List<Entity>> FindListByPage(Entity obj,int page,int size,Language language= Language.Chinese)
         {
            return base.Channel.FindListByPage(obj,page,size,language);
        }

        /// <summary>根据条件及分页查询实体类数据集信息和实体类数据集数量信息 (契约接口方法必须要有System.ServiceModel.OperationContract注解 支持web 必须要有 System.ServiceModel.Web.WebGet：httpget System.ServiceModel.Web.WebInvoke:httppost)</summary>
        /// <param name="obj">实体类</param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        /// <param name="language"></param>
        ///<return>返回实体类数据集信息和实体类数据集数量信息</return>
        public virtual ResponseApi<ResultModel<Entity>> FindResultModelByPage(Entity obj,int page,int size,Language language= Language.Chinese)
        {
            return base.Channel.FindResultModelByPage(obj,page,size,language);
        }

        /// <summary>添加实体类信息 (契约接口方法必须要有System.ServiceModel.OperationContract注解 支持web 必须要有 System.ServiceModel.Web.WebGet：httpget System.ServiceModel.Web.WebInvoke:httppost)</summary>
        /// <param name="obj">实体类</param>
        /// <param name="language"></param>
        ///<param name="cancellationToken"></param>
        ///<return>返回添加结果,大于0 返回添加成功,小于等于0 返回添加失败 </return>
        public virtual Task<ResponseApi> InsertAsync(Entity obj,Language language= Language.Chinese, CancellationToken cancellationToken = default)
        {
            return  new Task<ResponseApi>(()=> Insert(obj, language));
        }

        /// <summary>修改实体类信息 (契约接口方法必须要有System.ServiceModel.OperationContract注解 支持web 必须要有 System.ServiceModel.Web.WebGet：httpget System.ServiceModel.Web.WebInvoke:httppost)</summary>
        /// <param name="obj">实体类</param>
        /// <param name="language"></param>
       ///<param name="cancellationToken"></param>
        ///<return>返回修改结果,大于0 返回修改成功,小于等于0 返回修改失败 </return>
        public virtual Task<ResponseApi> UpdateAsync(Entity obj,Language language= Language.Chinese,CancellationToken cancellationToken = default)
        {
            return new Task<ResponseApi>(()=> Update(obj, language));
        }

        /// <summary>根据id删除实体类信息 (契约接口方法必须要有System.ServiceModel.OperationContract注解 支持web 必须要有 System.ServiceModel.Web.WebGet：httpget System.ServiceModel.Web.WebInvoke:httppost)</summary>
        /// <param name="id">id</param>
        /// <param name="language"></param>
        ///<param name="cancellationToken"></param>
        ///<return>返回删除结果,大于0 返回删除成功,小于等于0 返回删除失败 </return>
        public virtual Task<ResponseApi> DeleteAsync(Key id,Language language= Language.Chinese,CancellationToken cancellationToken = default)
        {
            return new Task<ResponseApi>(()=> Delete(id, language));
        }

        /// <summary>根据id删除实体类信息(多删除) (契约接口方法必须要有System.ServiceModel.OperationContract注解 支持web 必须要有 System.ServiceModel.Web.WebGet：httpget System.ServiceModel.Web.WebInvoke:httppost)</summary>
        /// <param name="ids">id</param>
        /// <param name="language"></param>
        ///<param name="cancellationToken"></param>
        ///<return>返回删除结果(多删除),大于0 返回删除成功(多删除),小于等于0 返回删除失败(多删除) </return>
        public virtual Task<ResponseApi> DeleteListAsync(Key[] ids,Language language= Language.Chinese,CancellationToken cancellationToken = default)
        {
            return new Task<ResponseApi>(()=> DeleteList(ids, language));
        }

        /// <summary>根据条件查询实体类数据集信息 (契约接口方法必须要有System.ServiceModel.OperationContract注解 支持web 必须要有 System.ServiceModel.Web.WebGet：httpget System.ServiceModel.Web.WebInvoke:httppost)</summary>
        /// <param name="obj">实体类</param>
        /// <param name="language"></param>
        ///<param name="cancellationToken"></param>
        ///<return>返回实体类数据集信息 </return>
        public virtual Task<ResponseApi<List<Entity>>> FindListAsync(Entity obj,Language language= Language.Chinese, CancellationToken cancellationToken = default)
        {
            return new Task<ResponseApi<List<Entity>>>(()=> FindList(obj, language));
        }

        /// <summary>根据条件查询实体类数据集数量信息 (契约接口方法必须要有System.ServiceModel.OperationContract注解 支持web 必须要有 System.ServiceModel.Web.WebGet：httpget System.ServiceModel.Web.WebInvoke:httppost)</summary>
        /// <param name="obj">实体类</param>
        /// <param name="language"></param>
        ///<param name="cancellationToken"></param>
        ///<return>返回实体类数据集数量信息</return>
        public virtual Task<ResponseApi<int>> CountAsync(Entity obj,Language language= Language.Chinese, CancellationToken cancellationToken = default)
        {
            return new Task<ResponseApi<int>>(()=> Count(obj, language));
        }

        /// <summary>根据条件及分页查询实体类数据集信息 (契约接口方法必须要有System.ServiceModel.OperationContract注解 支持web 必须要有 System.ServiceModel.Web.WebGet：httpget System.ServiceModel.Web.WebInvoke:httppost)</summary>
        /// <param name="obj">实体类</param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        /// <param name="language"></param>
        ///<param name="cancellationToken"></param>
        ///<return>返回实体类数据集信息</return>
        public virtual Task<ResponseApi<List<Entity>>>  FindListByPageAsync(Entity obj,int page,int size,Language language= Language.Chinese, CancellationToken cancellationToken = default)
        {
            return new Task<ResponseApi<List<Entity>>>(()=> FindListByPage(obj, page, size, language));
        }

        /// <summary>根据条件及分页查询实体类数据集信息和实体类数据集数量信息 (契约接口方法必须要有System.ServiceModel.OperationContract注解 支持web 必须要有 System.ServiceModel.Web.WebGet：httpget System.ServiceModel.Web.WebInvoke:httppost)</summary>
        /// <param name="obj">实体类</param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        /// <param name="language"></param>
        ///<param name="cancellationToken"></param>
        ///<return>返回实体类数据集信息和实体类数据集数量信息</return>
        public virtual Task<ResponseApi<ResultModel<Entity>>>  FindResultModelByPageAsync(Entity obj,int page,int size,Language language= Language.Chinese, CancellationToken cancellationToken = default)
        {
            return new Task<ResponseApi<Model.ResultModel<Entity>>>(()=> FindResultModelByPage(obj, page, size, language));
        }
    }

}
#endif