#if !(NET10 || NET11 || NET20 || NET30 || NET35  || NETCOREAPP1_0 || NETCOREAPP1_1 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
//#if NET40 || NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48 || NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0 || NETCOREAPP3_1 || NET5_0 || NETSTANDARD2_0 || NETSTANDARD2_1
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace Utility.Dapper
{
    /// <summary> dapper 封装类 </summary>
    public class DapperTemplate : IDisposable
    {
        private readonly IDbConnection _connection;//数据库连接对象

        /// <summary> 构造注册数据库连接对象</summary>
        /// <param name="connection">数据库连接对象</param>
        public DapperTemplate(IDbConnection connection)
        {
            this._connection = connection;
        }

        /// <summary>数据库连接对象 </summary>
        public IDbConnection Connection { get { return _connection; } }

        /// <summary>
        /// 
        /// </summary>
        ~DapperTemplate()
        {
            Dispose();
        }


#region
        /// <summary>添加 </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">实体</param>
        /// <returns></returns>
        public virtual int? Insert<T>(T obj) where T : class => Insert(Connection, obj);

        /// <summary>增删改</summary>
        /// <param name="sql">sql</param>
        /// <param name="obj">参数 实体 new{}都支持 </param>
        /// <returns></returns>
        public virtual int Execute(string sql, object obj = null) => Execute(Connection, sql, obj);

        /// <summary>查询 </summary>
        /// <param name="sql"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual dynamic FindSingle(string sql, object obj = null) => FindSingle(Connection, sql, obj);

        /// <summary>查询 </summary>
        /// <param name="sql"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual IEnumerable<dynamic> FindList(string sql, object obj = null) => FindList(Connection, sql, obj);

        /// <summary>添加 </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="Key"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual Key Insert<T, Key>(T obj) where T : class => Insert<T, Key>(Connection, obj);

        /// <summary> 修改 </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">实体</param>
        /// <returns></returns>
        public virtual int Update<T>(T obj) where T : class => Update(Connection, obj);

        /// <summary> 删除 </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">实体</param>
        /// <returns></returns>
        public virtual int Delete<T>(T obj) where T : class => Delete(Connection, obj);


        /// <summary> 删除 </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id">实体 id</param>
        /// <returns></returns>
        public virtual int Delete<T>(object id) where T : class => Delete<T>(Connection, id);

        /// <summary> 删除 </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where">条件</param>
        /// <returns></returns>
        public virtual int DeleteList<T>(object where=null) where T : class => DeleteList<T>(Connection, where);

        /// <summary> 删除 </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where">条件</param>
        /// <param name="param"></param>
        /// <returns></returns>
        public virtual int DeleteList<T>(string where=null, object param = null) where T : class => DeleteList<T>(Connection, where, param);


        /// <summary> 查询 </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where">条件</param>
        /// <returns></returns>
        public virtual IEnumerable<T> FindList<T>(object where=null) where T : class => FindList<T>(Connection, where);


        /// <summary> 查询 </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public virtual IEnumerable<T> FindList<T>() where T : class => FindList<T>(Connection);

        /// <summary> 查询 </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where">条件</param>
        /// <param name="param"></param>
        /// <returns></returns>
        public virtual IEnumerable<T> FindList<T>(string where=null, object param = null) where T : class => FindList<T>(Connection, where, param);

        /// <summary> 查询 </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where">条件</param>
        /// <param name="parameters"></param>
        /// <param name="orderby"></param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public virtual IEnumerable<T> FindListByPage<T>(string where = null, string orderby = "", object parameters = null,int page = 1, int size =10) where T : class => FindListByPage<T>(Connection, where, orderby, parameters, page, size);


        /// <summary> 查询 </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id">id</param>
        /// <returns></returns>
        public virtual T FindSingle<T>(object id) where T : class => FindSingle<T>(Connection, id);


        /// <summary> 查询 </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="conditions">条件</param>
        /// <param name="param">参数</param>
        /// <returns></returns>
        public virtual int Count<T>(string conditions, object param) where T : class => Count<T>(Connection, conditions, param);

        /// <summary> 查询 </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where">条件</param>
        /// <returns></returns>
        public virtual int Count<T>(object where) where T : class => Count<T>(Connection, where);
#endregion

#region static method
        /// <summary>添加 </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection"></param>
        /// <param name="obj">实体</param>
        /// <returns></returns>
        public static int? Insert<T>(IDbConnection connection, T obj) where T : class => connection.Insert(obj);

        /// <summary>增删改</summary>
        /// <param name="sql">sql</param>
        /// <param name="connection"></param>
        /// <param name="obj">参数 实体 new{}都支持 </param>
        /// <returns></returns>
        public static int Execute(IDbConnection connection, string sql, object obj = null) => connection.Execute(sql, obj);

        /// <summary>查询 </summary>
        /// <param name="connection"></param>
        /// <param name="sql"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static dynamic FindSingle(IDbConnection connection, string sql, object obj = null) => connection.QueryFirst(sql, obj);

        /// <summary>查询 </summary>
        /// <param name="connection"></param>
        /// <param name="sql"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static IEnumerable<dynamic> FindList(IDbConnection connection, string sql, object obj = null) => connection.Query(sql, obj);


        /// <summary>添加 </summary>
        /// <param name="connection"></param>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="Key"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Key Insert<T, Key>(IDbConnection connection, T obj) where T : class =>
#if !(NET40 || NET45)
            connection.Insert<Key, T>(obj);
#else
            throw new NotSupportedException();
#endif


        /// <summary> 修改 </summary>
        /// <param name="connection"></param>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">实体</param>
        /// <returns></returns>
        public static int Update<T>(IDbConnection connection, T obj) where T : class => connection.Update(obj);

        /// <summary> 删除 </summary>
        /// <param name="connection"></param>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">实体</param>
        /// <returns></returns>
        public static int Delete<T>(IDbConnection connection, T obj) where T : class => connection.Delete(obj);

        /// <summary> 删除 </summary>
        /// <param name="connection"></param>
        /// <typeparam name="T"></typeparam>
        /// <param name="id">实体 id</param>
        /// <returns></returns>
        public static int Delete<T>(IDbConnection connection, object id) where T : class => connection.Delete<T>(id);

        /// <summary> 删除 </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection"></param>
        /// <param name="where">条件</param>
        /// <returns></returns>
        public static int DeleteList<T>(IDbConnection connection, object where=null) where T : class => connection.DeleteList<T>(where);

        /// <summary> 删除 </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection"></param>
        /// <param name="where">条件</param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static int DeleteList<T>(IDbConnection connection, string where=null, object param = null) where T : class => connection.DeleteList<T>(where, param);


        /// <summary>查询 </summary>
        /// <param name="connection"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public static IEnumerable<T> FindList<T>(IDbConnection connection, object where=null) where T : class => connection.GetList<T>(where);

        /// <summary>查询 </summary>
        /// <param name="connection"></param>
        public static IEnumerable<T> FindList<T>(IDbConnection connection) where T : class => connection.GetList<T>();

        /// <summary>查询 </summary>
        /// <param name="connection"></param>
        /// <param name="where"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static IEnumerable<T> FindList<T>(IDbConnection connection, string where=null, object param = null) where T : class => connection.GetList<T>(where, param);

        /// <summary>查询 </summary>
        /// <param name="connection"></param>
        /// <param name="where"></param>
        /// <param name="parameters"></param>
        /// <param name="orderby"></param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static IEnumerable<T> FindListByPage<T>(IDbConnection connection, string where = null, string orderby = "", object parameters = null, int page = 1, int size=10) where T : class => connection.GetListPaged<T>(page, size, where, orderby, parameters);

        /// <summary>
        /// 根据 id 查询对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static T FindSingle<T>(IDbConnection connection, object id) where T : class
        {
            return connection.Get<T>(id);
        }

       /// <summary>
       /// 根据条件查询数量
       /// </summary>
       /// <typeparam name="T"></typeparam>
       /// <param name="connection"></param>
       /// <param name="where"></param>
       /// <param name="param"></param>
       /// <returns></returns>
        public static int Count<T>(IDbConnection connection, string where=null, object param=null) where T : class => connection.RecordCount<T>(where, param);

        /// <summary>
        /// 根据条件查询数量
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public static int Count<T>(IDbConnection connection, object where=null) where T : class => connection.RecordCount<T>(where);
#endregion  static method

#region async

        /// <summary>添加 </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">实体</param>
        /// <returns></returns>
        public virtual Task<int?> InsertAsync<T>(T obj) where T : class
        {
            return InsertAsync(Connection, obj);
        }

       /// <summary>
       /// 增 删  改 操作
       /// </summary>
       /// <param name="sql"></param>
       /// <param name="obj"></param>
       /// <returns></returns>
        public virtual Task<int> ExecuteAsync(string sql, object obj = null)
        {
            return ExecuteAsync(Connection, sql, obj);
        }

        /// <summary>
        /// 根据条件查询一条数据
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual Task<dynamic> FindSingleAsync(string sql, object obj = null)
        {
            return FindSingleAsync(Connection, sql, obj);
        }
        /// <summary>
        /// 根据条件 查询 数据
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="obj"></param>
        /// <returns></returns>

        public virtual Task<IEnumerable<dynamic>> FindListAsync(string sql, object obj = null)
        {
            return FindListAsync(Connection, sql, obj);
        }
        /// <summary>添加 </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="Key"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual Task<Key> InsertAsync<T, Key>(T obj) where T : class
        {
            return InsertAsync<T, Key > (Connection, obj);
        }

        /// <summary> 修改 </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">实体</param>
        /// <returns></returns>
        public virtual Task<int> UpdateAsync<T>(T obj) where T : class
        {
            return UpdateAsync<T>(Connection, obj);
        }

        /// <summary> 删除 </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">实体</param>
        /// <returns></returns>
        public virtual Task<int> DeleteAsync<T>(T obj) where T : class
        {
            return DeleteAsync<T>(Connection, obj);
        }

        /// <summary>
        /// 根据 id 删除 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual Task<int> DeleteAsync<T>(object id) where T : class
        { 
            return DeleteAsync<T>(Connection, id);
        }

        /// <summary> 删除 </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where">条件</param>
        /// <returns></returns>
        public virtual Task<int> DeleteListAsync<T>(object where=null) where T : class
        {
            return DeleteListAsync<T>(Connection, where);
        }

        /// <summary> 删除 </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where">条件</param>
        /// <param name="param"></param>
        /// <returns></returns>
        public virtual Task<int> DeleteListAsync<T>(string where=null, object param = null) where T : class
        {
            return DeleteListAsync<T>(Connection, where, param);
        }

        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <returns></returns>
        public virtual Task<IEnumerable<T>> FindListAsync<T>(object where=null) where T : class
        {
            return FindListAsync<T>(Connection, where);
        }

        /// <summary>
        /// 查询所有
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public virtual Task<IEnumerable<T>> FindListAsync<T>() where T : class
        {
            return FindListAsync<T>(Connection);
        }

        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public virtual Task<IEnumerable<T>> FindListAsync<T>(string where=null, object param = null) where T : class
        {
            return FindListAsync<T>(Connection, where,param);
        }

        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <param name="orderby"></param>
        /// <param name="parameters"></param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public virtual Task<IEnumerable<T>> FindListByPageAsync<T>(string where = null, string orderby = "", object parameters = null,int page = 1, int size=10) where T : class
        {
            return FindListByPageAsync<T>(Connection, where, orderby, parameters, page,size);
        }

        /// <summary>
        /// 根据 id 查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual Task<T> FindSingleAsync<T>(object id) where T : class
        {
            return FindSingleAsync<T>(Connection,id);
        }

        /// <summary>
        /// 根据条件 查询 数量
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public virtual Task<int> CountAsync<T>(string where=null, object param=null) where T : class
        {
           return CountAsync<T>(Connection, where, param);
        }

        /// <summary>
        /// 根据条件 查询 数量
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <returns></returns>
        public virtual Task<int> CountAsync<T>(object where=null) where T : class
        {
            return CountAsync<T>(Connection, where);
        }

#endregion async

#region async static method



        /// <summary>添加 </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="Key"></typeparam>
        /// <param name="connection">连接对象</param>
        /// <param name="obj">实体</param>
        /// <returns></returns>
        public static Task<Key> InsertAsync<T, Key>(IDbConnection connection, T obj) where T : class
        {
#if NET40 || NET45
            return new Task<Key>(()=>Insert<T,Key>(connection,obj));
#else
            return connection.InsertAsync<Key, T>(obj);
#endif
        }

        /// <summary>添加 </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection">连接对象</param>
        /// <param name="obj">实体</param>
        /// <returns></returns>
        public static Task<int?> InsertAsync<T>(IDbConnection connection, T obj) where T : class
        {
#if NET40
            return new Task<int?>(()=>Insert(connection,obj));
#else
            return connection.InsertAsync(obj);
#endif
        }

        /// <summary>
        /// 增 删 改
        /// </summary>
        /// <param name="connection">连接对象</param>
        /// <param name="sql"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Task<int> ExecuteAsync(IDbConnection connection, string sql, object obj = null)
        {
#if NET40
            return new Task<int>(()=>Execute(connection,sql,obj));
#else
            return connection.ExecuteAsync(sql, obj);
#endif
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="connection">连接对象</param>
        /// <param name="sql"></param>
        /// <param name="obj">条件</param>
        /// <returns></returns>
        public static Task<dynamic> FindSingleAsync(IDbConnection connection, string sql, object obj = null)
        {
#if NET40
            return new Task<dynamic>(()=>FindSingle(connection,sql,obj));
#else
            return connection.QueryFirstAsync<dynamic>(sql, obj);
#endif
        }


        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="connection">连接对象</param>
        /// <param name="sql"></param>
        /// <param name="obj">条件</param>
        /// <returns></returns>
        public static Task<IEnumerable<dynamic>> FindListAsync(IDbConnection connection, string sql, object obj = null)
        {
#if NET40
            return new Task<IEnumerable<dynamic>>(()=>FindList(connection,sql,obj));
#else
            return connection.QueryAsync(sql, obj);
#endif
        }


        /// <summary>添加 </summary>
        /// <typeparam name="Key"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection">连接对象</param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Task<Key> InsertAsync<Key, T>(IDbConnection connection, T obj) where T : class
        {
#if NET40 || NET45
            throw new NotSupportedException();
#else
            return connection.InsertAsync<Key, T>(obj);
#endif
        }

        /// <summary> 修改 </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection">连接对象</param>
        /// <param name="obj">实体</param>
        /// <returns></returns>
        public static Task<int> UpdateAsync<T>(IDbConnection connection, T obj) where T : class
        {
#if NET40
            return new Task<int>(()=>Update(connection,obj));
#else
            return connection.UpdateAsync(obj);
#endif
        }

        /// <summary> 删除 </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection">连接对象</param>
        /// <param name="obj">实体</param>
        /// <returns></returns>
        public static Task<int> DeleteAsync<T>(IDbConnection connection, T obj) where T : class
        {
#if NET40
            return new Task<int>(()=>Delete(connection,obj));
#else
            return connection.DeleteAsync(obj);
#endif
        }

        /// <summary> 删除 </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection">连接对象</param>
        /// <param name="id">实体 id</param>
        /// <returns></returns>
        public static Task<int> DeleteAsync<T>(IDbConnection connection, object id) where T : class
        {
#if NET40
            return new Task<int>(()=>Delete<T>(connection,id));
#else
            return connection.DeleteAsync<T>(id);
#endif
        }

        /// <summary> 删除 </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection">连接对象</param>
        /// <param name="where">条件</param>
        /// <returns></returns>
        public static Task<int> DeleteListAsync<T>(IDbConnection connection, object where=null) where T : class
        {
#if NET40
            return new Task<int>(()=>DeleteList<T>(connection,where));
#else
            return connection.DeleteListAsync<T>(where);
#endif
        }

        /// <summary> 删除 </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection">连接对象</param>
        /// <param name="where">条件</param>
        /// <param name="param">参数</param>
        /// <returns></returns>
        public static Task<int> DeleteListAsync<T>(IDbConnection connection, string where=null, object param = null) where T : class
        {
#if NET40
            return new Task<int>(()=>DeleteList<T>(connection,where,param));
#else
            return connection.DeleteListAsync<T>(where, param);
#endif
        }


        /// <summary> 查询 </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection">连接对象</param>
        /// <param name="where">条件</param>
        /// <returns></returns>
        public static Task<IEnumerable<T>> FindListAsync<T>(IDbConnection connection, object where=null) where T : class
        {
#if NET40
            return new Task<IEnumerable<T>>(()=>FindList<T>(connection,where));
#else
            return connection.GetListAsync<T>(where);
#endif
        }

        /// <summary> 查询 </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection">连接对象</param>
        /// <returns></returns>
        public static Task<IEnumerable<T>> FindListAsync<T>(IDbConnection connection) where T : class
        {
#if NET40
            return new Task<IEnumerable<T>>(()=>FindList<T>(connection));
#else
            return connection.GetListAsync<T>();
#endif
        }

        /// <summary> 查询 </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection">连接对象</param>
        /// <param name="where">条件</param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static Task<IEnumerable<T>> FindListAsync<T>(IDbConnection connection, string where=null, object param = null) where T : class
        {
#if NET40
            return new Task<IEnumerable<T>>(()=>FindList<T>(connection,where,param));
#else
            return connection.GetListAsync<T>(where, param);
#endif
        }

        /// <summary> 查询 </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection">连接对象</param>
        /// <param name="where">条件</param>
        /// <param name="parameters"></param>
        /// <param name="size"></param>
        /// <param name="page"></param>
        /// <param name="orderby"></param>
        /// <returns></returns>
        public static Task<IEnumerable<T>> FindListByPageAsync<T>(IDbConnection connection, string where = null, string orderby = "", object parameters = null,int page=1, int size=10) where T : class
        {
#if NET40
            return new Task<IEnumerable<T>>(()=>FindListByPage<T>(connection,where, orderby, parameters,page, size));
#else
            return connection.GetListPagedAsync<T>(page, size,  where, orderby, parameters);
#endif
        }


        /// <summary> 查询 </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection">连接对象</param>
        /// <param name="id">id</param>
        /// <returns></returns>
        public static Task<T> FindSingleAsync<T>(IDbConnection connection, object id) where T : class
        {
#if NET40
            return new Task<T>(()=>FindSingle<T>(connection,id));
#else
            return connection.GetAsync<T>(id);
#endif
        }

        /// <summary> 查询 数量 </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection">连接对象</param>
        /// <param name="where">条件</param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static Task<int> CountAsync<T>(IDbConnection connection, string where=null, object param=null) where T : class
        {
#if NET40
            return new Task<int>(()=>Count<T>(connection,where, param));
#else
            return connection.RecordCountAsync<T>(where, param);
#endif
        }

        /// <summary> 查询 数量 </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection">连接对象</param>
        /// <param name="where">条件</param>
        /// <returns></returns>
        public static Task<int> CountAsync<T>(IDbConnection connection, object where=null) where T : class
        {
#if NET40
            return new Task<int>(()=>Count<T>(connection,where));
#else
            return connection.RecordCountAsync<T>(where);
#endif
        }


        /// <summary> 查询 </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection">连接对象</param>
        /// <param name="where">条件</param>
        /// <param name="obj"></param>
        /// <param name="size"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public static Tuple<List<T>, int> FindTupleByPage<T>(IDbConnection connection,T obj, string where = null, int page = 1, int size = 10) where T : class
        {
            var res = connection.GetListPaged<T>(page, size, where,/*"  ORDER BY  ID ASC "*/"", obj);
            var datas = res.ToList();
            var count = Count<T>(connection, where, obj);
            return new Tuple<List<T>, int>(datas, count);
        }


        /// <summary> 查询 </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection">连接对象</param>
        /// <param name="where">条件</param>
        /// <param name="obj"></param>
        /// <param name="size"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public static Task<Tuple<List<T>, int>> FindTupleByPageAsync<T>(IDbConnection connection, T obj, string where, int page = 1, int size = 10) where T : class
        {
#if NET40
            throw new NotSupportedException();
#else
            var res = connection.GetListPagedAsync<T>(page, size, where,/*"  ORDER BY  ID ASC "*/"", obj).Result;
            var datas = res.ToList();
            var count = CountAsync<T>(connection,where, obj).Result;
            return new Task<Tuple<List<T>, int>>(() => new Tuple<List<T>, int>(datas, count));
#endif
        }


        /// <summary> 查询 </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where">条件</param>
        /// <param name="obj"></param>
        /// <param name="size"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public Tuple<List<T>, int> FindTupleByPage<T>(T obj, string where = null, int page = 1, int size = 10) where T : class
        {
            return FindTupleByPage(Connection, obj, where, page, size);
        }


        /// <summary> 查询 </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where">条件</param>
        /// <param name="obj"></param>
        /// <param name="size"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public Task<Tuple<List<T>, int>> FindTupleByPageAsync<T>(T obj, string where, int page = 1, int size = 10) where T : class
        {
            return FindTupleByPageAsync(Connection, obj, where, page, size);
        }
#endregion async static method

        /// <summary>
        /// 释放资源
        /// </summary>
        public virtual void Dispose()
        {
            _connection.Close();
            _connection.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
#endif