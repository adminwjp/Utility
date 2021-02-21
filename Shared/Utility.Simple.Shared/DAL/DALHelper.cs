#if !(NET20 || NET30 )
using System;
using System.Linq;
#endif
using System.Text;
using System.Threading;
#if !(NET20 || NET30 || NET35)
using System.Threading.Tasks;
#endif
using Utility.Cache;
using Utility.Domain.Uow;
using Utility.Model;

namespace Utility.DAL
{
    /// <summary>
    /// 数据访问层接口 静态 方法  通用部分
    /// </summary>
    public class DALHelper<Model, Key>: DALHelper<Model> where Model : class, IModel<Key>
    {     


        /// <summary>根据id删除实体类信息(多删除)</summary>
        /// <param name="ids">id</param>
        ///<return>返回删除结果(多删除),大于0 返回删除成功(多删除),小于等于0 返回删除失败(多删除) </return>
        public static int DeleteList(Key[] ids)
        {
            return DALHelper.DeleteList<Model,Key>(UnitWork, ids, TableName,Id);
        }

        /// <summary>根据id删除实体类信息(多删除)</summary>
        /// <param name="ids">id</param>
        /// <param name="unitWork"></param>
        ///<return>返回删除结果(多删除),大于0 返回删除成功(多删除),小于等于0 返回删除失败(多删除) </return>
        public static int DeleteList(IUnitWork unitWork, Key[] ids)
        {
            return DALHelper.DeleteList<Model, Key>(unitWork, ids, TableName, Id);
        }

        /// <summary>根据id删除实体类信息</summary>
        /// <param name="id">id</param>
        ///<return>返回删除结果,大于0 返回删除成功,小于等于0 返回删除失败 </return>
        public static int Delete(Key id)
        {
            return DALHelper.Delete<Model,Key>(UnitWork, id);
        }

        /// <summary>根据id删除实体类信息</summary>
        /// <param name="id">id</param>
        /// <param name="unitWork"></param>
        ///<return>返回删除结果,大于0 返回删除成功,小于等于0 返回删除失败 </return>
        public static int Delete(IUnitWork unitWork, Key id)
        {
            return DALHelper.Delete<Model, Key>(unitWork, id);
        }

#if !(NET20 || NET30 || NET35)
        /// <summary>根据id删除实体类信息</summary>
        /// <param name="id">id</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回删除结果,大于0 返回删除成功,小于等于0 返回删除失败 </return>
        public static  Task<int> DeleteAsync(Key id, CancellationToken cancellationToken = default)
        {
            return DALHelper.DeleteAsync<Model,Key>(UnitWork, id, cancellationToken);
        }

        /// <summary>根据id删除实体类信息</summary>
        /// <param name="id">id</param>
        /// <param name="unitWork"></param>
        /// <param name="cancellationToken"></param>
        ///<return>返回删除结果,大于0 返回删除成功,小于等于0 返回删除失败 </return>
        public static  Task<int> DeleteAsync(IUnitWork unitWork, Key id, CancellationToken cancellationToken = default)
        {
            return DALHelper.DeleteAsync<Model, Key>(unitWork, id, cancellationToken);
        }

        /// <summary>根据id删除实体类信息(多删除)</summary>
        /// <param name="ids">id</param>
        /// <param name="idColumn"></param>
        /// <param name="table"></param>
        /// <param name="cancellationToken"></param>
        ///<return>返回删除结果(多删除),大于0 返回删除成功(多删除),小于等于0 返回删除失败(多删除) </return>
        public static  Task<int> DeleteListAsyn(Key[] ids, string table, string idColumn = DALHelper.IdColumn, CancellationToken cancellationToken = default)
        {
            return DALHelper.DeleteListAsync<Model,Key>(UnitWork, ids, table, idColumn, cancellationToken);
        }

        /// <summary>根据id删除实体类信息(多删除)</summary>
        /// <param name="ids">id</param>
        /// <param name="unitWork"></param>
        /// <param name="idColumn"></param>
        /// <param name="table"></param>
        /// <param name="cancellationToken"></param>
        ///<return>返回删除结果(多删除),大于0 返回删除成功(多删除),小于等于0 返回删除失败(多删除) </return>
        public static  Task<int> DeleteListAsync(IUnitWork unitWork, Key[] ids, string table, string idColumn = DALHelper.IdColumn, CancellationToken cancellationToken = default)
        {
            return DALHelper.DeleteListAsync<Model, Key>(unitWork, ids, table, idColumn, cancellationToken);
        }
#endif
    }

