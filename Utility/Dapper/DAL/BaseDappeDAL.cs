#if !(NET10 || NET11 || NET20 || NET30 || NET35  || NETCOREAPP1_0 || NETCOREAPP1_1 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
//#if NET40 ||NET45 || NET451 || NET452 || NET46 ||NET461 || NET462 || NET47 || NET471 || NET472 || NET48  || NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0 || NETCOREAPP3_1 || NET5_0 || NETSTANDARD2_0 || NETSTANDARD2_1
using System.Linq;
using System.Data;
using Dapper;
using Utility.Model;
using System.Threading.Tasks;
using Utility.DAL;
using Utility.Dapper.Uow;
using System.Collections.Generic;
using System.Threading;
using Utility.Dapper.Repositories;

namespace Utility.Dapper.DAL
{
    /// <summary> 
    /// dapper 数据访问层接口 实现
    /// 没法统一接口啊 提示 要么循环 要么参数统一替换(多继承接口出现问题)
    /// </summary>
    public class BaseDapperDAL<Model, Key> : BaseDapperDAL<Model>, IDAL<Model, Key> where
        Model : class, IModel<Key>
    {
        /// <summary> 构造注入数据库连接对象</summary>
        /// <param name="connection">数据库连接对象</param>
        public BaseDapperDAL(IDbConnection connection) : base(connection)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual string GetTable()
        {
            return string.Empty;
        }

        /// <summary>根据id删除用实体类信息</summary>
        /// <param name="id">实体类 id</param>
        ///<return>返回删除实体类信息结果,大于0 返回删除成功,小于等于0 返回删除失败 </return>
        public virtual int Delete(Key id)
        {
            string sql = $"DELETE FROM {GetTable()} WHERE  Id =@Id";
            return DapperRepository.Connection.Execute(sql, new { Id = id });
        }


        /// <summary>根据id删除实体类信息(多删除)</summary>
        /// <param name="ids">id</param>
        ///<return>返回删除结果(多删除),大于0 返回删除成功(多删除),小于等于0 返回删除失败(多删除) </return>
        public virtual int DeleteList(Key[] ids)
        {
            string sql = DALHelper.DeleteListSql(ids, GetTable());
            return DapperRepository.Connection.Execute(sql);
        }

        /// <summary>根据id删除实体类信息</summary>
        /// <param name="id">id</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回删除结果,大于0 返回删除成功,小于等于0 返回删除失败 </return>
        public virtual Task<int> DeleteAsync(Key id, CancellationToken cancellationToken = default)
        {
            var res = Delete(id);
            return new  Task<int>(()=>res);
        }

        /// <summary>根据id删除实体类信息(多删除)</summary>
        /// <param name="ids">id</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回删除结果(多删除),大于0 返回删除成功(多删除),小于等于0 返回删除失败(多删除) </return>
        public virtual Task<int> DeleteListAsync(Key[] ids, CancellationToken cancellationToken = default)
        {
            var res = DeleteList(ids);
            return new Task<int>(() => res);
        }

    }

    /// <summary>dapper 数据访问层接口基类</summary>
    public class BaseDapperDAL<T> where T : class
    {
        /// <summary>
        /// 
        /// </summary>
        protected BaseDapperRepository<T> DapperRepository { get; set; }
        /// <summary>
        /// 
        /// </summary>

        public IDbConnection Connection { get; set; }

        /// <summary> 构造注入数据库连接对象</summary>
        /// <param name="connection">数据库连接对象</param>
        public BaseDapperDAL(IDbConnection connection)
        {
            Connection = connection;
            DapperRepository = new BaseDapperRepository<T>(connection);
        }
        /// <summary>添加实体类信息</summary>
        /// <param name="obj">实体类</param>
        ///<return>返回添加结果,大于0 返回添加成功,小于等于0 返回添加失败 </return>
        public virtual int Insert(T obj)
        {
            var res = DapperRepository.Connection.Insert(obj);
            return res.HasValue ? res.Value : 0;
        }

        /// <summary>修改实体类信息</summary>
        /// <param name="obj">实体类</param>
        ///<return>返回修改结果,大于0 返回修改成功,小于等于0 返回修改失败 </return>
        public virtual int Update(T obj)
        {
            DapperRepository.Update(obj);
            return 1;
        }

        /// <summary>根据id删除实体类信息</summary>
        /// <param name="id">id</param>
        ///<return>返回删除结果,大于0 返回删除成功,小于等于0 返回删除失败 </return>
        public virtual int Delete(object  id)
        {
            DapperRepository.UnitWork.Delete<T>(id);
            return 1;
        }

        /// <summary>根据条件查询实体类数据集数量信息</summary>
        /// <param name="obj">实体类</param>
        ///<return>返回实体类数据集数量信息</return>
        public virtual int Count(T obj)
        {
            var res = DapperRepository.UnitWork.DapperTemplate.Count<T>(QueryWhere(obj),obj);
            return res;
        }

        /// <summary>根据条件查询实体类数据集信息</summary>
        /// <param name="obj">实体类</param>
        ///<return>返回实体类数据集信息</return>
        public virtual List<T> FindList(T obj)
        {
            var res = DapperRepository.UnitWork.DapperTemplate.FindList<T>(QueryWhere(obj), obj).ToList();
            return res;
        }

