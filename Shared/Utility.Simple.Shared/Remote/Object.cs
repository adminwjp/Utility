#if NET20 || NET30 || NET35 || NET40 || NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48
#if !(NET20 || NET30 || NET35)
using System.Threading.Tasks;
#endif
using System.Threading;
using Utility.Model;
using System.Collections.Generic;
using System;
using Utility.DAL;
using Utility.Json;
using Utility.ObjectMapping;

namespace Utility.Remote
{
    /// <summary>
    /// remote  基类
    /// </summary>
	/// <typeparam name="DALImpl"></typeparam>
	/// <typeparam name="ResultModel"></typeparam>
	/// <typeparam name="ListModel"></typeparam>
	/// <typeparam name="Entity">remote 模型</typeparam>
    /// <typeparam name="EntityModel">模型</typeparam>
    /// <typeparam name="Key"></typeparam>
    public class Object<DALImpl,ResultModel,ListModel,Entity, EntityModel, Key> : System.MarshalByRefObject, IObject<ResultModel,ListModel,Entity, Key> 
		where DALImpl : IDAL<EntityModel, Key>
		where ResultModel : Utility.Model.ResultModel<Entity>, new()
        where ListModel : System.Collections.Generic.List<Entity>,new()
		where Entity : class
        where EntityModel : class
    {
        /// <summary>
        /// 
        /// </summary>
        protected DALImpl DAL { get; set; }
        /// <summary>
        /// 
        /// </summary>
        protected IObjectMapper ObjectMapper { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Object()
        {
            //注意只能无参构造函数 传参数不支持怎么传 支持注册类型 这玩意服务器端怎么绑定了(比较麻烦参数结果都需要序列化 引用类型都需要指出序列化)
            //手动调用
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dAL"></param>
        public Object(DALImpl dAL)
        {
            DAL = dAL;
        }
        /// <summary>添加实体类信息</summary>
        /// <param name="obj">实体类</param>
        ///<return>返回添加结果,大于0 返回添加成功,小于等于0 返回添加失败 </return>
        public virtual int Insert(Entity obj)
        {
            var entity=ObjectMapper.Map<Entity, EntityModel>(obj);
            return DAL.Insert(entity);
        }

        /// <summary>修改实体类信息</summary>
        /// <param name="obj">实体类</param>
        ///<return>返回修改结果,大于0 返回修改成功,小于等于0 返回修改失败 </return>
        public virtual int Update(Entity obj)
        {
            var entity = ObjectMapper.Map<Entity, EntityModel>(obj);
            return DAL.Update(entity);
        }

        /// <summary>根据id删除实体类信息 </summary>
        /// <param name="id">id</param>
        ///<return>返回删除结果,大于0 返回删除成功,小于等于0 返回删除失败 </return>
        public virtual int Delete(Key id)
        {
            return DAL.Delete(id);
        }

        /// <summary>根据id删除实体类信息(多删除)</summary>
        /// <param name="ids">id</param>
        ///<return>返回删除结果(多删除),大于0 返回删除成功(多删除),小于等于0 返回删除失败(多删除) </return>
        public virtual int DeleteList(Key[] ids)
        {
            return DAL.DeleteList(ids);
        }

        /// <summary>根据条件查询实体类数据集信息 不支持 会出现异常 序列化失败</summary>
        /// <param name="obj">实体类</param>
        ///<return>返回实体类数据集信息 </return>
        public virtual ListModel FindList(Entity obj)
        {
            var entity = ObjectMapper.Map<Entity, EntityModel>(obj);
            var res= DAL.FindList(entity);
            var data=ObjectMapper.Map <System.Collections.Generic.List<EntityModel> , ListModel>(res);
            return data;
        }

        /// <summary>根据条件查询实体类数据集数量信息 不支持 会出现异常 序列化失败</summary>
        /// <param name="obj">实体类</param>
        ///<return>返回实体类数据集数量信息</return>
        public virtual int Count(Entity obj)
        {
            var entity = ObjectMapper.Map<Entity, EntityModel>(obj);
            return DAL.Count(entity);
        }

        /// <summary>根据条件及分页查询实体类数据集信息 不支持 会出现异常 序列化失败</summary>
        /// <param name="obj">实体类</param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        ///<return>返回实体类数据集信息</return>
        public virtual ListModel FindListByPage(Entity obj, int page, int size)
        {
            var entity = ObjectMapper.Map<Entity, EntityModel>(obj);
            var res = DAL.FindListByPage(entity,page,size);
            var data = ObjectMapper.Map<System.Collections.Generic.List<EntityModel>, ListModel>(res);
            return data;
        }

        /// <summary>根据条件及分页查询实体类数据集信息和实体类数据集数量信息 不支持 会出现异常 序列化失败</summary>
        /// <param name="obj">实体类</param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        ///<return>返回实体类数据集信息和实体类数据集数量信息</return>
        public virtual ResultModel FindResultModelByPage(Entity obj, int page, int size)
        {
            var entity = ObjectMapper.Map<Entity, EntityModel>(obj);
            var res = DAL.FindResultModelByPage(entity, page, size);
            var data = ObjectMapper.Map<Utility.Model.ResultModel<EntityModel>, ResultModel>(res);
            return data;
        }
    }


    /// <summary>
    /// Remote 实现
    /// </summary>
    /// <typeparam name="DALImpl">基于 <see cref="IDAL{Model, Key}"/> 实现</typeparam>
    /// <typeparam name="EntityModel">实体模型</typeparam>
    /// <typeparam name="Key">主键类型</typeparam>
    public class Object<DALImpl,EntityModel, Key> : System.MarshalByRefObject,IObject
		where DALImpl : IDAL<EntityModel, Key>
		where EntityModel : class
    {
        /// <summary>
        /// 基于 <see cref="IDAL{Model, Key}"/> 实现
        /// </summary>
        protected DALImpl DAL { get; set; }
        /// <summary>
        /// 
        /// </summary>
        protected IObjectMapper ObjectMapper { get; set; }
        /// <summary>
        /// Remote 实现
        /// </summary>
        public Object()
        {
            //注意只能无参构造函数 传参数不支持怎么传 支持注册类型 这玩意服务器端怎么绑定了(比较麻烦参数结果都需要序列化 引用类型都需要指出序列化)
            //手动调用
        }

        /// <summary>
        /// Remote 实现
        /// </summary>
        /// <param name="dAL">基于 <see cref="IDAL{Model, Key}"/> 实现</param>
        public Object(DALImpl dAL)
        {
            DAL = dAL;
        }
        /// <summary>添加实体类信息</summary>
        /// <param name="obj">实体类</param>
        ///<return>返回添加结果,大于0 返回添加成功,小于等于0 返回添加失败 </return>
        public virtual int Insert(string obj)
        {
            var entity = JsonHelper.ToObject<EntityModel>(obj);
            return DAL.Insert(entity);
        }

        /// <summary>修改实体类信息</summary>
        /// <param name="obj">实体类</param>
        ///<return>返回修改结果,大于0 返回修改成功,小于等于0 返回修改失败 </return>
        public virtual int Update(string obj)
        {
            var entity = JsonHelper.ToObject<EntityModel>(obj);
            return DAL.Update(entity);
        }

        /// <summary>根据id删除实体类信息 </summary>
        /// <param name="id">id</param>
        ///<return>返回删除结果,大于0 返回删除成功,小于等于0 返回删除失败 </return>
        public virtual int Delete(string id)
        {
            return DAL.Delete((Key)Convert.ChangeType(id,typeof(Key)));
        }

        /// <summary>根据id删除实体类信息(多删除)</summary>
        /// <param name="ids">id</param>
        ///<return>返回删除结果(多删除),大于0 返回删除成功(多删除),小于等于0 返回删除失败(多删除) </return>
        public virtual int DeleteList(string ids)
        {
            var entity = JsonHelper.ToObject<Key[]>(ids);
            return DAL.DeleteList(entity);
        }

        /// <summary>根据条件查询实体类数据集信息 不支持 会出现异常 序列化失败</summary>
        /// <param name="obj">实体类</param>
        ///<return>返回实体类数据集信息 </return>
        public virtual string FindList(string obj)
        {
            var entity = JsonHelper.ToObject<EntityModel>(obj);
            var res = DAL.FindList(entity);
            var data =JsonHelper.ToJson(res);
            return data;
        }

        /// <summary>根据条件查询实体类数据集数量信息 不支持 会出现异常 序列化失败</summary>
        /// <param name="obj">实体类</param>
        ///<return>返回实体类数据集数量信息</return>
        public virtual int Count(string obj)
        {
            var entity = JsonHelper.ToObject<EntityModel>(obj);
            return DAL.Count(entity);
        }

        /// <summary>根据条件及分页查询实体类数据集信息 不支持 会出现异常 序列化失败</summary>
        /// <param name="obj">实体类</param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        ///<return>返回实体类数据集信息</return>
        public virtual string FindListByPage(string obj, int page, int size)
        {
            var entity = JsonHelper.ToObject<EntityModel>(obj);
            var res = DAL.FindListByPage(entity, page, size);
            var data = JsonHelper.ToJson(res);
            return data;
        }

        /// <summary>根据条件及分页查询实体类数据集信息和实体类数据集数量信息 不支持 会出现异常 序列化失败</summary>
        /// <param name="obj">实体类</param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        ///<return>返回实体类数据集信息和实体类数据集数量信息</return>
        public  virtual string FindResultModelByPage(string obj, int page, int size)
        {
            var entity = JsonHelper.ToObject<EntityModel>(obj);
            var res = DAL.FindResultModelByPage(entity, page, size);
            var data = JsonHelper.ToJson(res);
            return data;
        }
    }
	
	/// <summary>
    /// remote dal 基类
    /// </summary>
	/// <typeparam name="Object">remote 基类实现</typeparam>
	/// <typeparam name="T"> 模型</typeparam>
	/// <typeparam name="Key">主键类型</typeparam>
    public class RemoteDAL<Object, T, Key>: IDAL<T, Key> 
		where Object:IObject,new()
		where T : class
       
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
            return @object.Insert(JsonHelper.ToJson(obj));
        }

         /// <summary>修改实体类信息</summary>
        /// <param name="obj">实体类</param>
        ///<return>返回修改结果,大于0 返回修改成功,小于等于0 返回修改失败 </return>
        public virtual int Update(T obj)
        {
            return @object.Update(JsonHelper.ToJson(obj));
        }

         /// <summary>根据id删除实体类信息</summary>
        /// <param name="id">id</param>
        ///<return>返回删除结果,大于0 返回删除成功,小于等于0 返回删除失败 </return>
        public virtual int Delete(Key id)
        {
            return @object.Delete(id.ToString());
        }

        /// <summary>根据id删除实体类信息(多删除)</summary>
        /// <param name="ids">id</param>
        ///<return>返回删除结果(多删除),大于0 返回删除成功(多删除),小于等于0 返回删除失败(多删除) </return>
        public virtual int DeleteList(Key[] ids)
        {
            return @object.DeleteList(JsonHelper.ToJson(ids));
        }

         /// <summary>根据条件查询实体类数据集信息</summary>
        /// <param name="obj">实体类</param>
        ///<return>返回实体类数据集信息 </return>
        public virtual List<T> FindList(T obj)
        {
            var res= @object.FindList(JsonHelper.ToJson(obj));
            var data = JsonHelper.ToObject< List<T>>(res);
            return data;
        }

          /// <summary>根据条件查询实体类数据集数量信息</summary>
        /// <param name="obj">实体类</param>
        ///<return>返回实体类数据集数量信息</return>
        public  virtual int Count(T obj)
        {
            return @object.Count(JsonHelper.ToJson(obj));
        }


        /// <summary>根据条件及分页查询实体类数据集信息</summary>
        /// <param name="obj">实体类</param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        ///<return>返回实体类数据集信息</return>
        public virtual List<T> FindListByPage(T obj, int page, int size)
        {
            var res = @object.FindListByPage(JsonHelper.ToJson(obj),page,size);
            var data = JsonHelper.ToObject<List<T>>(res);
            return data;
        }

        /// <summary>根据条件及分页查询实体类数据集信息和实体类数据集数量信息</summary>
        /// <param name="obj">实体类</param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        ///<return>返回实体类数据集信息和实体类数据集数量信息</return>
        public virtual ResultModel<T> FindResultModelByPage(T obj, int page, int size)
        {
            var res = @object.FindResultModelByPage(JsonHelper.ToJson(obj), page, size);
            var data = JsonHelper.ToObject<ResultModel<T>>(res);
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