    /// <summary>
    /// 数据访问层接口 静态 方法 通用部分
    /// </summary>
    public class DALHelper<Model> where Model :class
    {
        /// <summary>
        /// 缓存
        /// </summary>

        public static ICacheContent Cache { get; set; }
        /// <summary>
        /// 表名
        /// </summary>
        public static string TableName { get; set; }
        /// <summary>
        /// 主键 名称 
        /// </summary>
        public static string Id { get; set; } = DALHelper.IdColumn;
        /// <summary>
        /// 工作 单元 
        /// </summary>
        public static IUnitWork UnitWork { get; set; }
        /// <summary>添加实体类信息信息</summary>
        /// <param name="obj">实体类信息</param>
        ///<return>返回添加实体类信息结果,大于0 返回添加成功,小于等于0 返回添加用失败 </return>
        public static int Insert(Model obj)
        {
            return DALHelper.Insert(UnitWork, obj);
        }

        /// <summary>添加实体类信息信息</summary>
        /// <param name="unitWork"></param>
        /// <param name="obj">实体类信息</param>
        ///<return>返回添加实体类信息结果,大于0 返回添加成功,小于等于0 返回添加用失败 </return>
        public static int Insert(IUnitWork unitWork, Model obj)
        {
            return DALHelper.Insert<Model>(unitWork, obj);
        }

        /// <summary>修改实体类信息</summary>
        /// <param name="obj">实体类</param>
        ///<return>返回修改结果,大于0 返回修改成功,小于等于0 返回修改失败 </return>
        public static int Update(Model obj)
        {
            return DALHelper.Update(UnitWork, obj);
        }


        /// <summary>修改实体类信息</summary>
        /// <param name="unitWork"></param>
        /// <param name="obj">实体类</param>
        ///<return>返回修改结果,大于0 返回修改成功,小于等于0 返回修改失败 </return>
        public static int Update(IUnitWork unitWork, Model obj)
        {
            return DALHelper.Update<Model>(unitWork, obj);
        }

        /// <summary>根据id删除实体类信息</summary>
        /// <param name="id">id</param>
        ///<return>返回删除结果,大于0 返回删除成功,小于等于0 返回删除失败 </return>
        public static int Delete(object id)
        {
            return DALHelper.Delete<Model>(UnitWork, id);
        }


        /// <summary>根据id删除实体类信息</summary>
        /// <param name="id">id</param>
        /// <param name="unitWork"></param>
        ///<return>返回删除结果,大于0 返回删除成功,小于等于0 返回删除失败 </return>
        public static int Delete(IUnitWork unitWork, object id)
        {
            return DALHelper.Delete<Model>(unitWork, id);
        }

        #region async
#if !(NET20 || NET30 || NET35)
        /// <summary>添加实体类信息</summary>
        /// <param name="obj">实体类</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回添加结果,大于0 返回添加成功,小于等于0 返回添加失败 </return>
        public static  Task<int> InsertAsync(Model obj, CancellationToken cancellationToken = default)
        {
            return DALHelper.InsertAsync(UnitWork, obj, cancellationToken);
        }


        /// <summary>添加实体类信息</summary>
        /// <param name="obj">实体类</param>
        /// <param name="unitWork"></param>
        /// <param name="cancellationToken"></param>
        ///<return>返回添加结果,大于0 返回添加成功,小于等于0 返回添加失败 </return>
        public static  Task<int> InsertAsync(IUnitWork unitWork, Model obj, CancellationToken cancellationToken = default)
        {
            return DALHelper.InsertAsync<Model>(unitWork, obj, cancellationToken);
        }

        /// <summary>修改实体类信息</summary>
        /// <param name="obj">实体类</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回修改结果,大于0 返回修改成功,小于等于0 返回修改失败 </return>
        public static  Task<int> UpdateAsync(Model obj, CancellationToken cancellationToken = default)
        {
            return DALHelper.UpdateAsync(UnitWork, obj, cancellationToken);
        }

        /// <summary>修改实体类信息</summary>
        /// <param name="obj">实体类</param>
        /// <param name="unitWork"></param>
        /// <param name="cancellationToken"></param>
        ///<return>返回修改结果,大于0 返回修改成功,小于等于0 返回修改失败 </return>
        public static  Task<int> UpdateAsync(IUnitWork unitWork, Model obj, CancellationToken cancellationToken = default)
        {
            return DALHelper.UpdateAsync<Model>(unitWork, obj, cancellationToken);
        }

