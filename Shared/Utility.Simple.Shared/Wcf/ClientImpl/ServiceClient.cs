#if true
//#if NET40 || NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48
using Utility.Enums;
using System.Threading;

namespace Utility.Wcf
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="Entity"></typeparam>
    /// <typeparam name="Service"></typeparam>
    /// <typeparam name="Key"></typeparam>

    public class ServiceClient<Service, Entity, Key> : System.ServiceModel.ClientBase<Service>,IService<Entity,Key> ,IServiceClient<Entity,Key>
		where Service: class,IService<Entity,Key> 
        where Entity : class
    {
        /// <summary>
        /// 
        /// </summary>
        public ServiceClient()       
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="endpointConfigurationName"></param>
        public ServiceClient(string endpointConfigurationName):base(endpointConfigurationName)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="endpointConfigurationName"></param>
        /// <param name="remoteAddress"></param>
        public ServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress):base(endpointConfigurationName, remoteAddress)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="endpointConfigurationName"></param>
        /// <param name="remoteAddress"></param>
        public ServiceClient(string endpointConfigurationName, string remoteAddress):base(endpointConfigurationName, remoteAddress)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="binding"></param>
        /// <param name="remoteAddress"></param>
        public ServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress):base(binding, remoteAddress)
        {

        }

        /// <summary>添加实体类信息 (契约接口方法必须要有System.ServiceModel.OperationContract注解 支持web 必须要有 System.ServiceModel.Web.WebGet：httpget System.ServiceModel.Web.WebInvoke:httppost)</summary>
        /// <param name="obj">实体类</param>
        ///<return>返回添加结果,大于0 返回添加成功,小于等于0 返回添加失败 </return>
        public virtual int Insert(Entity obj)
        {
            return base.Channel.Insert(obj);
        }

        /// <summary>修改实体类信息 (契约接口方法必须要有System.ServiceModel.OperationContract注解 支持web 必须要有 System.ServiceModel.Web.WebGet：httpget System.ServiceModel.Web.WebInvoke:httppost)</summary>
        /// <param name="obj">实体类</param>
        ///<return>返回修改结果,大于0 返回修改成功,小于等于0 返回修改失败 </return>
        public virtual int Update(Entity obj)
        {
            return base.Channel.Update(obj);
        }

        /// <summary>根据id删除实体类信息 (契约接口方法必须要有System.ServiceModel.OperationContract注解 支持web 必须要有 System.ServiceModel.Web.WebGet：httpget System.ServiceModel.Web.WebInvoke:httppost)</summary>
        /// <param name="id">id</param>
        ///<return>返回删除结果,大于0 返回删除成功,小于等于0 返回删除失败 </return>
        public virtual int Delete(Key id)
        {
            return base.Channel.Delete(id);
        }

        /// <summary>根据id删除实体类信息(多删除) (契约接口方法必须要有System.ServiceModel.OperationContract注解 支持web 必须要有 System.ServiceModel.Web.WebGet：httpget System.ServiceModel.Web.WebInvoke:httppost)</summary>
        /// <param name="ids">id</param>
        ///<return>返回删除结果(多删除),大于0 返回删除成功(多删除),小于等于0 返回删除失败(多删除) </return>
        public virtual int DeleteList(Key[] ids)
        {
            return base.Channel.DeleteList(ids);
        }

        /// <summary>根据条件查询实体类数据集信息 (契约接口方法必须要有System.ServiceModel.OperationContract注解 支持web 必须要有 System.ServiceModel.Web.WebGet：httpget System.ServiceModel.Web.WebInvoke:httppost)</summary>
        /// <param name="obj">实体类</param>
        ///<return>返回实体类数据集信息 </return>
        public virtual System.Collections.Generic.List<Entity> FindList(Entity obj)
        {
            return base.Channel.FindList(obj);
        }

        /// <summary>根据条件查询实体类数据集数量信息 (契约接口方法必须要有System.ServiceModel.OperationContract注解 支持web 必须要有 System.ServiceModel.Web.WebGet：httpget System.ServiceModel.Web.WebInvoke:httppost)</summary>
        /// <param name="obj">实体类</param>
        ///<return>返回实体类数据集数量信息</return>
        public virtual int Count(Entity obj)
        {
            return base.Channel.Count(obj);
        }

        /// <summary>根据条件及分页查询实体类数据集信息 (契约接口方法必须要有System.ServiceModel.OperationContract注解 支持web 必须要有 System.ServiceModel.Web.WebGet：httpget System.ServiceModel.Web.WebInvoke:httppost)</summary>
        /// <param name="obj">实体类</param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        ///<return>返回实体类数据集信息</return>
        public virtual System.Collections.Generic.List<Entity> FindListByPage(Entity obj, int page, int size)
        {
            return base.Channel.FindListByPage(obj, page, size);
        }

        /// <summary>根据条件及分页查询实体类数据集信息和实体类数据集数量信息 (契约接口方法必须要有System.ServiceModel.OperationContract注解 支持web 必须要有 System.ServiceModel.Web.WebGet：httpget System.ServiceModel.Web.WebInvoke:httppost)</summary>
        /// <param name="obj">实体类</param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        ///<return>返回实体类数据集信息和实体类数据集数量信息</return>
        public virtual Utility.Model.ResultModel<Entity> FindResultModelByPage(Entity obj, int page, int size)
        {
            return base.Channel.FindResultModelByPage(obj,page,size);
        }

