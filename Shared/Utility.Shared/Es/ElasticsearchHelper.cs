#if !(NET10 || NET11 || NET20 || NET30 || NET35 || NET40 || NETCOREAPP1_0 || NETCOREAPP1_1 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
//#if  NET45 || NET451 || NET452 || NET46 ||NET461 || NET462|| NET47 || NET471 || NET472|| NET48 ||  NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0 || NETCOREAPP3_1 || NET5_0 || NETSTANDARD2_0 || NETSTANDARD2_1
using Elasticsearch.Net;
using System;
using System.Collections.Generic;
using Nest;
using System.Linq;
using Utility.Json;
using Utility.Json.Extensions;
using Utility.Utils;
using Utility.Regexs;

namespace Utility.Es
{
    /// <summary>
    /// 中文 特殊父 ""(不知道怎么一直匹配不上 空的 不用"" 用 none 替代 不然查询失败) 无法 精确匹配 只能用 match_phrase 
    /// </summary>
    public class ElasticsearchHelper
    {
        IElasticLowLevelClient _lowLevelClient;
        private const string  Josn = "{\"query\":{\"match_all\":{}}}";
        public const string Address = "http://127.0.0.1:9200/";
        public IElasticLowLevelClient LowLevelClient => _lowLevelClient;
        public ElasticClient Client { get; private set; }
        public string Url { get; set; }= Address;
        public ElasticsearchHelper(string url= Address)
        {
            Uri uri = new Uri(url);
            CreateElasticLowLevelClient(uri);
            CreateElasticClient(uri);

        }
        public ElasticsearchHelper(string[] urls)
        {
            var uris = urls.Select(h => new Uri(h)).ToArray();
            //  this.SetElasticLowLevelClient(uris);
            this.CreateElasticClient(uris);
            this._lowLevelClient = Client.LowLevel;
        }

        public virtual ElasticsearchHelper CreateElasticLowLevelClient(Uri uri)
        {
            var connection = new ConnectionConfiguration(uri).RequestTimeout(TimeSpan.FromMinutes(2));
            // _client = new ElasticLowLevelClient(connection);
            this.Client = new ElasticClient(uri);
            this._lowLevelClient = Client.LowLevel;
    /*        if (!this.Client.Indices.Exists(this.Index).IsValid)
                this.Client.Indices.Create(this.Index);*/
            return this;
        }

        public virtual ElasticsearchHelper CreateElasticClient(Uri uri)
        {
            var settings = new ConnectionSettings(uri);//.DefaultIndex(this.Index);
            this.Client = new ElasticClient(settings);
       /*     if (!this.Client.Indices.Exists(this.Index).IsValid)
                this.Client.Indices.Create(this.Index);*/
            return this;
        }
       
        public virtual ElasticsearchHelper CreateElasticLowLevelClient(Uri[] uris)
        {
            var pool = new StaticConnectionPool(uris);
            var connection = new ConnectionConfiguration(pool).RequestTimeout(TimeSpan.FromMinutes(2))
               .OnRequestCompleted(apiCallDetails =>
               {
                   if (apiCallDetails.HttpStatusCode == 418)
                   {
                       throw new Exception();
                   }
               })
              .ThrowExceptions();
            this._lowLevelClient = new ElasticLowLevelClient(connection); 
            return this;
        }
        
        public  virtual ElasticsearchHelper CreateElasticClient(Uri[] uris)
        {
            var pool = new SniffingConnectionPool(uris);
            var setting = new ConnectionSettings(pool)//.DefaultIndex(this.Index)
                .RequestTimeout(TimeSpan.FromMinutes(2))
               .OnRequestCompleted(apiCallDetails =>
               {
                   if (apiCallDetails.HttpStatusCode == 418)
                   {
                       throw new Exception();
                   }
               });
            this.Client = new ElasticClient(setting);
/*            if (!this.Client.Indices.Exists(this.Index).IsValid)
                this.Client.Indices.Create(this.Index);*/
            return this;
        }

