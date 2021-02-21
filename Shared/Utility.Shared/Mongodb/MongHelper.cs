#if !(NET10 || NET11 || NET20 || NET30 || NET35 || NET40 || NETCOREAPP1_0 || NETCOREAPP1_1 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
//#if NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48 || NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0 || NETCOREAPP3_1 || NET5_0 || NETSTANDARD2_0 || NETSTANDARD2_1
using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Utility.Model;
using Utility.Regexs;

namespace Utility.Mongodb
{
    /// <summary>
    /// MongHelper mong database helper 注意 docker 已安装 mongodb window 服务启动失败 只能在docker 上安装 要么不安装docker
    /// </summary>
    public class MongHelper
    {
        public readonly FilterDefinition<dynamic> EmptyWhere = Builders<dynamic>.Filter.Empty;

        private string _dbName = string.Empty;//数据库
        private MongoDB.Driver.IMongoDatabase _database;

        private readonly MongoDB.Driver.MongoClient _mongoClient = null;//初始化数据库对象

        /// <summary>  MongDatabase地址及端口 </summary>
        /// <param name="connectionStr">数据库连接地址</param>
        /// <param name="port">端口</param>
        /// <param name="dbname">数据库</param>
        public MongHelper(HostAddress[] hosts, string dbname )
        { 
            this._dbName = string.IsNullOrEmpty(dbname) ? this._dbName : dbname;
            MongoDB.Driver.MongoClientSettings  settings=  new MongoDB.Driver.MongoClientSettings();
            var serverAddresses=new List<MongoDB.Driver.MongoServerAddress>();
            foreach (var item in hosts)
            {
                var address = new MongoDB.Driver.MongoServerAddress(item.Host, item.Port);
               ;
                serverAddresses.Add(address);
            }
            this._mongoClient = new MongoDB.Driver.MongoClient(new MongoDB.Driver.MongoClientSettings() { Servers = serverAddresses });
          
        }

        public MongHelper(string url= "mongodb://root:bbdh5201314.@192.168.99.101:27017/test")
        {
            this._mongoClient= new MongoDB.Driver.MongoClient(url);
            this._dbName = RegexHelper.GetValue(url, "\\d+:\\d+/([\\w\\W]+)");
        }

        public static MongHelper GetMongDatabase()
        {
            return new MongHelper(new HostAddress[] { new HostAddress("localhost",27017)},"test");
        }

        public static MongoDB.Driver.MongoClient GetMongDatabase(string url= "mongodb://root:bbdh5201314.@192.168.99.101:27017/test")
        {
            var mongoClient = new MongoDB.Driver.MongoClient(url);
            return mongoClient;
        }

        /// <summary>设置数据库</summary>
        /// <param name="dbName">数据库名称</param>
        /// <returns></returns>
        public virtual MongHelper Set(string dbName)
        {
            this._dbName = dbName;
            return this;
        }

#region mong
        public virtual void CreateTable(string name)
        {
            this._mongoClient.GetDatabase(this._dbName).CreateCollection(name);
        }

        //public static readonly object Lock = new object();
        //多线程下 id 重复 搞错了 id没给造成重复的了
        public static string GetId()
        {
           // lock (Lock)
            {
                return ObjectId.GenerateNewId().ToString();//多线程下id出错
            }
        }

        ///<sumary>插入</sumary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tab"></param>
        /// <param name="t"></param>

        public virtual void Insert<T>(string tab,T t)
        {
            this._mongoClient.GetDatabase(this._dbName).GetCollection<T>(tab).InsertOne(t);
        }

        ///<sumary>插入多条</sumary>
        public virtual void InsertMany<T>(string tab,IEnumerable<T> t)
        {
            this._mongoClient.GetDatabase(this._dbName).GetCollection<T>(tab).InsertMany(t);
        }

        ///<sumary>修改</sumary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tab">表</param>
        /// <param name="update">修改的内容</param>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        public virtual long Update<T>(string tab, MongoDB.Driver.UpdateDefinition<T> update, MongoDB.Driver.FilterDefinition<T> query=null)
        {
            MongoDB.Driver.IMongoCollection<T> col = this._mongoClient.GetDatabase(this._dbName).GetCollection<T>(tab);
            return col.UpdateOne(query ?? Builders<T>.Filter.Empty, update).ModifiedCount;
        }