#if !(NET20 || NET30 || NET35)

        /// <summary>添加实体类信息 (契约接口方法必须要有System.ServiceModel.OperationContract注解 支持web 必须要有 System.ServiceModel.Web.WebGet：httpget System.ServiceModel.Web.WebInvoke:httppost)</summary>
        /// <param name="obj">实体类</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回添加结果,大于0 返回添加成功,小于等于0 返回添加失败 </return>
        public virtual System.Threading.Tasks.Task<int> InsertAsync(Entity obj, CancellationToken cancellationToken = default)
        {
            return new System.Threading.Tasks.Task<int>(() => Insert(obj));
        }

        /// <summary>修改实体类信息 (契约接口方法必须要有System.ServiceModel.OperationContract注解 支持web 必须要有 System.ServiceModel.Web.WebGet：httpget System.ServiceModel.Web.WebInvoke:httppost)</summary>
        /// <param name="obj">实体类</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回修改结果,大于0 返回修改成功,小于等于0 返回修改失败 </return>
        public virtual System.Threading.Tasks.Task<int> UpdateAsync(Entity obj, CancellationToken cancellationToken = default)
        {
            return new System.Threading.Tasks.Task<int>(() => Update(obj));
        }

        /// <summary>根据id删除实体类信息 (契约接口方法必须要有System.ServiceModel.OperationContract注解 支持web 必须要有 System.ServiceModel.Web.WebGet：httpget System.ServiceModel.Web.WebInvoke:httppost)</summary>
        /// <param name="id">id</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回删除结果,大于0 返回删除成功,小于等于0 返回删除失败 </return>
        public virtual System.Threading.Tasks.Task<int> DeleteAsync(Key id, CancellationToken cancellationToken = default)
        {
            return new System.Threading.Tasks.Task<int>(() => Delete(id));
        }

        /// <summary>根据id删除实体类信息(多删除) (契约接口方法必须要有System.ServiceModel.OperationContract注解 支持web 必须要有 System.ServiceModel.Web.WebGet：httpget System.ServiceModel.Web.WebInvoke:httppost)</summary>
        /// <param name="ids">id</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回删除结果(多删除),大于0 返回删除成功(多删除),小于等于0 返回删除失败(多删除) </return>
        public virtual System.Threading.Tasks.Task<int> DeleteListAsync(Key[] ids, CancellationToken cancellationToken = default)
        {
            return new System.Threading.Tasks.Task<int>(() => DeleteList(ids));
        }

        /// <summary>根据条件查询实体类数据集信息 (契约接口方法必须要有System.ServiceModel.OperationContract注解 支持web 必须要有 System.ServiceModel.Web.WebGet：httpget System.ServiceModel.Web.WebInvoke:httppost)</summary>
        /// <param name="obj">实体类</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回实体类数据集信息 </return>
        public virtual System.Threading.Tasks.Task<System.Collections.Generic.List<Entity>> FindListAsync(Entity obj, CancellationToken cancellationToken = default)
        {
            return new System.Threading.Tasks.Task<System.Collections.Generic.List<Entity>>(() => FindList(obj));
        }

        /// <summary>根据条件查询实体类数据集数量信息 (契约接口方法必须要有System.ServiceModel.OperationContract注解 支持web 必须要有 System.ServiceModel.Web.WebGet：httpget System.ServiceModel.Web.WebInvoke:httppost)</summary>
        /// <param name="obj">实体类</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回实体类数据集数量信息</return>
        public virtual System.Threading.Tasks.Task<int> CountAsync(Entity obj, CancellationToken cancellationToken = default)
        {
            return new System.Threading.Tasks.Task<int>(() => Count(obj));
        }

        /// <summary>根据条件及分页查询实体类数据集信息 (契约接口方法必须要有System.ServiceModel.OperationContract注解 支持web 必须要有 System.ServiceModel.Web.WebGet：httpget System.ServiceModel.Web.WebInvoke:httppost)</summary>
        /// <param name="obj">实体类</param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回实体类数据集信息</return>
        public virtual System.Threading.Tasks.Task<System.Collections.Generic.List<Entity>> FindListByPageAsync(Entity obj, int page, int size, CancellationToken cancellationToken = default)
        {
            return new System.Threading.Tasks.Task<System.Collections.Generic.List<Entity>>(() => FindListByPage(obj, page, size));
        }

        /// <summary>根据条件及分页查询实体类数据集信息和实体类数据集数量信息 (契约接口方法必须要有System.ServiceModel.OperationContract注解 支持web 必须要有 System.ServiceModel.Web.WebGet：httpget System.ServiceModel.Web.WebInvoke:httppost)</summary>
        /// <param name="obj">实体类</param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回实体类数据集信息和实体类数据集数量信息</return>
        public virtual System.Threading.Tasks.Task<Utility.Model.ResultModel<Entity>> FindResultModelByPageAsync(Entity obj, int page, int size, CancellationToken cancellationToken = default)
        {
            return new System.Threading.Tasks.Task<Model.ResultModel<Entity>>(() => FindResultModelByPage(obj, page, size));
        }
#endif
        }
}
#endif