        public virtual bool CreateIndex(string index)
        {
#if NET45 || NET451 || NET452 || NET46
            return Client.CreateIndex(index).IsValid ;
#else
            //map.put("index.mapping.total_fields.limit",999999999);
            var res = this.Client.Indices.Create(index).IsValid;

            return res;
#endif
        }
        public virtual bool CreateIndexIfNotExists(string index)
        {
            if (ExistsIndex(index))
            {
                return true;
            }
            return CreateIndex(index);
        }
        public virtual bool CreateIndexIfExistsDrop(string index)
        {
            if (ExistsIndex(index))
            {
                DeleteIndex(index);
            }
            return CreateIndex(index);
        }
        public virtual bool ExistsIndex(string index)
        {
#if NET45 || NET451 || NET452 || NET46
            return  Client.IndexExists(index).IsValid ;
#else
            //map.put("index.mapping.total_fields.limit",999999999);
            var res = this.Client.Indices.Exists(index).IsValid;

            return res;
#endif
        }
        public virtual bool DeleteIndex(string index)
        {
#if NET45 || NET451 || NET452 || NET46
           return    Client.DeleteIndex(index).IsValid ;
#else
            //map.put("index.mapping.total_fields.limit",999999999);
            var res = this.Client.Indices.Delete(index).IsValid;
            return res;
#endif
        }
        public virtual bool DeleteIndexIfExists(string index)
        {
            if (ExistsIndex(index))
            {
                //this.LowLevelClient.Indices.Delete<StringResponse>(index).Success;
                return DeleteIndex(index);
            }
            return true;
        }
        /// <summary>
        ///   可以删除 没数据 也 返回 true
        /// </summary>
        /// <param name="index"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public virtual bool Delete(string index,string type,string json= Josn)
        {
       
#if NET45 || NET451 || NET452 || NET46
               var res = LowLevelClient.DeleteByQuery<Elasticsearch.Net.StringResponse>(index, type,Elasticsearch.Net.PostData.Serializable(json)).Success;
#else
            //map.put("index.mapping.total_fields.limit",999999999);
            //var str= HttpUtils.PostJson($"{Url}/{index}/{type}/_delete_by_query", json);
#pragma warning disable CS0618 // 类型或成员已过时
            var res = this.LowLevelClient.DeleteByQueryUsingType<StringResponse>(index, type, PostData.String(json)).Success;
#pragma warning restore CS0618 // 类型或成员已过时
            //return false;
#endif
            return res;
        }


        public virtual bool Delete(string index, string id)
        {
#if NET45 || NET451 || NET452 || NET46
             var res = this.LowLevelClient.Delete<StringResponse>(index,"_doc", id).Success;
#else
            var res = this.LowLevelClient.Delete<StringResponse>(index, id).Success;
#endif
            return res;
        }
        public virtual bool DeleteById(string index, string type,string id)
        {
#if NET45 || NET451 || NET452 || NET46
            var res = this.LowLevelClient.Delete<StringResponse>(index,type, id).Success;
#else
#pragma warning disable CS0618 // 类型或成员已过时
            var res = this.LowLevelClient.DeleteUsingType<StringResponse>(index,type, id).Success;
#pragma warning restore CS0618 // 类型或成员已过时
#endif
            return res;
        }
        public virtual bool InsertByIndex<T>(string index,string id,T entity)where T : class
        { 
            var response= LowLevelClient.Index<StringResponse>(index,id, PostData.Serializable(entity));
            return response.Success;
        }
        public virtual bool InsertIfNotExists(string index, string id, object obj) 
        {
            var postData = PostData.Serializable(obj);
            // PostData.Serializable(new { query = new { match = obj } })
            if (Count(index, obj) > 0)
            {
                return false;
            }
            return Insert(index, id, (PostData)postData);
        }
        public virtual bool Insert(string index, string id,PostData postData)
        {
            var response = LowLevelClient.Index<StringResponse>(index, id, postData);
            return response.Success;
        }
        public virtual bool Insert<T>(T entity) where T : class
        {
            var response = Client.IndexDocument<T>(entity);
            return response.IsValid;
        }
       