        ///<sumary>修改</sumary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tab">表</param>
        /// <param name="update">修改的内容</param>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        public virtual long UpdateJson<T>(string tab,string update, MongoDB.Driver.FilterDefinition<T> query = null)
        {
            MongoDB.Driver.IMongoCollection<T> col = this._mongoClient.GetDatabase(this._dbName).GetCollection<T>(tab);
            return col.UpdateOne(query ?? Builders<T>.Filter.Empty,new JsonUpdateDefinition<T>(update)).ModifiedCount;
        }

        ///<sumary>修改</sumary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tab">表</param>
        /// <param name="update">修改的内容</param>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        public virtual long UpdateMany<T>(string tab, MongoDB.Driver.UpdateDefinition<T> update, MongoDB.Driver.FilterDefinition<T> query=null)
        {
            MongoDB.Driver.IMongoCollection<T> col = this._mongoClient.GetDatabase(this._dbName).GetCollection<T>(tab);
            return col.UpdateMany(query ?? Builders<T>.Filter.Empty, update).ModifiedCount;
        }

        ///<sumary>删除</sumary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tab">表</param>
        /// <param name="query">查询条件</param>
        public virtual long Delete<T>( string tab, MongoDB.Driver.FilterDefinition<T> query = null)
        {
           return DataBase.GetCollection<T>(tab).DeleteOne(query ?? Builders<T>.Filter.Empty).DeletedCount;
        }