        /// <summary>根据id删除实体类信息</summary>
        /// <param name="id">id</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回删除结果,大于0 返回删除成功,小于等于0 返回删除失败 </return>
        public static  Task<int> DeleteAsync(object id, CancellationToken cancellationToken = default)
        {
            return DALHelper.DeleteAsync<Model>(UnitWork, id, cancellationToken);
        }

        /// <summary>根据id删除实体类信息</summary>
        /// <param name="id">id</param>
        /// <param name="unitWork"></param>
        /// <param name="cancellationToken"></param>
        ///<return>返回删除结果,大于0 返回删除成功,小于等于0 返回删除失败 </return>
        public static  Task<int> DeleteAsync(IUnitWork unitWork, object id, CancellationToken cancellationToken = default)
        {
            return DALHelper.DeleteAsync<Model>(unitWork, id, cancellationToken);
        }

#endif
#endregion async
    }

    /// <summary>
    /// 数据访问层接口 静态 方法 通用部分 
    /// </summary>
    public class DALHelper
    {
        /// <summary>
        /// 缓存
        /// </summary>

        public static ICacheContent Cache { get; set; }
        /// <summary>
        /// 默认 主键 列 为 Id
        /// </summary>
        public const string IdColumn = "Id";
    
        /// <summary>
        /// 
        /// </summary>
        public static IUnitWork UnitWork { get; set; }

        /// <summary>添加实体类信息信息</summary>
        /// <param name="model">实体类信息</param>
        ///<return>返回添加实体类信息结果,大于0 返回添加成功,小于等于0 返回添加用失败 </return>
        public static int Insert<Model, Key>(Model model) where Model : class, IModel<Key>
        {
            return Insert<Model, Key>(UnitWork, model);
        }

        /// <summary>添加实体类信息信息</summary>
        /// <param name="unitWork"></param>
        /// <param name="model">实体类信息</param>
        ///<return>返回添加实体类信息结果,大于0 返回添加成功,小于等于0 返回添加用失败 </return>

        public static int Insert<Model, Key>(IUnitWork unitWork, Model model) where Model : class, IModel<Key>
        {
            unitWork.Insert(model);
            return 1;
        }

        /// <summary>根据id删除实体类信息(多删除)</summary>
        /// <param name="ids">id</param>
        /// <param name="tableName">表</param>
        /// <param name="id">id name</param>
        ///<return>返回删除结果(多删除),大于0 返回删除成功(多删除),小于等于0 返回删除失败(多删除) </return>
        public static int DeleteList<Model, Key>(Key[] ids, string tableName, string id) where Model : class, IModel<Key>
        {
            return DeleteList<Model, Key>(UnitWork, ids,tableName,id);
        }


        /// <summary>根据id删除实体类信息(多删除)</summary>
        /// <param name="ids">id</param>
        /// <param name="unitWork"></param>
        /// <param name="tableName">表</param>
        /// <param name="id">id name</param>
        ///<return>返回删除结果(多删除),大于0 返回删除成功(多删除),小于等于0 返回删除失败(多删除) </return>
        public static int DeleteList<Model, Key>(IUnitWork unitWork, Key[] ids,string tableName,string id) where Model : class, IModel<Key>
        {
            string sql = DeleteListSql(ids, tableName, id);
            return unitWork.ExecuteSql(sql);
        }


