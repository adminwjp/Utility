#if !(NETCOREAPP1_0 || NETCOREAPP1_1 || NETCOREAPP1_2   || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
using System;
using Utility.DAL;
using Utility.Model;
using System.Collections.Generic;
using Utility.Response;
using Utility.Enums;
using Utility.Cache;
#if !(NET20 || NET30 || NET35)
using System.Threading.Tasks;
using System.Threading;
#endif
using System.Reflection;

namespace Utility.BLL
{
    /// <summary>
    /// 公共静态业务逻辑层 不能重叠(无法确定对象) 无法使用(反射可以)
    /// </summary>
    /// <typeparam name="BLL"></typeparam>
    /// <typeparam name="DAL"></typeparam>
    /// <typeparam name="Model"></typeparam>
    /// <typeparam name="Key"></typeparam>
    public class ResponseApiBLLHelper<BLL, DAL,Model, Key>
           where BLL : BLLHelper<DAL,Model, Key>
        where DAL : DALHelper<Model, Key>
        where Model : class, IModel<Key>
    {
        /// <summary>
        /// 缓存
        /// </summary>

        protected static ICacheContent Cache { get; set; }
        /// <summary>
        /// BLL.Method 不支持这种语法
        /// </summary>
        protected static Type BLLType { get; set; }
        /// <summary>
        /// 锁
        /// </summary>
        protected static readonly object Lock = new object();

        static ResponseApiBLLHelper()
        {
            Initial();//反射 不会 进来 调用时 才会 加载
        }
        /// <summary>
        ///静态类 静态方法 继承 初始化 操作 反射 不会继承 
        /// </summary>

        protected static void Initial()
        {
            if (BLLType == null)
            {
                lock (Lock)
                {
                    if (BLLType == null)
                    {
                        BLLType = typeof(BLL);
                        //这一步 执行 没啥用 DALType 值 没 更新  还为 null 纳尼 
                        // BLLType.BaseType.GetMethod("Initial", BindingFlags.Static | BindingFlags.Public| BindingFlags.NonPublic | BindingFlags.InvokeMethod);
                    }
                }
            }
        }
        /// <summary>添加实体类信息</summary>
        /// <param name="model">实体类</param>
        /// <param name="language">语言</param>
        ///<return>返回添加结果,大于0 返回添加成功,小于等于0 返回添加失败 </return>
        public static ResponseApi Insert(Model model, Language language = Language.Chinese)
        {
            var res = (int)GetMethodInfo("Insert").Invoke(null, new object[] { model });
            return ResponseApi.Create(language, res > 0 ? Code.AddSuccess : Code.AddFail);
        }

        /// <summary>
        /// 获取方法 方法名 相同 (重载 需要跟 参数匹配 查找 未 实现) 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static MethodInfo GetMethodInfo(string name)
        {
           // Initial();
            return BLLType.BaseType.GetMethod(name,BindingFlags.Static |BindingFlags.Public | BindingFlags.InvokeMethod);
        }

        /// <summary>修改实体类信息</summary>
        /// <param name="model">实体类</param>
        /// <param name="language">语言</param>
        ///<return>返回修改结果,大于0 返回修改成功,小于等于0 返回修改失败 </return>
        public static ResponseApi Update(Model model, Language language = Language.Chinese)
        {
            var res = (int)GetMethodInfo("Update").Invoke(null, new object[] { model });
            return ResponseApi.Create(language, res > 0 ? Code.ModifySuccess : Code.ModifyFail);
        }


        /// <summary>根据id删除实体类信息</summary>
        /// <param name="id">id</param>
        /// <param name="language">语言</param>
        ///<return>返回删除结果,大于0 返回删除成功,小于等于0 返回删除失败 </return>
        public static ResponseApi Delete(Key id, Language language = Language.Chinese)
        {
            var res = (int)GetMethodInfo("Delete").Invoke(null, new object[] { id });
            return ResponseApi.Create(language, res > 0 ? Code.DeleteSuccess : Code.DeleteFail);
        }

        /// <summary>根据id删除实体类信息(多删除)</summary>
        /// <param name="ids">id</param>
        /// <param name="language">语言</param>
        ///<return>返回删除结果(多删除),大于0 返回删除成功(多删除),小于等于0 返回删除失败(多删除) </return>
        public static ResponseApi DeleteList(Key[] ids, Language language = Language.Chinese)
        {
            var res = (int)GetMethodInfo("DeleteList").Invoke(null, new object[] { ids });
            return ResponseApi.Create(language,res>0? Code.DeleteSuccess:Code.DeleteFail);
        }

        /// <summary>根据条件查询实体类数据集信息</summary>
        /// <param name="model">实体类</param>
        /// <param name="language">语言</param>
        ///<return>返回实体类数据集信息 </return>
        public static ResponseApi<List<Model>> FindList(Model model, Language language = Language.Chinese)
        {
            var res = (List<Model>)GetMethodInfo("FindList").Invoke(null, new object[] { model });
            return ResponseApi<List<Model>>.Create(language, Code.QuerySuccess).SetData(res);
        }



        /// <summary>根据条件查询实体类数据集数量信息</summary>
        /// <param name="model">实体类</param>
        /// <param name="language">语言</param>
        ///<return>返回实体类数据集数量信息</return>
        public static ResponseApi<int> Count(Model model, Language language = Language.Chinese)
        {
            var res = (int)GetMethodInfo("Count").Invoke(null, new object[] { model });
            return ResponseApi<int>.Create(language, Code.Success).SetData(res);
        }

        /// <summary>根据条件及分页查询实体类数据集信息</summary>
        /// <param name="model">实体类</param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        /// <param name="language">语言</param>
        ///<return>返回实体类数据集数量信息</return>
        public static ResponseApi<List<Model>> FindListByPage(Model model, int page, int size, Language language = Language.Chinese)
        {
            var res = (List<Model>)GetMethodInfo("FindListByPage").Invoke(null, new object[] { model, page, size });
            return ResponseApi<List<Model>>.Create(language, Code.QuerySuccess).SetData(res);
        }


        /// <summary>根据条件及分页查询实体类数据集信息和实体类数据集数量信息</summary>
        /// <param name="model">实体类</param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        /// <param name="language">语言</param>
        ///<return>返回实体类数据集信息和实体类数据集数量信息</return>
        public static ResponseApi<ResultModel<Model>> FindResultModelByPage(Model model, int page, int size,Language language = Language.Chinese)
        {
            var res= (ResultModel<Model>)GetMethodInfo("FindResultModelByPage").Invoke(null, new object[] { model, page, size });
            return ResponseApi<ResultModel<Model>>.Create(language,Code.QuerySuccess).SetData(res);
        }

#if !(NET20 || NET30 || NET35)
        /// <summary>添加实体类信息</summary>
        /// <param name="model">实体类</param>
        /// <param name="cancellationToken"></param>
       /// <param name="language">语言</param>
        ///<return>返回添加结果,大于0 返回添加成功,小于等于0 返回添加失败 </return>
        public Task<ResponseApi> InsertAsync(Model model, Language language = Language.Chinese, CancellationToken cancellationToken = default)
        {
            var res = ((Task<int>)GetMethodInfo("InsertAsync").Invoke(null, new object[] { model, cancellationToken })).Result;
            return new Task<ResponseApi>(() => ResponseApi.Create(language, res > 0 ? Code.AddSuccess : Code.AddFail));
        }

        /// <summary>修改实体类信息</summary>
        /// <param name="model">实体类</param>
        /// <param name="cancellationToken"></param>
         /// <param name="language">语言</param>
        ///<return>返回修改结果,大于0 返回修改成功,小于等于0 返回修改失败 </return>
        public static Task<ResponseApi> UpdateAsync(Model model, Language language = Language.Chinese, CancellationToken cancellationToken = default)
        {
            var res = ((Task<int>)GetMethodInfo("UpdateAsync").Invoke(null, new object[] { model, cancellationToken })).Result;
            return new Task<ResponseApi>(() => ResponseApi.Create(language, res > 0 ? Code.ModifySuccess : Code.ModifyFail));
        }

        /// <summary>根据id删除实体类信息</summary>
        /// <param name="id">id</param>
        /// <param name="cancellationToken"></param>
         /// <param name="language">语言</param>
        ///<return>返回删除结果,大于0 返回删除成功,小于等于0 返回删除失败 </return>
        public static Task<ResponseApi> DeleteAsync(Key id, Language language = Language.Chinese, CancellationToken cancellationToken = default)
        {
            var res = ((Task<int>)GetMethodInfo("DeleteAsync").Invoke(null, new object[] { id, cancellationToken })).Result;
            return new Task<ResponseApi>(() => ResponseApi.Create(language, res > 0 ? Code.DeleteSuccess : Code.DeleteFail));
        }

        /// <summary>根据id删除实体类信息(多删除)</summary>
        /// <param name="ids">id</param>
        /// <param name="cancellationToken"></param>
        /// <param name="language">语言</param>
        ///<return>返回删除结果(多删除),大于0 返回删除成功(多删除),小于等于0 返回删除失败(多删除) </return>
        public static Task<ResponseApi> DeleteListAsync(Key[] ids, Language language = Language.Chinese, CancellationToken cancellationToken = default)
        {
            GetMethodInfo("DeleteListAsync").Invoke(null, new object[] { ids, cancellationToken });
            return new Task<ResponseApi>(() => ResponseApi.Create(language, Code.DeleteSuccess));
        }

        /// <summary>根据条件查询实体类数据集信息</summary>
        /// <param name="model">实体类</param>
        /// <param name="cancellationToken"></param>
         /// <param name="language">语言</param>
        ///<return>返回实体类数据集信息 </return>
        public static Task<ResponseApi<List<Model>>> FindListAsync(Model model, Language language = Language.Chinese, CancellationToken cancellationToken = default)
        {
            var res = ((Task<List<Model>>)GetMethodInfo("FindListAsync").Invoke(null, new object[] { model, cancellationToken })).Result;
            return new Task<ResponseApi<List<Model>>>(() => ResponseApi<List<Model>>.Create(language, Code.QuerySuccess).SetData(res));
        }


        /// <summary>根据条件查询实体类数据集数量信息</summary>
        /// <param name="model">实体类</param>
        /// <param name="cancellationToken"></param>
         /// <param name="language">语言</param>
        ///<return>返回实体类数据集数量信息</return>
        public static Task<ResponseApi<int>> CountAsync(Model model, Language language = Language.Chinese, CancellationToken cancellationToken = default)
        {
            var res = ((Task<int>)GetMethodInfo("CountAsync").Invoke(null, new object[] { model, cancellationToken })).Result;
            return new Task<ResponseApi<int>>(() => ResponseApi<int>.Create(language, Code.QuerySuccess).SetData(res));
        }

        /// <summary>根据条件及分页查询实体类数据集信息</summary>
        /// <param name="model">实体类</param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        /// <param name="cancellationToken"></param>
         /// <param name="language">语言</param>
        ///<return>返回实体类数据集信息</return>
        public static Task<ResponseApi<List<Model>>> FindListByPageAsync(Model model, int page, int size,
             Language language = Language.Chinese, CancellationToken cancellationToken = default)
        {
            var res = ((Task<List<Model>>)GetMethodInfo("FindListByPageAsync").Invoke(null, new object[] { model, page, size, cancellationToken })).Result;
            return new Task<ResponseApi<List<Model>>>(() => ResponseApi<List<Model>>.Create(language, Code.QuerySuccess).SetData(res));
        }

        /// <summary>根据条件及分页查询实体类数据集信息和实体类数据集数量信息</summary>
        /// <param name="model">实体类</param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        /// <param name="cancellationToken"></param>
         /// <param name="language">语言</param>
        ///<return>返回实体类数据集信息和实体类数据集数量信息</return>
        public static Task<ResponseApi<ResultModel<Model>>> FindResultModelByPageAsync(Model model, int page, int size, 
            Language language = Language.Chinese, CancellationToken cancellationToken = default)
        {
            var res= ((Task<ResultModel<Model>>)GetMethodInfo("FindResultModelByPageAsync").Invoke(null, new object[] { model, page, size, cancellationToken })).Result;
            return new Task<ResponseApi<ResultModel<Model>>>(() => ResponseApi<ResultModel<Model>>.Create(language, Code.QuerySuccess).SetData(res));
        }

#endif
    }

    /// <summary>
    /// 统一 接口 返回 结果
    /// </summary>
    /// <typeparam name="Service"></typeparam>
    /// <typeparam name="Manager"></typeparam>
    /// <typeparam name="Model"></typeparam>
    /// <typeparam name="Key"></typeparam>
    public class ResponseApiServiceHelper<Service, Manager, Model, Key> : ResponseApiBLLHelper<Service, Manager, Model, Key>
        where Service : ServiceHelper<Manager, Model, Key>
        where Manager : ManagerHelper<Model, Key>
        where Model : class, IModel<Key>

    {

    }
}
#endif