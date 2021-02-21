#if true
//#if NET40 || NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48
using System.Threading;
using Utility.DAL;

namespace Utility.Wcf.DAL
{
    /// <summary>WCF 实体 数据访问层接口实现  </summary>
    public    class WcfDAL<Client,Service,Entity,Key>: IDAL<Entity, Key> 
		where Client:ServiceClient<Service, Entity,  Key>,new()
		where Service : class, IService<Entity, Key>
        where Entity : class
    {

        /// <summary>
        /// 
        /// </summary>
        protected string Name;

        /// <summary>
        /// 
        /// </summary>
        protected Client ClientProxy;

        /// <summary>添加实体类信息 (契约接口方法必须要有System.ServiceModel.OperationContract注解 支持web 必须要有 System.ServiceModel.Web.WebGet：httpget System.ServiceModel.Web.WebInvoke:httppost)</summary>
        /// <param name="obj">实体类</param>
        ///<return>返回添加结果,大于0 返回添加成功,小于等于0 返回添加失败 </return>
        public virtual int Insert(Entity obj)
        {
            using (Client client=new Client())
            {
                return client.Insert(obj);
            }
        }

        /// <summary>修改实体类信息 (契约接口方法必须要有System.ServiceModel.OperationContract注解 支持web 必须要有 System.ServiceModel.Web.WebGet：httpget System.ServiceModel.Web.WebInvoke:httppost)</summary>
        /// <param name="obj">实体类</param>
        ///<return>返回修改结果,大于0 返回修改成功,小于等于0 返回修改失败 </return>
        public virtual int Update(Entity obj)
        {
            using (Client client = new Client())
            {
                return client.Update(obj);
            }
        }

        /// <summary>根据id删除实体类信息 (契约接口方法必须要有System.ServiceModel.OperationContract注解 支持web 必须要有 System.ServiceModel.Web.WebGet：httpget System.ServiceModel.Web.WebInvoke:httppost)</summary>
        /// <param name="id">id</param>
        ///<return>返回删除结果,大于0 返回删除成功,小于等于0 返回删除失败 </return>
        public virtual int Delete(Key id)
        {
            using (Client client = new Client())
            {
                return client.Delete(id);
            }
        }

        /// <summary>根据id删除实体类信息(多删除) (契约接口方法必须要有System.ServiceModel.OperationContract注解 支持web 必须要有 System.ServiceModel.Web.WebGet：httpget System.ServiceModel.Web.WebInvoke:httppost)</summary>
        /// <param name="ids">id</param>
        ///<return>返回删除结果(多删除),大于0 返回删除成功(多删除),小于等于0 返回删除失败(多删除) </return>
        public virtual int DeleteList(Key[] ids)
        {
            using (System.ServiceModel.ChannelFactory<Service> channel = new System.ServiceModel.ChannelFactory<Service>(Name))
            {
                var wcfService = channel.CreateChannel();
                return wcfService.DeleteList(ids);
            }
        }

        /// <summary>根据条件查询实体类数据集信息 (契约接口方法必须要有System.ServiceModel.OperationContract注解 支持web 必须要有 System.ServiceModel.Web.WebGet：httpget System.ServiceModel.Web.WebInvoke:httppost)</summary>
        /// <param name="obj">实体类</param>
        ///<return>返回实体类数据集信息 </return>
        public virtual System.Collections.Generic.List<Entity> FindList(Entity obj)
        {
            using (System.ServiceModel.ChannelFactory<Service> channel = new System.ServiceModel.ChannelFactory<Service>(Name))
            {
                var wcfService = channel.CreateChannel();
                return wcfService.FindList(obj);
            }
        }

        /// <summary>根据条件查询实体类数据集数量信息 (契约接口方法必须要有System.ServiceModel.OperationContract注解 支持web 必须要有 System.ServiceModel.Web.WebGet：httpget System.ServiceModel.Web.WebInvoke:httppost)</summary>
        /// <param name="obj">实体类</param>
        ///<return>返回实体类数据集数量信息</return>
        public virtual int Count(Entity obj)
        {
            using (Client client = new Client())
            {
                return client.Count(obj);
            }
        }

        /// <summary>根据条件及分页查询实体类数据集信息 (契约接口方法必须要有System.ServiceModel.OperationContract注解 支持web 必须要有 System.ServiceModel.Web.WebGet：httpget System.ServiceModel.Web.WebInvoke:httppost)</summary>
        /// <param name="obj">实体类</param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        ///<return>返回实体类数据集信息</return>
        public virtual System.Collections.Generic.List<Entity> FindListByPage(Entity obj, int page, int size)
        {
            using (Client client = new Client())
            {
                return client.FindListByPage(obj,page,size);
            }
        }

        /// <summary>根据条件及分页查询实体类数据集信息和实体类数据集数量信息 (契约接口方法必须要有System.ServiceModel.OperationContract注解 支持web 必须要有 System.ServiceModel.Web.WebGet：httpget System.ServiceModel.Web.WebInvoke:httppost)</summary>
        /// <param name="obj">实体类</param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        ///<return>返回实体类数据集信息和实体类数据集数量信息</return>
        public virtual Utility.Model.ResultModel<Entity> FindResultModelByPage(Entity obj, int page, int size)
        {
            using (Client client = new Client())
            {
                return client.FindResultModelByPage(obj, page, size);
            }
        }


        /// <summary>添加实体类信息 (契约接口方法必须要有System.ServiceModel.OperationContract注解 支持web 必须要有 System.ServiceModel.Web.WebGet：httpget System.ServiceModel.Web.WebInvoke:httppost)</summary>
        /// <param name="obj">实体类</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回添加结果,大于0 返回添加成功,小于等于0 返回添加失败 </return>
        public virtual System.Threading.Tasks.Task<int> InsertAsync(Entity obj, CancellationToken cancellationToken = default)
        {
            using (Client client = new Client())
            {
                return client.InsertAsync(obj,cancellationToken);
            }
        }

        /// <summary>修改实体类信息 (契约接口方法必须要有System.ServiceModel.OperationContract注解 支持web 必须要有 System.ServiceModel.Web.WebGet：httpget System.ServiceModel.Web.WebInvoke:httppost)</summary>
        /// <param name="obj">实体类</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回修改结果,大于0 返回修改成功,小于等于0 返回修改失败 </return>
        public virtual System.Threading.Tasks.Task<int> UpdateAsync(Entity obj, CancellationToken cancellationToken = default)
        {
            using (Client client = new Client())
            {
                return client.UpdateAsync(obj, cancellationToken);
            }
        }

        /// <summary>根据id删除实体类信息 (契约接口方法必须要有System.ServiceModel.OperationContract注解 支持web 必须要有 System.ServiceModel.Web.WebGet：httpget System.ServiceModel.Web.WebInvoke:httppost)</summary>
        /// <param name="id">id</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回删除结果,大于0 返回删除成功,小于等于0 返回删除失败 </return>
        public virtual System.Threading.Tasks.Task<int> DeleteAsync(Key id, CancellationToken cancellationToken = default)
        {
            using (Client client = new Client())
            {
                return client.DeleteAsync(id, cancellationToken);
            }
        }

        /// <summary>根据id删除实体类信息(多删除) (契约接口方法必须要有System.ServiceModel.OperationContract注解 支持web 必须要有 System.ServiceModel.Web.WebGet：httpget System.ServiceModel.Web.WebInvoke:httppost)</summary>
        /// <param name="ids">id</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回删除结果(多删除),大于0 返回删除成功(多删除),小于等于0 返回删除失败(多删除) </return>
        public virtual System.Threading.Tasks.Task<int> DeleteListAsync(Key[] ids, CancellationToken cancellationToken = default)
        {
            using (Client client = new Client())
            {
                return client.DeleteListAsync(ids, cancellationToken);
            }
        }

        /// <summary>根据条件查询实体类数据集信息 (契约接口方法必须要有System.ServiceModel.OperationContract注解 支持web 必须要有 System.ServiceModel.Web.WebGet：httpget System.ServiceModel.Web.WebInvoke:httppost)</summary>
        /// <param name="obj">实体类</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回实体类数据集信息 </return>
        public virtual System.Threading.Tasks.Task<System.Collections.Generic.List<Entity>> FindListAsync(Entity obj, CancellationToken cancellationToken = default)
        {
            using (Client client = new Client())
            {
                return client.FindListAsync(obj, cancellationToken);
            }
        }

        /// <summary>根据条件查询实体类数据集数量信息 (契约接口方法必须要有System.ServiceModel.OperationContract注解 支持web 必须要有 System.ServiceModel.Web.WebGet：httpget System.ServiceModel.Web.WebInvoke:httppost)</summary>
        /// <param name="obj">实体类</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回实体类数据集数量信息</return>
        public virtual System.Threading.Tasks.Task<int> CountAsync(Entity obj, CancellationToken cancellationToken = default)
        {
            using (Client client = new Client())
            {
                return client.CountAsync(obj, cancellationToken);
            }
        }

        /// <summary>根据条件及分页查询实体类数据集信息 (契约接口方法必须要有System.ServiceModel.OperationContract注解 支持web 必须要有 System.ServiceModel.Web.WebGet：httpget System.ServiceModel.Web.WebInvoke:httppost)</summary>
        /// <param name="obj">实体类</param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回实体类数据集信息</return>
        public virtual System.Threading.Tasks.Task<System.Collections.Generic.List<Entity>> FindListByPageAsync(Entity obj, int page, int size, CancellationToken cancellationToken = default)
        {
            using (Client client = new Client())
            {
                return client.FindListByPageAsync(obj,page,size, cancellationToken);
            }
        }

        /// <summary>根据条件及分页查询实体类数据集信息和实体类数据集数量信息 (契约接口方法必须要有System.ServiceModel.OperationContract注解 支持web 必须要有 System.ServiceModel.Web.WebGet：httpget System.ServiceModel.Web.WebInvoke:httppost)</summary>
        /// <param name="obj">实体类</param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回实体类数据集信息和实体类数据集数量信息</return>
        public virtual System.Threading.Tasks.Task<Utility.Model.ResultModel<Entity>> FindResultModelByPageAsync(Entity obj, int page, int size, CancellationToken cancellationToken = default)
        {
            using (Client client = new Client())
            {
                return client.FindResultModelByPageAsync(obj, page, size, cancellationToken);
            }
        }
    }
}
#endif