        public virtual bool InsertByType<T>(string index,string type, T entity)
        {
#if NET45 || NET451 || NET452 || NET46
            var response =LowLevelClient.Index<Elasticsearch.Net.StringResponse>(index, type, Elasticsearch.Net.PostData.Serializable<T>(entity));
#else
#pragma warning disable CS0618 // 类型或成员已过时
            var response = LowLevelClient.IndexUsingType<StringResponse>(index, type, PostData.Serializable(entity));
#pragma warning restore CS0618 // 类型或成员已过时
#endif
             return response.Success;
        }
       /// <summary>
       /// id 存在 则 更新
       /// </summary>
       /// <returns></returns>
        public virtual bool InsertByType<T>(string index, string type,string id, T entity) 
        {
#if NET45 || NET451 || NET452 || NET46
           var response =LowLevelClient.Index<Elasticsearch.Net.StringResponse>(index, type,id, Elasticsearch.Net.PostData.Serializable<T>(entity));
#else
#pragma warning disable CS0618 // 类型或成员已过时
            var response = LowLevelClient.IndexUsingType<StringResponse>(index, type,id, PostData.Serializable(entity));
#pragma warning restore CS0618 // 类型或成员已过时
#endif
            return response.Success;
        }
        public virtual T FindSingle<T>(string index, string type, string id) where T :class
        {
#if NET45 || NET451 || NET452 || NET46
            var response = LowLevelClient.Get<StringResponse>(index, type, id);
#else
#pragma warning disable CS0618 // 类型或成员已过时
            var response = LowLevelClient.GetUsingType<StringResponse>(index, type, id);
#pragma warning restore CS0618 // 类型或成员已过时
#endif
            if (response.Success)
            {
                var data= JsonHelper.ToObject <T> (response.Body);
                return data;
            }
            return null;
        }
   
        public virtual List<T> FindList<T>(string index, string type,dynamic query) where T : class
        {
#if NET45 || NET451 || NET452 || NET46
             var response = LowLevelClient.Mget<Elasticsearch.Net.StringResponse>(index, type,Elasticsearch.Net.PostData.Serializable<dynamic>(query));
#else
#pragma warning disable CS0618 // 类型或成员已过时
            var response = LowLevelClient.MultiGetUsingType<StringResponse>(index, type,PostData.Serializable(query));
#pragma warning restore CS0618 // 类型或成员已过时
#endif
            if (response.Success)
            {
                var data = JsonHelper.ToObject<List<T>>(response.Body);
                return data;
            }
            return null;
        }
        public virtual bool InsertMany<T>(string index, string type,string[] ids,T[] entities) where T : class
        {
            object[] objs = new object[ids.Length*2];
            for (int i = 0; i < ids.Length; i++)
            {
                objs[i * 2] = new { index= new { _index = index, _type = type, _id = ids[i] } };
                objs[i * 2 + 1] = entities[i];
            }
            var response = LowLevelClient.Bulk<StringResponse>(PostData.MultiJson(objs));
            return response.Success;
        }
        public virtual bool Update<T>(string index,string id, T entity) where T : class
        {
#if NET45 || NET451 || NET452 || NET46
            var response = LowLevelClient.Update<StringResponse>(index,"_doc",id, PostData.Serializable(entity));
#else
            var response = LowLevelClient.Update<StringResponse>(index,id, PostData.Serializable(entity));
#endif
            return response.Success;
        }
        public virtual string Find(string index,int from=0,int size=10) 
        {
            var searchResponse = LowLevelClient.Search<StringResponse>(index, PostData.Serializable(new
            {
                from, size, query = new { match = new{} }
            }));
            var responseJson = searchResponse.Body;
            return responseJson;
        }
        /// <summary>
        /// 根据 index id 查询数据 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="index">相对 db db</param>
        /// <param name="id">pk</param>
        /// <returns></returns>
        public  virtual T FindSingle<T>(string index,string id)where T:class
        {
#if NET45 || NET451 || NET452 || NET46
            GetRequest getRequest = new GetRequest(index,"_doc",id);
#else
            GetRequest getRequest = new GetRequest(index,id);
#endif
            var  response=Client.Get<T>(getRequest);
            return response.Source;
        }
        /// <summary>
        /// 添加一个Term，一个列一个值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="musts"></param>
        /// <param name="field">要查询的列</param>
        /// <param name="value">要比较的值</param>
        public virtual  void AddTerm<T>(List<Func<QueryContainerDescriptor<T>, QueryContainer>> musts, string field,
            object value) where T : class
        {
            musts.Add(d => d.Term(field, value));
        }
        public virtual string Find(string index, string type, int from = 0, int size = 10)
        {
            var obj = new { from, size };
            return Find(index,type,obj);
        }

