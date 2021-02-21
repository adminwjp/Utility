#if NET40 || NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48
using System;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using Utility.Model;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Utility.DAL;
using System.Collections.Generic;

namespace Utility.EnterpriseLibrary.DAL
{
    /// <summary>实体  企业库 数据访问层接口 实现  </summary>
    public class BaseEnterpriseLibraryDAL<T, Key> : IDAL<T, Key> where T : class
    {
        /// <summary>
        /// 
        /// </summary>
        protected string ConnectionStringName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        protected Microsoft.Practices.EnterpriseLibrary.Data.Database Database { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public BaseEnterpriseLibraryDAL()
        {
            Database = DatabaseFactory.CreateDatabase(ConnectionStringName);
        }


        /// <summary>添加实体类信息</summary>
        /// <param name="obj">实体类</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回添加结果,大于0 返回添加成功,小于等于0 返回添加失败 </return>
        public Task<int> InsertAsync(T obj, CancellationToken cancellationToken = default)
        {
            return new Task<int>(()=>Insert(obj));
        }

        /// <summary>添加实体类信息</summary>
        /// <param name="obj">实体类</param>
        ///<return>返回添加结果,大于0 返回添加成功,小于等于0 返回添加失败 </return>
        public int Insert(T obj)
        {
            return AddOrUpdateOperator(obj);
        }

        private int AddOrUpdateOperator(T obj)
        {
            Action<DbCommand> action = (command) => { AddOrUpdate(command, obj); };
            return EnterpriseLibraryDbHelper.Operator(action);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <param name="obj"></param>
        protected virtual void AddOrUpdate(DbCommand command, T obj)
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
        /// <summary>修改实体类信息</summary>
        /// <param name="obj">实体类</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回修改结果,大于0 返回修改成功,小于等于0 返回修改失败 </return>
        public System.Threading.Tasks.Task<int> UpdateAsync(T obj, CancellationToken cancellationToken = default)
        {
            return new Task<int>(()=>Update(obj));
        }

        /// <summary>修改实体类信息</summary>
        /// <param name="obj">实体类</param>
        ///<return>返回修改结果,大于0 返回修改成功,小于等于0 返回修改失败 </return>
        public int Update(T obj)
        {
            return AddOrUpdateOperator(obj);
        }

        /// <summary>根据id删除实体类信息</summary>
        /// <param name="id">id</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回删除结果,大于0 返回删除成功,小于等于0 返回删除失败 </return>
        public Task<int> DeleteAsync(Key id, CancellationToken cancellationToken = default)
        {
            return new Task<int>(()=>Delete(id));
        }

        /// <summary>根据id删除实体类信息</summary>
        /// <param name="id">id</param>
        ///<return>返回删除结果,大于0 返回删除成功,小于等于0 返回删除失败 </return>
        public int Delete(Key id)
        {
            return EnterpriseLibraryDbHelper.Delete(GetTable(), id);
        }

        /// <summary>根据id删除实体类信息(多删除)</summary>
        /// <param name="ids">id</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回删除结果(多删除),大于0 返回删除成功(多删除),小于等于0 返回删除失败(多删除) </return>
        public Task<int> DeleteListAsync(Key[] ids, CancellationToken cancellationToken = default)
        {
            return new Task<int>(()=>DeleteList(ids));
        }

        /// <summary>根据id删除实体类信息(多删除)</summary>
        /// <param name="ids">id</param>
        ///<return>返回删除结果(多删除),大于0 返回删除成功(多删除),小于等于0 返回删除失败(多删除) </return>
        public int DeleteList(Key[] ids)
        {
            return EnterpriseLibraryDbHelper.DeleteList(GetTable(), ids);
        }

        /// <summary>根据条件查询实体类数据集信息</summary>
        /// <param name="obj">实体类</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回实体类数据集信息 </return>
        public Task<List<T>> FindListAsync(T obj, CancellationToken cancellationToken = default)
        {
            return new Task<System.Collections.Generic.List<T>>(()=>FindList(obj));
        }

        /// <summary>根据条件查询实体类数据集信息</summary>
        /// <param name="obj">实体类</param>
        ///<return>返回实体类数据集信息 </return>
        public List<T> FindList(T obj)
        {
            DbConnection connection = null;
            DbCommand command = null;
            try
            {
                connection = Database.CreateConnection();
                command = connection.CreateCommand();
                command.CommandText = $"SELECT {QuerySelectColumn()} FROM {GetTable()}";
                QueryWhere(obj, command);
                var reader = Database.ExecuteReader(command);
                try
                {
                    return Reader(reader);
                }
                finally
                {
                    reader.Dispose();
                }
            }
            finally
            {
                command?.Dispose();
                connection?.Dispose();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        protected virtual List<T> Reader(IDataReader reader)
        {
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="command"></param>
        protected virtual void QueryWhere(T obj, DbCommand command)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual string QuerySelectColumn()
        {
            return string.Empty;
        }
        /// <summary>根据条件查询实体类数据集数量信息</summary>
        /// <param name="obj">实体类</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回实体类数据集数量信息</return>
        public Task<int> CountAsync(T obj,CancellationToken cancellationToken = default)
        {
            return new Task<int>(()=>Count(obj));
        }
        /// <summary>根据条件查询实体类数据集数量信息</summary>
        /// <param name="obj">实体类</param>
        ///<return>返回实体类数据集数量信息</return>
        public int Count(T obj)
        {
            DbConnection connection = null;
            DbCommand command = null;
            try
            {
                connection = Database.CreateConnection();
                command = connection.CreateCommand();
                QueryWhere(obj, command);
                string where = command.CommandText;
                int num = Count(where, command);
                return num;
            }
            finally
            {
                command.Dispose();
                connection.Dispose();
            }
        }

        private int Count(string where, DbCommand command)
        {
            return EnterpriseLibraryDbHelper.Count(GetTable(), where, command);
        }

        /// <summary>根据条件及分页查询实体类数据集信息</summary>
        /// <param name="obj">实体类</param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回实体类数据集信息</return>
        public Task<List<T>> FindListByPageAsync(T obj, int page, int size,CancellationToken cancellationToken = default)
        {
            return new Task<List<T>>(()=>FindListByPage(obj, page, size));
        }
        /// <summary>根据条件及分页查询实体类数据集信息</summary>
        /// <param name="obj">实体类</param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        ///<return>返回实体类数据集信息</return>
        public List<T> FindListByPage(T obj, int page, int size)
        {
            DbConnection connection = null;
            DbCommand command = null;
            try
            {
                connection = Database.CreateConnection();
                command = connection.CreateCommand();
                //mysql
                QueryWhere(obj, command);
                string where = command.CommandText;
                return FindListByPage(page, size, where, command);
            }
            finally
            {
                command?.Dispose();
                connection?.Dispose();
            }
        }

        private List<T> FindListByPage(int page, int size, string where, DbCommand command)
        {
            //mysql
            command.CommandText = $"SELECT {QuerySelectColumn()} FROM {GetTable()} {where} LIMIT {page - 1},{size};";
            var reader = Database.ExecuteReader(command);
            try
            {
                return Reader(reader);
            }
            finally
            {
                reader.Dispose();
            }
        }

        /// <summary>根据条件及分页查询实体类数据集信息和实体类数据集数量信息</summary>
        /// <param name="obj">实体类</param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回实体类数据集信息和实体类数据集数量信息</return>
        public Task<ResultModel<T>> FindResultModelByPageAsync(T obj, int page, int size,CancellationToken cancellationToken = default)
        {
            return new Task<ResultModel<T>>(()=>FindResultModelByPage(obj, page, size));
        }
        /// <summary>根据条件及分页查询实体类数据集信息和实体类数据集数量信息</summary>
        /// <param name="obj">实体类</param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        ///<return>返回实体类数据集信息和实体类数据集数量信息</return>
        public virtual ResultModel<T> FindResultModelByPage(T obj, int page, int size)
        {
            DbConnection connection = null;
            DbCommand command = null;
            try
            {
                connection = Database.CreateConnection();
                command = connection.CreateCommand();
                QueryWhere(obj, command);
                string where = command.CommandText;
                //mysql
                List<T> datas = FindListByPage(page, size, where, command);
                int count = Count(where, command);
                ResultModel<T> result = new ResultModel<T>();
                result.Data = datas.Any() ? datas : null;
                result.Result = new PageModel() { Records = count, Total = count == 0 ? 0 : count % size == 0 ? count / size : (count / size + 1), Size = size, Page = page };
                return result;
            }
            finally
            {
                command?.Dispose();
                connection?.Dispose();
            }
        }

    }
}
#endif