        /// <summary>根据条件及分页查询实体类数据集信息</summary>
        /// <param name="obj">实体类</param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        ///<return>返回实体类数据集信息</return>
        public virtual List<T> FindListByPage(T obj, int page, int size)
        {
            var res = DapperRepository.Connection.GetListPaged<T>(page,size,QueryWhere(obj),/*"  ORDER BY  ID ASC "*/"", obj).ToList();
            return res;
        }

        /// <summary>根据条件及分页查询实体类数据集信息和实体类数据集数量信息</summary>
        /// <param name="obj">实体类</param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        ///<return>返回实体类数据集信息和实体类数据集数量信息</return>
        public virtual ResultModel<T> FindResultModelByPage(T obj, int page, int size)
        {
            var where = QueryWhere(obj);
            var tuple = DapperRepository.UnitWork.DapperTemplate.FindTupleByPage<T>(obj,where,page, size);
            ResultModel<T> result = new ResultModel<T>(tuple,page,size);
            return result;
        }

        /// <summary>查询wehere sql </summary>
        /// <param name="obj">地址信息</param>
        /// <returns></returns>
        protected virtual string QueryWhere(T obj)
        {
            return string.Empty;
        }

#region async
        /// <summary>添加实体类信息</summary>
        /// <param name="obj">实体类</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回添加结果,大于0 返回添加成功,小于等于0 返回添加失败 </return>
        public virtual
#if !NET40
            async
#endif
            Task<int> InsertAsync(T obj, CancellationToken cancellationToken = default(CancellationToken))
        {
#if !NET40
            var res =await DapperRepository.Connection.InsertAsync(obj);
            return res.HasValue ? res.Value : 0;
#else
            return new Task<int>(() => Insert(obj));
#endif
        }

        /// <summary>修改实体类信息</summary>
        /// <param name="obj">实体类</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回修改结果,大于0 返回修改成功,小于等于0 返回修改失败 </return>
        public virtual Task<int> UpdateAsync(T obj,CancellationToken cancellationToken = default)
        {
            DapperRepository.UpdateAsync(obj);
            return new Task<int>(()=>1);
        }

        /// <summary>根据id删除实体类信息</summary>
        /// <param name="id">id</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回删除结果,大于0 返回删除成功,小于等于0 返回删除失败 </return>
        public virtual Task<int> DeleteAsync(object id, CancellationToken cancellationToken = default)
        {
            DapperRepository.UnitWork.DeleteAsync<T>(id);
            return new Task<int>(()=>1);
        }

        /// <summary>根据条件查询实体类数据集数量信息</summary>
        /// <param name="obj">实体类</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回实体类数据集数量信息</return>
        public virtual Task<int> CountAsync(T obj,CancellationToken cancellationToken = default)
        {
            var res = DapperRepository.UnitWork.DapperTemplate.CountAsync<T>(QueryWhere(obj), obj);
            return res;
        }

        /// <summary>根据条件查询实体类数据集信息</summary>
        /// <param name="obj">实体类</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回实体类数据集信息</return>
        public virtual
#if !NET40
            async
#endif
            Task<List<T>> FindListAsync(T obj, CancellationToken cancellationToken = default)
        {
#if !NET40
            var res =await DapperRepository.UnitWork.DapperTemplate.FindListAsync<T>(QueryWhere(obj), obj);
            var data = res.ToList();
            return data;
#else
            return new Task<List<T>>(()=>FindList(obj));
#endif

        }

        /// <summary>根据条件及分页查询实体类数据集信息</summary>
        /// <param name="obj">实体类</param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回实体类数据集信息</return>
        public virtual
#if !NET40
            async
#endif
            Task<List<T>> FindListByPageAsync(T obj, int page, int size,CancellationToken cancellationToken = default)
        {
#if !NET40
            var res = await DapperRepository.UnitWork.DapperTemplate.FindListByPageAsync<T>(QueryWhere(obj),/*"  ORDER BY  ID ASC "*/"", obj,page, size);
            var data = res.ToList();
            return data;
#else
            return new Task<List<T>>(()=>FindListByPage(obj,page,size));
#endif

        }

        /// <summary>根据条件及分页查询实体类数据集信息和实体类数据集数量信息</summary>
        /// <param name="obj">实体类</param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回实体类数据集信息和实体类数据集数量信息</return>
        public virtual
#if !NET40
            async
#endif
            Task<ResultModel<T>> FindResultModelByPageAsync(T obj, int page, int size,CancellationToken cancellationToken = default)
        {

#if !NET40
            var where = QueryWhere(obj);
            var tuple = await DapperRepository.UnitWork.DapperTemplate.FindTupleByPageAsync(obj, where, page, size);
            ResultModel<T> result = new ResultModel<T>(tuple, page, size);
            return result;
#else
return new Task<ResultModel<T>>(()=>FindResultModelByPage(obj,page,size));
#endif
        }

#endregion async
    }

    /// <summary>
    /// 
    /// </summary>
    public class BaseDapperDAL
    {
        /// <summary>
        /// 
        /// </summary>
        public DapperUnitWork DapperUnitWork { get;protected set; }
        /// <summary> 构造注入数据库连接对象</summary>
        /// <param name="connection">数据库连接对象</param>
        public BaseDapperDAL(IDbConnection connection) 
        {
            DapperUnitWork = new DapperUnitWork(connection);
        }
    }
}
#endif