        /// <summary>根据id删除实体类信息(多删除)</summary>
        /// <param name="ids">id</param>
        /// <param name="table"></param>
        /// <param name="idColumn"></param>
        ///<return>返回删除结果(多删除),大于0 返回删除成功(多删除),小于等于0 返回删除失败(多删除) </return>
        public static string DeleteListSql<Key>(Key[] ids, string table, string idColumn = IdColumn)
        {
            string where = IdSql(ids);
            string sql = $"DELETE FROM {table} WHERE  {idColumn} EXISTS({where})";
            return sql;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="Key"></typeparam>
        /// <param name="ids"></param>
        /// <returns></returns>
        public static string IdSql<Key>(Key[] ids)
        {
#if (NETCOREAPP1_0 || NETCOREAPP1_1 ||NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
            throw new NotImplementedException();
#else
            string where = string.Empty;
            if (typeof(Key).IsValueType)
            {
                StringBuilder id = new StringBuilder();
                foreach (var item in ids)
                {
                    id.Append(item);
                }
                where = id.ToString();
            }
            else
            {
                StringBuilder id = new StringBuilder();
                foreach (var item in ids)
                {
                    id.Append("'").Append(item).Append("'");
                }
                where = id.ToString();
            }
            return where;
#endif
        }


        /// <summary>添加实体类信息</summary>
        /// <param name="model">实体类</param>
        ///<return>返回添加结果,大于0 返回添加成功,小于等于0 返回添加失败 </return>
        public static int Insert<Model>(Model model) where Model : class
        {
            return Insert(UnitWork, model);
        }

        /// <summary>添加实体类信息</summary>
        /// <param name="model">实体类</param>
        /// <param name="unitWork"></param>
        ///<return>返回添加结果,大于0 返回添加成功,小于等于0 返回添加失败 </return>
        public static int Insert<Model>(IUnitWork unitWork, Model model) where Model : class
        {
            unitWork.Insert(model);
            return 1;
        }

        /// <summary>修改实体类信息</summary>
        /// <param name="obj">实体类</param>
        ///<return>返回修改结果,大于0 返回修改成功,小于等于0 返回修改失败 </return>
        public static int Update<Model>(Model obj) where Model : class
        {
            return Update(UnitWork, obj);
        }

        /// <summary>修改实体类信息</summary>
        /// <param name="obj">实体类</param>
        /// <param name="unitWork"></param>
        ///<return>返回修改结果,大于0 返回修改成功,小于等于0 返回修改失败 </return>
        public static int Update<Model>(IUnitWork unitWork, Model obj) where Model : class
        {
            unitWork.Update<Model>(obj);
            return 1;
        }

        /// <summary>根据id删除实体类信息</summary>
        /// <param name="id">id</param>
        ///<return>返回删除结果,大于0 返回删除成功,小于等于0 返回删除失败 </return>
        public static int Delete<Model, Key>(Key id) where Model : class
        {
            return Delete<Model, Key>(UnitWork, id);
        }

        /// <summary>根据id删除实体类信息</summary>
        /// <param name="id">id</param>
        /// <param name="unitWork"></param>
        ///<return>返回删除结果,大于0 返回删除成功,小于等于0 返回删除失败 </return>
        public static int Delete<Model, Key>(IUnitWork unitWork, Key id) where Model : class
        {
            unitWork.Delete<Model>(id);
            return 1;
        }

        /// <summary>根据id删除实体类信息</summary>
        /// <param name="id">id</param>
        ///<return>返回删除结果,大于0 返回删除成功,小于等于0 返回删除失败 </return>
        public static int Delete<Model>(object id) where Model : class, new()
        {
            return Delete<Model>(UnitWork, id);
        }

        /// <summary>根据id删除实体类信息</summary>
        /// <param name="id">id</param>
        /// <param name="unitWork"></param>
        ///<return>返回删除结果,大于0 返回删除成功,小于等于0 返回删除失败 </return>
        public static int Delete<Model>(IUnitWork unitWork, object id) where Model : class
        {
            unitWork.Delete<Model>(id);
            return 1;
        }



        #region async

#if !(NET20 || NET30 || NET35)
        /// <summary>根据id删除实体类信息</summary>
        /// <param name="id">id</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回删除结果,大于0 返回删除成功,小于等于0 返回删除失败 </return>
        public static  Task<int> DeleteAsync<Model, Key>(Key id, CancellationToken cancellationToken = default) 
            where Model : class, IModel<Key>
        {
            return  DeleteAsync<Model, Key>(UnitWork, id, cancellationToken);
        }

        /// <summary>根据id删除实体类信息</summary>
        /// <param name="id">id</param>
        /// <param name="unitWork"></param>
        /// <param name="cancellationToken"></param>
        ///<return>返回删除结果,大于0 返回删除成功,小于等于0 返回删除失败 </return>
        public static  Task<int> DeleteAsync<Model, Key>(IUnitWork unitWork, Key id, CancellationToken cancellationToken = default) 
            where Model :class, IModel<Key>
        {
             unitWork.DeleteAsync<Model>(id, cancellationToken);
#if !NET40
            return Task.FromResult(1);
#else
            return new Task<int>(()=>1);
#endif
        }

        /// <summary>
        /// 根据id删除实体类信息(多删除)
        /// </summary>
        /// <typeparam name="Model"></typeparam>
        /// <typeparam name="Key"></typeparam>
        /// <param name="ids">id</param>
        /// <param name="table"></param>
        /// <param name="idColumn"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>返回删除结果(多删除),大于0 返回删除成功(多删除),小于等于0 返回删除失败(多删除)</returns>
        public static  Task<int> DeleteListAsync<Model, Key>(Key[] ids, string table, string idColumn = IdColumn, CancellationToken cancellationToken = default)
        {
            return  DeleteListAsync<Model, Key>(UnitWork, ids, table, idColumn);
        }

        /// <summary>
        /// 根据id删除实体类信息(多删除)
        /// </summary>
        /// <typeparam name="Model"></typeparam>
        /// <typeparam name="Key"></typeparam>
        /// <param name="unitWork"></param>
        /// <param name="ids">id</param>
        /// <param name="table"></param>
        /// <param name="idColumn"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>返回删除结果(多删除),大于0 返回删除成功(多删除),小于等于0 返回删除失败(多删除)</returns>
        public static  Task<int> DeleteListAsync<Model, Key>(IUnitWork unitWork, Key[] ids, string table, string idColumn = IdColumn, CancellationToken cancellationToken = default)
        {
            string sql = DeleteListSql(ids, table, idColumn);
            return  unitWork.ExecuteSqlAsync(sql);
        }

        /// <summary>添加实体类信息</summary>
        /// <param name="obj">实体类</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回添加结果,大于0 返回添加成功,小于等于0 返回添加失败 </return>
        public static  Task<int> InsertAsync<Model>(Model obj, CancellationToken cancellationToken = default) where Model : class
        {
            return  InsertAsync(UnitWork, obj, cancellationToken);
        }


        /// <summary>添加实体类信息</summary>
        /// <param name="obj">实体类</param>
        /// <param name="unitWork"></param>
        /// <param name="cancellationToken"></param>
        ///<return>返回添加结果,大于0 返回添加成功,小于等于0 返回添加失败 </return>
        public static  Task<int> InsertAsync<Model>(IUnitWork unitWork, Model obj, CancellationToken cancellationToken = default) where Model : class
        {
            unitWork.InsertAsync(obj, cancellationToken);
            return new Task<int>(()=>1);
        }

        /// <summary>修改实体类信息</summary>
        /// <param name="obj">实体类</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回修改结果,大于0 返回修改成功,小于等于0 返回修改失败 </return>
        public static  Task<int> UpdateAsync<Model>(Model obj, CancellationToken cancellationToken = default) where Model : class
        {
            return  UpdateAsync(UnitWork, obj, cancellationToken);
        }


        /// <summary>修改实体类信息</summary>
        /// <param name="obj">实体类</param>
        /// <param name="unitWork"></param>
        /// <param name="cancellationToken"></param>
        ///<return>返回修改结果,大于0 返回修改成功,小于等于0 返回修改失败 </return>
        public static  Task<int> UpdateAsync<Model>(IUnitWork unitWork, Model obj, CancellationToken cancellationToken = default) where Model : class
        {
            unitWork.UpdateAsync(obj, cancellationToken);
            return new Task<int>(() => 1);
        }

        /// <summary>根据id删除实体类信息</summary>
        /// <param name="id">id</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回删除结果,大于0 返回删除成功,小于等于0 返回删除失败 </return>
        public static  Task<int> DeleteAsync<Model>(object id, CancellationToken cancellationToken = default) where Model : class
        {
            DeleteAsync<Model>(UnitWork, id, cancellationToken);
            return new Task<int>(() => 1);
        }

        /// <summary>根据id删除实体类信息</summary>
        /// <param name="id">id</param>
        /// <param name="unitWork"></param>
        /// <param name="cancellationToken"></param>
        ///<return>返回删除结果,大于0 返回删除成功,小于等于0 返回删除失败 </return>
        public static  Task<int> DeleteAsync<Model>(IUnitWork unitWork, object id, CancellationToken cancellationToken = default) where Model : class
        {
            unitWork.DeleteAsync<Model>(id, cancellationToken);
            return new Task<int>(() => 1);
        }
#endif
        #endregion async
    }

    /// <summary>
    /// 数据访问层  静态 方法 基类 DALHelper 别名
    /// </summary>
    /// <typeparam name="Model"></typeparam>
    /// <typeparam name="Key"></typeparam>
    public class ManagerHelper<Model, Key> : DALHelper<Model, Key> where Model : class, IModel<Key>
    {
    }

    /// <summary>
    ///  数据访问层 静态 方法 基类 DALHelper 别名
    /// </summary>
    /// <typeparam name="Model"></typeparam>
    public class ManagerHelper<Model> : DALHelper<Model> where Model : class
    {
    }

    /// <summary>
    ///  数据访问层 静态 方法 基类 DALHelper 别名
    /// </summary>
    public class ManagerHelper : DALHelper
    {
    }

}
