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

namespace Utility.EnterpriseLibrary
{
    /// <summary>
    /// 
    /// </summary>
    public enum DbFlag
    {
        /// <summary>
        /// None
        /// </summary>
        None,
        /// <summary>
        /// Mysql
        /// </summary>
        Mysql
    }
    /// <summary>实体  企业库 数据访问层接口 实现  </summary>
    public class EnterpriseLibraryDbHelper
    {
        /// <summary>
        /// 
        /// </summary>
        public static Microsoft.Practices.EnterpriseLibrary.Data.Database Database { get; set; }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public static int Operator(Action<DbCommand> action)
        {
            DbConnection connection = null;
            DbTransaction transaction = null;
            DbCommand command = null;
            try
            {
                connection = Database.CreateConnection();
                transaction = connection.BeginTransaction();
                command = connection.CreateCommand();
                action(command);
                int res = Database.ExecuteNonQuery(command);
                transaction.Commit();
                return res;
            }
            catch
            {
                transaction?.Rollback();
                throw;
            }
            finally
            {
                transaction?.Dispose();
                command?.Dispose();
                connection?.Dispose();
            }
        }


        /// <summary>根据id删除实体类信息</summary>
        /// <param name="id">id</param>
        /// <param name="table"></param>
        ///<return>返回删除结果,大于0 返回删除成功,小于等于0 返回删除失败 </return>
        public static int Delete<Key>(string table, Key id)
        {
            Action<DbCommand> action = (it) =>
            {
                it.CommandText = $"DELETE FROM {table} WHERE Id=@Id;";
                var parameter = it.CreateParameter();
                it.Parameters.Add(parameter);
                parameter.ParameterName = "@Id";
                parameter.Value = id;
            };
            return Operator(action);
        }



        /// <summary>根据id删除实体类信息(多删除)</summary>
        /// <param name="ids">id</param>
        /// <param name="table"></param>
        ///<return>返回删除结果(多删除),大于0 返回删除成功(多删除),小于等于0 返回删除失败(多删除) </return>
        public static int DeleteList<Key>(string table, Key[] ids)
        {
            Action<DbCommand> action = (it) =>
            {
                it.CommandText =DALHelper.DeleteListSql(ids, table);
            };
            return Operator(action);
        }



        /// <summary>根据条件查询实体类数据集信息</summary>
        /// <param name="obj">实体类</param>
        /// <param name="whereCondition"></param>
        /// <param name="func"></param>
        /// <param name="sql"></param>
        ///<return>返回实体类数据集信息 </return>
        public static List<T> FindList<T>(T obj, string sql, Action<T, DbCommand> whereCondition,
            Func<IDataReader, List<T>> func) where T : class
        {
            DbConnection connection = null;
            DbCommand command = null;
            try
            {
                connection = Database.CreateConnection();
                command = connection.CreateCommand();
                command.CommandText = sql;
                return FindList(obj, command, whereCondition, func);
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
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="command"></param>
        /// <param name="whereCondition"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static List<T> FindList<T>(T obj, DbCommand command, Action<T, DbCommand> whereCondition,
         Func<IDataReader, List<T>> func) where T : class
        {
            whereCondition(obj, command);
            var reader = Database.ExecuteReader(command);
            return func(reader);
        }
        /// <summary>根据条件查询实体类数据集数量信息</summary>
        /// <param name="obj">实体类</param>
        /// <param name="whereCondition"></param>
        ///<return>返回实体类数据集数量信息</return>
        public static int Count<T>(T obj, Action<T, DbCommand> whereCondition) where T : class
        {
            DbConnection connection = null;
            DbCommand command = null;
            try
            {
                connection = Database.CreateConnection();
                command = connection.CreateCommand();
                return Count(obj, command, whereCondition);
            }
            finally
            {
                command.Dispose();
                connection.Dispose();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="command"></param>
        /// <param name="whereCondition"></param>
        /// <returns></returns>
        public static int Count<T>(T obj, DbCommand command, Action<T, DbCommand> whereCondition) where T : class
        {
            whereCondition(obj, command);
            string where = command.CommandText;
            command.CommandText = where;
            int num = Count(command);
            return num;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="table"></param>
        /// <param name="where"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        public static int Count(string table, string where, DbCommand command)
        {
            string sql = $"SELECT COUNT(*) FROM {table} {where};";
            command.CommandText = sql;
            return Count(command);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static int Count(DbCommand command)
        {
            var res = Database.ExecuteScalar(command);
            int num = (int)res;
            return num;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="command"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static List<T> FindListByPage<T>(DbCommand command, Func<IDataReader, List<T>> func) where T : class
        {
            var reader = Database.ExecuteReader(command);
            try
            {
                return func(reader);
            }
            finally
            {
                reader.Dispose();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="selectColumn"></param>
        /// <param name="table"></param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public static string QueryMySqlByPage<T>(string selectColumn, string table, int page, int size, string where) where T : class
        {
            return $"SELECT {selectColumn} FROM {table} {where} LIMIT {page - 1},{size};";
        }


        /// <summary>根据条件及分页查询实体类数据集信息和实体类数据集数量信息</summary>
        /// <param name="obj">实体类</param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        /// <param name="func"></param>
        /// <param name="whereCondition"></param>
        ///<return>返回实体类数据集信息和实体类数据集数量信息</return>
        public static ResultModel<T> FindResultModelByPage<T>(T obj, int page, int size, Action<T, DbCommand> whereCondition,
            Func<IDataReader, System.Collections.Generic.List<T>> func) where T : class
        {
            DbConnection connection = null;
            DbCommand command = null;
            try
            {
                connection = Database.CreateConnection();
                command = connection.CreateCommand();
                whereCondition(obj, command);
                string where = command.CommandText;
                //mysql
                List<T> datas = FindListByPage(command, func);
                int count = Count(obj, whereCondition);
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