#if !(NET10 || NET11 || NET20 || NET30 || NET35 || NETCOREAPP1_0 || NETCOREAPP1_1 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
//#if NET40 ||NET45 || NET451 || NET452 || NET46 ||NET461 || NET462 || NET47 || NET471 || NET472 || NET48  || NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0 || NETCOREAPP3_1 || NET5_0 || NETSTANDARD2_0 || NETSTANDARD2_1
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Utility.Model;
using Utility.Page;

namespace Utility.Dapper.DAL
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="Model"></typeparam>
    /// <typeparam name="Key"></typeparam>
    public class DapperDALHelper<Model, Key>: DapperDALHelper<Model> where Model : class, Utility.Model.IModel<Key>
    {

    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DapperDALHelper<T> : DapperDALHelper where T : class
    {
        /// <summary>
        /// 
        /// </summary>
        protected static Func<T, string> WhereEmpty = (it) => string.Empty;
        private static Func<T, string> where;

        /// <summary>
        /// 
        /// </summary>
        public static Func<T, string> Where { get => where?? WhereEmpty; set => where = value; }

        /// <summary>添加实体类信息</summary>
        /// <param name="obj">实体类</param>
        ///<return>返回添加结果,大于0 返回添加成功,小于等于0 返回添加失败 </return>
        public static int Insert(T obj)
        {
            var res = DapperTemplate.Insert(Connection, obj);
            return res.HasValue ? res.Value : -1;
        }

        /// <summary>修改实体类信息</summary>
        /// <param name="obj">实体类</param>
        ///<return>返回修改结果,大于0 返回修改成功,小于等于0 返回修改失败 </return>
        public static int Update(T obj)
        {
            var res = DapperTemplate.Update(Connection, obj);
            return res;
        }

        /// <summary>根据id删除实体类信息</summary>
        /// <param name="id">id</param>
        ///<return>返回删除结果,大于0 返回删除成功,小于等于0 返回删除失败 </return>
        public static int Remove(object id)
        {
            var res = DapperTemplate.Delete<T>(Connection, id);
            return res;
        }

        /// <summary>根据条件查询实体类数据集数量信息</summary>
        /// <param name="obj">实体类</param>
        ///<return>返回实体类数据集数量信息</return>
        public static int Count(T obj)
        {
            var res = DapperTemplate.Count<T>(Connection, Where(obj), obj);
            return res;
        }

        /// <summary>根据条件查询实体类数据集信息</summary>
        /// <param name="obj">实体类</param>
        ///<return>返回实体类数据集信息</return>
        public static List<T> Query(T obj)
        {
            var res = DapperTemplate.FindList<T>(Connection, Where(obj), obj).ToList();
            return res;
        }

        /// <summary>根据条件及分页查询实体类数据集信息</summary>
        /// <param name="obj">实体类</param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        ///<return>返回实体类数据集信息</return>
        public static List<T> QueryByPage(T obj, int page, int size)
        {
            var res = DapperTemplate.FindListByPage<T>(Connection, Where(obj),/*"  ORDER BY  ID ASC "*/"", obj, page, size).ToList();
            return res;
        }

        /// <summary>根据条件及分页查询实体类数据集信息和实体类数据集数量信息</summary>
        /// <param name="obj">实体类</param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        ///<return>返回实体类数据集信息和实体类数据集数量信息</return>
        public static ResultModel<T> QueryResultModelByPage(T obj, int page, int size)
        {
            var where = Where(obj);
            PageHelper.Set(ref page, ref size);
            var datas = DapperTemplate.FindTupleByPage<T>(Connection,obj, where, page, size);
            ResultModel<T> result = new ResultModel<T>(datas, page, size);
            return result;
        }


#region async

#if !NET40
        /// <summary>添加实体类信息</summary>
        /// <param name="obj">实体类</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回添加结果,大于0 返回添加成功,小于等于0 返回添加失败 </return>
        public static  Task<int> InsertAsync(T obj, CancellationToken cancellationToken = default)
        {
            var res =  DapperTemplate.InsertAsync(Connection,obj).Result;
            return new Task<int>(() => res.HasValue ? res.Value : 0);
        }

        /// <summary>修改实体类信息</summary>
        /// <param name="obj">实体类</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回修改结果,大于0 返回修改成功,小于等于0 返回修改失败 </return>
        public virtual Task<int> UpdateAsync(T obj, CancellationToken cancellationToken = default)
        {
            var res = DapperTemplate.UpdateAsync(Connection,obj);
            return res;
        }

        /// <summary>根据id删除实体类信息</summary>
        /// <param name="id">id</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回删除结果,大于0 返回删除成功,小于等于0 返回删除失败 </return>
        public virtual Task<int> RemoveAsync(object id,CancellationToken cancellationToken = default)
        {
            var res = DapperTemplate.DeleteAsync<T>(Connection, id);
            return res;
        }

        /// <summary>根据条件查询实体类数据集数量信息</summary>
        /// <param name="obj">实体类</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回实体类数据集数量信息</return>
        public virtual Task<int> CountAsync(T obj, CancellationToken cancellationToken = default)
        {
            var res = DapperTemplate.CountAsync<T>(Connection,Where(obj), obj);
            return res;
        }

        /// <summary>根据条件查询实体类数据集信息</summary>
        /// <param name="obj">实体类</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回实体类数据集信息</return>
        public virtual async Task<List<T>> QueryAsync(T obj, CancellationToken cancellationToken = default)
        {
            var res = await DapperTemplate.FindListAsync<T>(Connection,Where(obj), obj);
            var data = res.ToList();
            return data;
        }

        /// <summary>根据条件及分页查询实体类数据集信息</summary>
        /// <param name="obj">实体类</param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回实体类数据集信息</return>
        public virtual async Task<List<T>> QueryByPageAsync(T obj, int page, int size, CancellationToken cancellationToken = default)
        {
            var res = await DapperTemplate.FindListByPageAsync<T>(Connection, Where(obj),/*"  ORDER BY  ID ASC "*/"", obj, page, size);
            var data = res.ToList();
            return data;
        }

        /// <summary>根据条件及分页查询实体类数据集信息和实体类数据集数量信息</summary>
        /// <param name="obj">实体类</param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回实体类数据集信息和实体类数据集数量信息</return>
        public virtual async Task<ResultModel<T>> QueryResultModelByPageAsync(T obj, int page, int size,CancellationToken cancellationToken = default)
        {
            var where = Where(obj);
            PageHelper.Set(ref page, ref size);
            var res = await DapperTemplate.FindTupleByPageAsync<T>(Connection, obj, where,  page, size);
            ResultModel<T> result = new ResultModel<T>(res,page,size);
            return result;
        }
#endif
#endregion async
    }

    /// <summary>
    /// 
    /// </summary>
    public  class DapperDALHelper
    {
        /// <summary>
        /// 
        /// </summary>
        public static IDbConnection Connection { get; set; }

    }
}
#endif