        ///<sumary>删除</sumary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tab">表</param>
        /// <param name="query">查询条件</param>
        public virtual long DeleteMany<T>(string tab, MongoDB.Driver.FilterDefinition<T> query=null)
        {
            return DataBase.GetCollection<T>(tab).DeleteMany(query ?? Builders<T>.Filter.Empty).DeletedCount;
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbName">数据库名称</param>
        /// <param name="tab">表</param>
        /// <returns></returns>
        public virtual List<T> Query<T>(string dbName, string tab)
        {
            List<T> documents = new List<T>();
            MongoDB.Driver.IAsyncCursor<T> async = this._mongoClient.GetDatabase(dbName).GetCollection<T>(tab).FindAsync<T>(MongoDB.Driver.FilterDefinition<T>.Empty).Result;
            while (async.MoveNext())
            {
                documents.AddRange(async.Current);
            }
            return documents;
        }

        ///<sumarty>查询</sumarty>
        /// <typeparam name="T"></typeparam>
        /// <param name="tab">表</param>
        /// <param name="query">查询条件</param>
        public virtual T Find<T>(string tab, MongoDB.Driver.FilterDefinition<T> query = null, string[] fields=null) where T : class
        {
            if (fields != null && fields.Length > 0)
            {
                var json = new StringBuilder();
                json.Append("{");
                for (int i = 0; i < fields.Length; i++)
                {
                    json.Append("\"").Append(fields[i]).Append("\":1");
                    if (fields.Length - 1 != i)
                    {
                        json.Append(",");
                    }
                }
                json.Append("}");
                return DataBase.GetCollection<T>(tab).Find<T>(query ?? Builders<T>.Filter.Empty).Project<T>(
                    new JsonProjectionDefinition<T, T>(json.ToString()))
                    .Limit(1).FirstOrDefault();
            }
            else
            {
                return DataBase.GetCollection<T>(tab).Find<T>(query ?? Builders<T>.Filter.Empty)
                   .Limit(1).FirstOrDefault();
            }
        }

        public virtual List<T> FindListByQueryJsonToProjectionJson<T>(string tab, string query = null, string project = null) where T : class
        {
            if (!string.IsNullOrEmpty(project))
            {
                return DataBase.GetCollection<T>(tab).Find<T>(string.IsNullOrEmpty(query) ? Builders<T>.Filter.Empty:new JsonFilterDefinition<T>(query)).Project<T>(
                    new JsonProjectionDefinition<T, T>(project)).ToList();
            }
            else
            {
                return DataBase.GetCollection<T>(tab).Find<T>(string.IsNullOrEmpty(query) ? Builders<T>.Filter.Empty : new JsonFilterDefinition<T>(query))
                   .ToList();
            }
        }

        public  virtual T FindToProjectionJson<T>(string tab, MongoDB.Driver.FilterDefinition<T> query = null, string json = null) where T : class
        {
            if (!string.IsNullOrEmpty(json))
            {
                return DataBase.GetCollection<T>(tab).Find<T>(query ?? Builders<T>.Filter.Empty).Project<T>(
                    new JsonProjectionDefinition<T, T>(json))
                    .Limit(1)?.FirstOrDefault();
            }
            else
            {
                return DataBase.GetCollection<T>(tab).Find<T>(query ?? Builders<T>.Filter.Empty)
                   .Limit(1)?.FirstOrDefault();
            }
        }

        public virtual FindOptions<T, T> GetFindOptions<T>(string[] fields = null)
        {
            FindOptions<T, T> find = null;
            if (fields != null && fields.Length > 0)
            {
                find = new FindOptions<T, T>() { Projection = GetProjectionDefinition<T>(fields) };
            }
            return find;
        }

        public virtual ProjectionDefinition<T> GetProjectionDefinition<T>(string[] fields = null)
        {
            if (fields != null && fields.Length > 0)
            {
                var pro = Builders<T>.Projection;
                ProjectionDefinition<T> proDefinition = null;
                foreach (var item in fields)
                {
                    proDefinition = proDefinition == null ? pro.Include(item) : proDefinition.Include(item);

                }
                return proDefinition;
            }
            return null;
        }

        public virtual T FindSortDesc<T>(string tab, Expression<Func<T, object>> expression) where T : class
        {
            return FindSortDesc<T>(tab,expression,null);
        }

        public virtual T FindSortDesc<T>(string tab, Expression<Func<T, object>> expression, FilterDefinition<T> query=null) where T : class
        {
            return DataBase.GetCollection<T>(tab).Find<T>(query ?? Builders<T>.Filter.Empty).SortByDescending(expression).Limit(1).FirstOrDefault();
        }

        ///<sumarty>查询</sumarty>
        /// <typeparam name="T"></typeparam>
        /// <param name="tab">表</param>
        /// <param name="filter">查询条件</param>
        /// <param name="fields"></param>
        public virtual IEnumerable<T> FindList<T>(string tab, FilterDefinition<T> filter = null, string[] fields = null) where T : class
        {
            FindOptions<T, T> find = GetFindOptions<T>(fields);
           IAsyncCursor<T> async = DataBase.GetCollection<T>(tab).FindSync<T>(filter ?? Builders<T>.Filter.Empty, find);
            while (async.MoveNext())
            {
                 return async.Current;
            }
            return null;
        }

        public virtual List<T> FindListByPage<T>(string tab,FilterDefinition<T> filter = null, string[] fields = null, int page = 1, int size = 10) where T : class
        {
            return GetPagingData<T>(tab, filter, null, page, size, fields);
        }

        public virtual List<BsonDocument> Aggregate(string tab,string[] jsons)
        {
            var stages = new List<IPipelineStageDefinition>();
            foreach (var item in jsons)
            {
                stages.Add(new JsonPipelineStageDefinition<BsonDocument, BsonDocument>(item));
            }
            //var pipeline = new PipelineStagePipelineDefinition<BsonDocument, BsonDocument>(stages);
            //this.DataBase.Aggregate(PipelineDefinition<NoPipelineInput, BsonDocument>.Create(stages)).Current.ToList();
            return this.DataBase.GetCollection<BsonDocument>(tab).Aggregate(PipelineDefinition<BsonDocument, BsonDocument>.Create(stages)).Current.ToList();
        }

        public virtual long Count<T>(string tab, FilterDefinition<T> filter=null)
        {
            var count = DataBase.GetCollection<T>(tab).CountDocuments(filter ?? Builders<T>.Filter.Empty);
            return count;
        }

        public virtual List<T> GetPagingData<T>(string tab, FilterDefinition<T> filter=null, SortDefinition<T> sort=null, int pageIndex=1, int pageSize=100, string[] fields = null)
        {
            var query = DataBase.GetCollection<T>(tab)
             .Find(filter ?? Builders<T>.Filter.Empty);
            if (sort != null)
            {
                query = query.Sort(sort);
            }
            if (fields != null)
            {
                query = query.Project<T>(GetProjectionDefinition<T>(fields));
            }
            var list = query
                .Skip((pageIndex - 1) * pageSize)
                .Limit(pageSize)
                .ToList();
            return list;
        }

        /// <summary>分页 </summary>
        /// <param name="tab"></param>
        /// <param name="filter"></param>
        /// <param name="sort"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        public virtual ResultModel<T> GetPagingDataByResultModel<T>(string tab,FilterDefinition<T> filter=null, SortDefinition<T> sort=null, int pageIndex=1, int pageSize=10, string[]  fields=null)
        {
            var list = GetPagingData<T>(tab,filter,sort,pageIndex,pageSize,fields);
            var count = Count<T>(tab,filter);
            var resultModel = new ResultModel<T>();
            resultModel.Result = new PageModel() { };
            resultModel.Data = list;
            resultModel.Result.Page = pageIndex;
            resultModel.Result.Size = pageSize;
            resultModel.Result.Records = Convert.ToInt32(count);
            resultModel.Result.Total = (int)Math.Ceiling((double)count / (double)pageSize);

            return resultModel;
        }
        public virtual ResultModel<T> GetPagingDataByResultModel<T>(string tab, FilterDefinition<T> filter = null,  int pageIndex = 1, int pageSize = 10, string[] fields = null)
        {
            return GetPagingDataByResultModel<T>(tab, filter, null, pageIndex, pageSize, fields);
        }

        ///<sumarty>查询所有</sumarty>
        /// <typeparam name="T"></typeparam>
        /// <param name="tab">表</param>
        /// <returns></returns>
        public virtual List<T> FindAll<T>(string tab)
        {
            MongoDB.Driver.IAsyncCursor<T> async = DataBase.GetCollection<T>(tab).FindSync<T>(MongoDB.Driver.FilterDefinition<T>.Empty);
            List<T> datas = new List<T>();
            while (async.MoveNext())
            {
                datas.AddRange(async.Current.ToList<T>());
            }
            return datas;
        }

        ///<sumary>查询并移除</sumary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tab">表</param>
        /// <param name="args">查询条件</param>
        /// <returns></returns>
        public virtual T FindAndRemove<T>(string tab, MongoDB.Driver.FilterDefinition<T> args = null) where T : class
        {
            return DataBase.GetCollection<T>(tab).FindOneAndDelete<T>(args);
        }


        ///<sumary>查询并修改</sumary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tab">表</param>
        /// <param name="update">修改条件</param>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        public  virtual T FindAndUpdate<T>(string tab, MongoDB.Driver.UpdateDefinition<T> update, MongoDB.Driver.FilterDefinition<T> query=null) where T : class
        {
            return DataBase.GetCollection<T>(tab).FindOneAndUpdate<T>(query ?? Builders<T>.Filter.Empty, update);
        }
        public List<string> GetCollection()
        {
            var data = this.DataBase.ListCollectionNames().ToList();
            return data;
        }

        public virtual void Drop(string tab)
        {
            this.DataBase.DropCollection(tab);
        }

        public virtual void Rename(string name,string newName)
        {
            this.DataBase.RenameCollection(name, newName);
        }

        public virtual bool ExistsField(string name, string fieldName)
        {
           return this.Count(name, Builders<dynamic>.Filter.Exists(fieldName)) > 0 ;
        }

        public virtual bool AddField(string name, Dictionary<string, object> fields, FilterDefinition<dynamic> filter = null)
        {
            filter = filter ?? EmptyWhere;
            UpdateDefinition<dynamic> add = null;
            var update = Builders<dynamic>.Update;
            foreach (var item in fields)
            {
                add = update.Set(item.Key, item.Value);
            }
            return this.Update(name, add, filter) > 0;
        }
        public virtual bool DropField(string name, string[] fields)
        {
            UpdateDefinition<dynamic> remove = null;
            var update = Builders<dynamic>.Update;
            foreach (var item in fields)
            {
                remove = update.Unset(item);
            }
            return this.Update(name, remove) > 0 ;
        }
#endregion mong

#region mong asyn
        /// <summary>
        /// 创建 集合(相当于 数据库 中表)
        /// </summary>
        /// <param name="name"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual Task CreateTableAsync(string name,CancellationToken cancellationToken=default(CancellationToken))
        {
            return this._mongoClient.GetDatabase(this._dbName).CreateCollectionAsync(name,null, cancellationToken);
        }



        ///<sumary>插入</sumary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tab"></param>
        /// <param name="t"></param>
        /// <param name="cancellationToken"></param>
        public virtual async Task InsertAsync<T>(string tab, T t, CancellationToken cancellationToken = default)
        {
#pragma warning disable CS0618 // 类型或成员已过时
            await this._mongoClient.GetDatabase(this._dbName).GetCollection<T>(tab).InsertOneAsync(t, cancellationToken);
#pragma warning restore CS0618 // 类型或成员已过时
        }

        ///<sumary>插入多条</sumary>
        /// <param name="cancellationToken"></param>
        /// <param name="t"></param>
        /// <param name="tab"></param>
        public virtual async Task InsertManyAsync<T>(string tab, IEnumerable<T> t, CancellationToken cancellationToken = default)
        {
           await  this._mongoClient.GetDatabase(this._dbName).GetCollection<T>(tab).InsertManyAsync(t,null,cancellationToken);
        }

        ///<sumary>修改</sumary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tab">表</param>
        /// <param name="update">修改的内容</param>
        /// <param name="query">查询条件</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<long> UpdateAsync<T>(string tab, UpdateDefinition<T> update, FilterDefinition<T> query = null, CancellationToken cancellationToken = default)
        {
            IMongoCollection<T> col = this._mongoClient.GetDatabase(this._dbName).GetCollection<T>(tab);
            var updateResult = await col.UpdateOneAsync(query ?? Builders<T>.Filter.Empty, update,null,cancellationToken);
            return updateResult.ModifiedCount;
        }

        ///<sumary>修改</sumary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tab">表</param>
        /// <param name="update">修改的内容</param>
        /// <param name="query">查询条件</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<long> UpdateJsonAsync<T>(string tab, string update, FilterDefinition<T> query = null, CancellationToken cancellationToken = default)
        {
           IMongoCollection<T> col = this._mongoClient.GetDatabase(this._dbName).GetCollection<T>(tab);
            var updateResult =await col.UpdateOneAsync(query ?? Builders<T>.Filter.Empty, new JsonUpdateDefinition<T>(update),null,cancellationToken);
            return updateResult.ModifiedCount;
        }

        ///<sumary>修改</sumary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tab">表</param>
        /// <param name="update">修改的内容</param>
        /// <param name="query">查询条件</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<long> UpdateManyAsync<T>(string tab,UpdateDefinition<T> update, FilterDefinition<T> query = null, CancellationToken cancellationToken = default)
        {
            IMongoCollection<T> col = this._mongoClient.GetDatabase(this._dbName).GetCollection<T>(tab);
            var updateResult = await col.UpdateManyAsync(query ?? Builders<T>.Filter.Empty, update,null,cancellationToken);
            return updateResult.ModifiedCount;
        }

        ///<sumary>删除</sumary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tab">表</param>
        /// <param name="query">查询条件</param>
        /// <param name="cancellationToken"></param>
        public virtual async Task<long> DeleteAsync<T>(string tab, FilterDefinition<T> query = null, CancellationToken cancellationToken = default)
        {
            var deleteResult =await DataBase.GetCollection<T>(tab).DeleteOneAsync(query ?? Builders<T>.Filter.Empty,cancellationToken);
            return deleteResult.DeletedCount;
        }

        ///<sumary>删除</sumary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tab">表</param>
        /// <param name="query">查询条件</param>
        /// <param name="cancellationToken"></param>
        public virtual async Task<long> DeleteManyAsync<T>(string tab,FilterDefinition<T> query = null, CancellationToken cancellationToken = default)
        {
            var deleteResult = await DataBase.GetCollection<T>(tab).DeleteManyAsync(query ?? Builders<T>.Filter.Empty,cancellationToken);
            return deleteResult.DeletedCount;
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbName">数据库名称</param>
        /// <param name="tab">表</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<List<T>> QueryAsync<T>(string dbName, string tab, CancellationToken cancellationToken = default)
        {
            IAsyncCursor<T> async =await this._mongoClient.GetDatabase(dbName).GetCollection<T>(tab).FindAsync<T>(MongoDB.Driver.FilterDefinition<T>.Empty,null,cancellationToken);
            List<T> documents = new List<T>();
            while (async.MoveNext())
            {
                documents.AddRange(async.Current);
            }
            return documents;
        }

        ///<sumarty>查询</sumarty>
        /// <typeparam name="T"></typeparam>
        /// <param name="tab">表</param>
        /// <param name="query">查询条件</param>
        /// <param name="fields"></param>
        /// <param name="cancellationToken"></param>
        public virtual async Task<T> FindAsync<T>(string tab, FilterDefinition<T> query = null, string[] fields = null, CancellationToken cancellationToken = default) where T : class
        {
            if (fields != null && fields.Length > 0)
            {
                var json = new StringBuilder();
                json.Append("{");
                for (int i = 0; i < fields.Length; i++)
                {
                    json.Append("\"").Append(fields[i]).Append("\":1");
                    if (fields.Length - 1 != i)
                    {
                        json.Append(",");
                    }
                }
                json.Append("}");
                return await DataBase.GetCollection<T>(tab).Find<T>(query ?? Builders<T>.Filter.Empty, null).Project<T>(
                    new JsonProjectionDefinition<T, T>(json.ToString()))
                    .Limit(1).FirstOrDefaultAsync(cancellationToken);
            }
            else
            {
                return await DataBase.GetCollection<T>(tab).Find<T>(query ?? Builders<T>.Filter.Empty)
                   .Limit(1).FirstOrDefaultAsync(cancellationToken);
            }
        }


        ///<sumarty>查询</sumarty>
        /// <typeparam name="T"></typeparam>
        /// <param name="tab">表</param>
        /// <param name="query">查询条件</param>
        /// <param name="project">返回 结果</param>
        /// <param name="cancellationToken"></param>
        public virtual async Task<List<T>> FindListByQueryJsonToProjectionJsonAsync<T>(string tab, string query = null, string project = null, CancellationToken cancellationToken = default) where T : class
        {
            if (!string.IsNullOrEmpty(project))
            {
                return await DataBase.GetCollection<T>(tab).Find<T>(string.IsNullOrEmpty(query) ? Builders<T>.Filter.Empty : new JsonFilterDefinition<T>(query)).Project<T>(
                    new JsonProjectionDefinition<T, T>(project)).ToListAsync(cancellationToken);
            }
            else
            {
                return await DataBase.GetCollection<T>(tab).Find<T>(string.IsNullOrEmpty(query) ? Builders<T>.Filter.Empty : new JsonFilterDefinition<T>(query))
                   .ToListAsync(cancellationToken);
            }
        }

        ///<sumarty>查询</sumarty>
        /// <typeparam name="T"></typeparam>
        /// <param name="tab">表</param>
        /// <param name="query">查询条件</param>
        /// <param name="json">返回 结果</param>
        /// <param name="cancellationToken"></param>
        public virtual async Task<T> FindToProjectionJsonAsync<T>(string tab, FilterDefinition<T> query = null, string json = null, CancellationToken cancellationToken = default) where T : class
        {
            if (!string.IsNullOrEmpty(json))
            {
                return await DataBase.GetCollection<T>(tab).Find<T>(query ?? Builders<T>.Filter.Empty).Project<T>(
                    new JsonProjectionDefinition<T, T>(json))
                    .Limit(1)?.FirstOrDefaultAsync(cancellationToken);
            }
            else
            {
                return await DataBase.GetCollection<T>(tab).Find<T>(query ?? Builders<T>.Filter.Empty)
                   .Limit(1)?.FirstOrDefaultAsync(cancellationToken);
            }
        }

        ///<sumarty>查询</sumarty>
        /// <typeparam name="T"></typeparam>
        /// <param name="tab">表</param>
        /// <param name="sort">排序条件</param>
        /// <param name="cancellationToken"></param>
        public virtual Task<T> FindSortDescAsync<T>(string tab, Expression<Func<T, object>> sort, CancellationToken cancellationToken = default) where T : class
        {
            return FindSortDescAsync<T>(tab, sort, null,cancellationToken);
        }

        ///<sumarty>查询</sumarty>
        /// <typeparam name="T"></typeparam>
        /// <param name="tab">表</param>
        /// <param name="sort">排序条件</param>
        /// <param name="query">查询条件</param>
        /// <param name="cancellationToken"></param>
        public virtual Task<T> FindSortDescAsync<T>(string tab, Expression<Func<T, object>> sort,FilterDefinition<T> query = null, CancellationToken cancellationToken = default) where T : class
        {
            return DataBase.GetCollection<T>(tab).Find<T>(query ?? Builders<T>.Filter.Empty).SortByDescending(sort).Limit(1).FirstOrDefaultAsync(cancellationToken);
        }

        ///<sumarty>查询</sumarty>
        /// <typeparam name="T"></typeparam>
        /// <param name="tab">表</param>
        /// <param name="filter">查询条件</param>
        /// <param name="cancellationToken"></param>
        public virtual async Task<IEnumerable<T>> FindListAsync<T>(string tab, FilterDefinition<T> filter = null, string[] fields = null, CancellationToken cancellationToken = default) where T : class
        {
            FindOptions<T, T> find = GetFindOptions<T>(fields);
            IAsyncCursor<T> async = await DataBase.GetCollection<T>(tab).FindAsync<T>(filter ?? Builders<T>.Filter.Empty, find,cancellationToken);
            while (async.MoveNext())
            {
                return async.Current;
            }
            return null;
        }


        ///<sumarty>查询</sumarty>
        /// <typeparam name="T"></typeparam>
        /// <param name="tab">表</param>
        /// <param name="filter">查询条件</param>
        /// <param name="fields">返回 结果</param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <param name="cancellationToken"></param>
        public virtual Task<List<T>> FindListByPageAsync<T>(string tab, FilterDefinition<T> filter = null, string[] fields = null, int page = 1, int size = 10, CancellationToken cancellationToken = default) where T : class
        {
            return GetPagingDataAsync<T>(tab, filter, null, page, size, fields,cancellationToken);
        }


        ///<sumarty>查询</sumarty>
        /// <param name="tab">表</param>
        /// <param name="jsons">查询条件</param>
        /// <param name="cancellationToken"></param>
        public virtual async Task<List<BsonDocument>> AggregateAsync(string tab, string[] jsons, CancellationToken cancellationToken = default)
        {
            var stages = new List<IPipelineStageDefinition>();
            foreach (var item in jsons)
            {
                stages.Add(new JsonPipelineStageDefinition<BsonDocument, BsonDocument>(item));
            }
            //var pipeline = new PipelineStagePipelineDefinition<BsonDocument, BsonDocument>(stages);
            //this.DataBase.Aggregate(PipelineDefinition<NoPipelineInput, BsonDocument>.Create(stages)).Current.ToList();
            var asyncCursor = await this.DataBase.GetCollection<BsonDocument>(tab).AggregateAsync(PipelineDefinition<BsonDocument, BsonDocument>.Create(stages),null, cancellationToken);
             return asyncCursor.Current.ToList();
        }


        ///<sumarty>查询</sumarty>
        /// <typeparam name="T"></typeparam>
        /// <param name="tab">表</param>
        /// <param name="filter">查询条件</param>
        /// <param name="cancellationToken"></param>
        public virtual async Task<long> CountAsync<T>(string tab, FilterDefinition<T> filter = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            var count = await DataBase.GetCollection<T>(tab).CountDocumentsAsync(filter ?? Builders<T>.Filter.Empty,null,cancellationToken);
            return count;
        }


        ///<sumarty>查询</sumarty>
        /// <typeparam name="T"></typeparam>
        /// <param name="tab">表</param>
        /// <param name="filter">查询条件</param>
        /// <param name="fields">返回 结果</param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <param name="sort"></param>
        /// <param name="cancellationToken"></param>
        public virtual Task<List<T>> GetPagingDataAsync<T>(string tab, FilterDefinition<T> filter = null, SortDefinition<T> sort = null, int page = 1, int size = 100, string[] fields = null, CancellationToken cancellationToken = default)
        {
            var query = DataBase.GetCollection<T>(tab)
             .Find(filter ?? Builders<T>.Filter.Empty);
            if (sort != null)
            {
                query = query.Sort(sort);
            }
            if (fields != null)
            {
                query = query.Project<T>(GetProjectionDefinition<T>(fields));
            }
            var list = query
                .Skip((page - 1) * size)
                .Limit(size)
                .ToListAsync(cancellationToken);
            return list;
        }

        /// <summary>分页 </summary>
        /// <param name="tab"></param>
        /// <param name="filter"></param>
        /// <param name="sort"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<T>> GetPagingDataByResultModelAsync<T>(string tab, FilterDefinition<T> filter = null, SortDefinition<T> sort = null, int pageIndex = 1, int pageSize = 10, string[] fields = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            var list =await GetPagingDataAsync<T>(tab, filter, sort, pageIndex, pageSize, fields,cancellationToken);
            var count = await CountAsync<T>(tab, filter,cancellationToken);
            var resultModel = new ResultModel<T>();
            resultModel.Result = new PageModel() { };
            resultModel.Data = list;
            resultModel.Result.Page = pageIndex;
            resultModel.Result.Size = pageSize;
            resultModel.Result.Records = Convert.ToInt32(count);
            resultModel.Result.Total = (int)Math.Ceiling((double)count / (double)pageSize);

            return resultModel;
        }

        public  virtual Task<ResultModel<T>> GetPagingDataByResultModelAsync<T>(string tab, FilterDefinition<T> filter = null, int pageIndex = 1, int pageSize = 10, string[] fields = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return GetPagingDataByResultModelAsync<T>(tab, filter, null, pageIndex, pageSize, fields,cancellationToken);
        }

        ///<sumarty>查询所有</sumarty>
        /// <typeparam name="T"></typeparam>
        /// <param name="tab">表</param>
        /// <returns></returns>
        public virtual async Task<List<T>> FindAllAsync<T>(string tab, CancellationToken cancellationToken = default(CancellationToken))
        {
            MongoDB.Driver.IAsyncCursor<T> async = await DataBase.GetCollection<T>(tab).FindAsync<T>(MongoDB.Driver.FilterDefinition<T>.Empty,null,cancellationToken);
            List<T> datas = new List<T>();
            while (async.MoveNext())
            {
                datas.AddRange(async.Current.ToList<T>());
            }
            return datas;
        }

        ///<sumary>查询并移除</sumary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tab">表</param>
        /// <param name="args">查询条件</param>
        /// <returns></returns>
        public virtual Task<T> FindAndRemoveAsync<T>(string tab, MongoDB.Driver.FilterDefinition<T> args = null, CancellationToken cancellationToken = default(CancellationToken)) where T : class
        {
            return DataBase.GetCollection<T>(tab).FindOneAndDeleteAsync<T>(args,null,cancellationToken);
        }


        ///<sumary>查询并修改</sumary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tab">表</param>
        /// <param name="update">修改条件</param>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        public virtual Task<T> FindAndUpdateAsync<T>(string tab, MongoDB.Driver.UpdateDefinition<T> update, MongoDB.Driver.FilterDefinition<T> query = null, CancellationToken cancellationToken = default(CancellationToken)) where T : class
        {
            return DataBase.GetCollection<T>(tab).FindOneAndUpdateAsync<T>(query ?? Builders<T>.Filter.Empty, update,null,cancellationToken);
        }
        public  virtual async Task<List<string>> GetCollectionAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            var asyncCursor   =await this.DataBase.ListCollectionNamesAsync(null,cancellationToken);
            var data = asyncCursor.ToList();
            return data;
        }