        public virtual String Find(string index,string type, object obj)
        {
#if NET45 || NET451 || NET452 || NET46
             var searchResponse = LowLevelClient.Search<StringResponse>(index, type, PostData.Serializable<object>(obj));
#else
#pragma warning disable CS0618 // 类型或成员已过时
            var searchResponse = LowLevelClient.SearchUsingType<StringResponse>(index, type, PostData.Serializable(obj));
#pragma warning restore CS0618 // 类型或成员已过时
#endif
            var body = searchResponse.Body;
            return body;
        }

        public virtual string Find(string index,string json= Josn)
        {
            var searchResponse = LowLevelClient.Search<StringResponse>(index, PostData.String(json));
            var body = searchResponse.Body;
            return body;
        }
        public virtual string Find(string index, object obj)
        {
            SearchRequest searchRequest = new SearchRequest();
            var searchResponse = LowLevelClient.Search<StringResponse>(index, PostData.Serializable(obj));
            var body = searchResponse.Body;
            return body;
        }
        public virtual string  DoRequest(Elasticsearch.Net.HttpMethod method, string path, PostData data = null, IRequestParameters requestParameters = null)
        {
            var searchResponse = LowLevelClient.DoRequest<StringResponse>(method, path, data, requestParameters);
            var body = searchResponse.Body;
            return body;
        }
        public virtual void SearchWhere(string index, object where) 
        {
            SearchRequest searchRequest = new SearchRequest(index);
            searchRequest.Query = GetBoolQuery(where);
            var searchResponse=Client.Search<StringResponse>(searchRequest);
        }
        private BoolQuery GetBoolQuery( object where)
        {
            IDictionary<string, object> obj = where as IDictionary<string, object>;
            BoolQuery boolQuery = new BoolQuery();
            List<QueryContainer> termQueries = new List<QueryContainer>();
            boolQuery.Must = termQueries;
            if (obj != null)
            {
                foreach (var item in obj)
                {
                    var val =  item.Value;
                    if(val is string str)
                    {
                        if(RegexHelper.IsMatch(str,RegexHelper.ChineseDoubleByte)|| RegexHelper.IsMatch(str, "[http|https|ftp]|/"))
                        {
                            termQueries.Add(new MatchPhraseQuery
                            {

                                Field = item.Key,
                                Query = str,
                            });
                            continue;
                        }
                    }
                    termQueries.Add(new TermQuery
                    {
                        /* Name = "named_query",
                         Boost = 1.1,*/
                        //IsVerbatim = true,
                        Field = item.Key,
                        Value = val,
                    });
                }
            }
            else
            {
                var type = where.GetType();
                foreach (var item in type.GetProperties())
                {
                    termQueries.Add(new TermQuery
                    {
                        //IsVerbatim = true,
                        Field = item.Name,
                        Value = item.GetValue(where),
                    });
                }
            }
            return boolQuery;
        }
        public virtual long Count(string index,object where)
        {
#if NET45 || NET451 || NET452 || NET46
           Nest.ICountRequest countRequest = new Nest.CountRequest(index);
            countRequest.Query =GetBoolQuery(where);
            var countResponse = Client.Count<Elasticsearch.Net.StringResponse>(countRequest);
          
#else
            ICountRequest countRequest = new CountRequest(index);
            countRequest.Query = GetBoolQuery(where);
            var countResponse = Client.Count(countRequest);
#endif
            var count = countResponse.Count;
            return count;
        }
        public virtual long Count(string index, PostData postData)
        {
            var searchResponse = LowLevelClient.Count<StringResponse>(index, postData);
            var body = searchResponse.Body;
            IDictionary<string, object> data = body.ToObject<IDictionary<string, object>>();
            if (data.ContainsKey("count"))
                return (long)data["count"];
            else return -1;
        }
    }
}
#endif