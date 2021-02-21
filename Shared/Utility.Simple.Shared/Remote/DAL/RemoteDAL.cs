#if NET20 || NET30 || NET35 || NET40 || NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
#if !(NET20 || NET30 || NET35)
using System.Threading.Tasks;
#endif
using Utility.DAL;
using Utility.Model;
using Utility.ObjectMapping;

namespace Utility.Remote.DAL
{
    /// <summary>
    /// remote dal 基类
    /// </summary>
	/// <typeparam name="Object">remote 基类实现</typeparam>
	/// <typeparam name="ResultModel"></typeparam>
	/// <typeparam name="ListModel"></typeparam>
	/// <typeparam name="T">模型</typeparam>
	/// <typeparam name="TModel">remote 模型</typeparam>
	/// <typeparam name="Key"></typeparam>
    public class RemoteDAL<Object,ResultModel,ListModel,T,TModel, Key>: IDAL<T, Key> 
		where Object:IObject<ResultModel, ListModel, TModel, Key >,new()
		where ResultModel : ResultModel<TModel>, new()
        where ListModel:List<TModel>,new()
		where T : class
		where TModel : class
       
    {
        /// <summary>
        /// 
        /// </summary>
        protected IObjectMapper ObjectMapper { get; set; }
        /// <summary>
        /// 
        /// </summary>
        protected Object @object;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        public RemoteDAL(string url)
        {
            System.Runtime.Remoting.RemotingConfiguration.RegisterActivatedClientType(typeof(Object), url);//http 不支持 TCP://localhost:20001/test
            @object = new Object();
        }


        /// <summary>添加实体类信息</summary>
        /// <param name="obj">实体类</param>
        ///<return>返回添加结果,大于0 返回添加成功,小于等于0 返回添加失败 </return>
        public virtual int Insert(T obj)
        {
            var model = ObjectMapper.Map<T, TModel>(obj);
            return @object.Insert(model);
        }

         /// <summary>修改实体类信息</summary>
        /// <param name="obj">实体类</param>
        ///<return>返回修改结果,大于0 返回修改成功,小于等于0 返回修改失败 </return>
        public  virtual int Update(T obj)
        {
            var model = ObjectMapper.Map<T, TModel>(obj);
            return @object.Update(model);
        }

         /// <summary>根据id删除实体类信息</summary>
        /// <param name="id">id</param>
        ///<return>返回删除结果,大于0 返回删除成功,小于等于0 返回删除失败 </return>
        public virtual int Delete(Key id)
        {
            return @object.Delete(id);
        }

        /// <summary>根据id删除实体类信息(多删除)</summary>
        /// <param name="ids">id</param>
        ///<return>返回删除结果(多删除),大于0 返回删除成功(多删除),小于等于0 返回删除失败(多删除) </return>
        public virtual int DeleteList(Key[] ids)
        {
            return @object.DeleteList(ids);
        }

         /// <summary>根据条件查询实体类数据集信息</summary>
        /// <param name="obj">实体类</param>
        ///<return>返回实体类数据集信息 </return>
        public virtual List<T> FindList(T obj)
        {
            var model = ObjectMapper.Map<T, TModel>(obj);
            var res= @object.FindList(model);
            var data = ObjectMapper.Map<ListModel, List<T>>(res);
            return data;
        }

          /// <summary>根据条件查询实体类数据集数量信息</summary>
        /// <param name="obj">实体类</param>
        ///<return>返回实体类数据集数量信息</return>
        public virtual int Count(T obj)
        {
            var model = ObjectMapper.Map<T, TModel>(obj);
            return @object.Count(model);
        }


        /// <summary>根据条件及分页查询实体类数据集信息</summary>
        /// <param name="obj">实体类</param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        ///<return>返回实体类数据集信息</return>
        public virtual List<T> FindListByPage(T obj, int page, int size)
        {
            var model = ObjectMapper.Map<T, TModel>(obj);
            var res = @object.FindListByPage(model,page,size);
            var data = ObjectMapper.Map<ListModel,List<T>>(res);
            return data;
        }

        /// <summary>根据条件及分页查询实体类数据集信息和实体类数据集数量信息</summary>
        /// <param name="obj">实体类</param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        ///<return>返回实体类数据集信息和实体类数据集数量信息</return>
        public virtual ResultModel<T> FindResultModelByPage(T obj, int page, int size)
        {
            var model = ObjectMapper.Map<T, TModel>(obj);
            var res = @object.FindResultModelByPage(model, page, size);
            var data = ObjectMapper.Map<ResultModel,ResultModel<T>>(res);
            return data;
        }


#if !(NET20 || NET30 || NET35)

        /// <summary>根据条件及分页查询实体类数据集信息和实体类数据集数量信息</summary>
        /// <param name="obj">实体类</param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回实体类数据集信息和实体类数据集数量信息</return>
        public virtual Task<ResultModel<T>> FindResultModelByPageAsync(T obj, int page, int size,CancellationToken cancellationToken = default)
        {
            return new Task<ResultModel<T>>(() => FindResultModelByPage(obj, page, size));
        }


        /// <summary>添加实体类信息</summary>
        /// <param name="obj">实体类</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回添加结果,大于0 返回添加成功,小于等于0 返回添加失败 </return>
        public virtual Task<int> InsertAsync(T obj, CancellationToken cancellationToken = default)
        {
            return new Task<int>(() => Insert(obj));
        }
        /// <summary>修改实体类信息</summary>
        /// <param name="obj">实体类</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回修改结果,大于0 返回修改成功,小于等于0 返回修改失败 </return>
        public virtual Task<int> UpdateAsync(T obj,CancellationToken cancellationToken = default)
        {
            return new Task<int>(() => Update(obj));
        }


        /// <summary>根据id删除实体类信息</summary>
        /// <param name="id">id</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回删除结果,大于0 返回删除成功,小于等于0 返回删除失败 </return>
        public virtual Task<int> DeleteAsync(Key id, CancellationToken cancellationToken = default)
        {
            return new Task<int>(() => Delete(id));
        }


        /// <summary>根据id删除实体类信息(多删除)</summary>
        /// <param name="ids">id</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回删除结果(多删除),大于0 返回删除成功(多删除),小于等于0 返回删除失败(多删除) </return>
        public virtual Task<int> DeleteListAsync(Key[] ids,CancellationToken cancellationToken = default)
        {
            return new Task<int>(() => DeleteList(ids));
        }


        /// <summary>根据条件查询实体类数据集信息</summary>
        /// <param name="obj">实体类</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回实体类数据集信息 </return>
        public virtual Task<List<T>> FindListAsync(T obj,CancellationToken cancellationToken = default)
        {
            return new Task<List<T>>(() => FindList(obj));
        }


        /// <summary>根据条件查询实体类数据集数量信息</summary>
        /// <param name="obj">实体类</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回实体类数据集数量信息</return>
        public virtual Task<int> CountAsync(T obj, CancellationToken cancellationToken = default)
        {
            return new Task<int>(() => Count(obj));
        }

        /// <summary>根据条件及分页查询实体类数据集信息</summary>
        /// <param name="obj">实体类</param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回实体类数据集信息</return>
        public virtual Task<List<T>> FindListByPageAsync(T obj, int page, int size,CancellationToken cancellationToken = default)
        {
            return new Task<List<T>>(() => FindListByPage(obj, page, size));
        }

#endif
        }
    }
#endif