        public virtual async Task DropAsync(string tab, CancellationToken cancellationToken = default(CancellationToken))
        {
           await this.DataBase.DropCollectionAsync(tab,cancellationToken);
        }

        public virtual async Task RenameAsync(string name, string newName, CancellationToken cancellationToken = default(CancellationToken))
        {
            await this.DataBase.RenameCollectionAsync(name, newName,null,cancellationToken);
        }

        public virtual async Task<bool> ExistsFieldAsync(string name, string fieldName, CancellationToken cancellationToken = default(CancellationToken))
        {
            var count = await this.CountAsync(name, Builders<dynamic>.Filter.Exists(fieldName),cancellationToken);
            return count > 0;
        }

        public  virtual async Task<bool> AddFieldAsyn(string name, Dictionary<string, object> fields, FilterDefinition<dynamic> filter = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            filter = filter ?? EmptyWhere;
            UpdateDefinition<dynamic> add = null;
            var update = Builders<dynamic>.Update;
            foreach (var item in fields)
            {
                add = update.Set(item.Key, item.Value);
            }
            var count =await this.UpdateAsync(name, add, filter, cancellationToken);
            return count > 0;
        }
        public virtual async Task<bool> DropFieldAsyn(string name, string[] fields, CancellationToken cancellationToken = default(CancellationToken))
        {
            UpdateDefinition<dynamic> remove = null;
            var update = Builders<dynamic>.Update;
            foreach (var item in fields)
            {
                remove = update.Unset(item);
            }
            var count = await this.UpdateAsync(name, remove,null,cancellationToken);
            return count > 0;
        }
#endregion mong asyn
        ///<summary>获取数据库连接</summary>
        public  virtual MongoDB.Driver.IMongoDatabase DataBase =>
            this._database??(this._database=this._mongoClient.GetDatabase(this._dbName));
    }
    public class HostAddress
    {
        public HostAddress(string host,int port)
        {
            Host = host;
            Port = port;
        }
        public string Host { get; }
        public int Port { get; }
    }
}
#endif