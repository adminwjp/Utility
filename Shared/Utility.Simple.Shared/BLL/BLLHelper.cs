#if !(NETCOREAPP1_0 || NETCOREAPP1_1 || NETCOREAPP1_2   || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
using System;
using Utility.DAL;
using Utility.Model;
using System.Collections.Generic;
#if !(NET20 || NET30 || NET35)
using System.Threading.Tasks;
using System.Threading;
#endif
using System.Reflection;
using Utility.Domain.Uow;
using Utility.Reflect;
using Utility.Cache;

namespace Utility.BLL
{
    /// <summary>
    /// 公共静态业务逻辑层 不能重叠(无法确定对象) 无法使用(反射可以)
    /// </summary>
    /// <typeparam name="DAL"></typeparam>
    /// <typeparam name="Model"></typeparam>
    /// <typeparam name="Key"></typeparam>
    public class BLLHelper<DAL,Model, Key>  where Model : class, IModel<Key> where DAL :DALHelper<Model,Key>
    {
        /// <summary>
        /// 缓存
        /// </summary>

        protected static ICacheContent Cache { get; set; }
        /// <summary>
        /// DAL.Method 不支持这种语法
        /// </summary>
        protected static Type DALType { get; set; }
        /**
         /// <summary>
        /// 抽象方法 虚 方法 重写  反射 可以 new(覆盖 ) 反射 不可以   
        /// </summary>
        public static Action StaticAction { get; set; }
         */
        /// <summary>
        /// 锁
        /// </summary>
        protected static readonly object Lock=new object();

        /// <summary>
        /// 继承 父类 先 执行 后 子类 执行
        /// </summary>
        //static BLLHelper()
        //{  
        //   StaticAction?.Invoke();
        //   Initial();//反射 不会 进来 调用时 才会 加载
        //}

        protected static void Initial()
        {
        
            if (DALType == null)
            {
                lock (Lock)
                {
                    if (DALType == null)
                    {
                        DALType = typeof(DAL);
                    }
                }
            }
        }

        /// <summary>添加实体类信息</summary>
        /// <param name="model">实体类</param>
        ///<return>返回添加结果,大于0 返回添加成功,小于等于0 返回添加失败 </return>
        public static int Insert(Model model)
        {
        
           return (int)GetMethodInfo("Insert").Invoke(null, new object[] { model});
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static MethodInfo GetMethodInfo(string name)
        {
            //StaticAction?.Invoke();
            Initial();//DALType null
            //静态 无法 继承 
            //出现 多个签名(方法重载 ) 咋办
            var method= ReflectHelper.GetMethodInfo(DALType, name, typeof(IUnitWork));
            return method;
        }

        

        /// <summary>修改实体类信息</summary>
        /// <param name="model">实体类</param>
        ///<return>返回修改结果,大于0 返回修改成功,小于等于0 返回修改失败 </return>
        public static int Update(Model model)
        {
            return (int)GetMethodInfo("Update").Invoke(null, new object[] { model });
        }


        /// <summary>根据id删除实体类信息</summary>
        /// <param name="id">id</param>
        ///<return>返回删除结果,大于0 返回删除成功,小于等于0 返回删除失败 </return>
        public static int Delete(Key id)
        {
            return (int)GetMethodInfo("Delete").Invoke(null, new object[] { id });
        }

        /// <summary>根据id删除实体类信息(多删除)</summary>
        /// <param name="ids">id</param>
        ///<return>返回删除结果(多删除),大于0 返回删除成功(多删除),小于等于0 返回删除失败(多删除) </return>
        public static int DeleteList(Key[] ids)
        {
            return (int)GetMethodInfo("DeleteList").Invoke(null, new object[] { ids });
        }

        /// <summary>根据条件查询实体类数据集信息</summary>
        /// <param name="model">实体类</param>
        ///<return>返回实体类数据集信息 </return>
        public static List<Model> FindList(Model model)
        {
            return (List<Model>)GetMethodInfo("FindList").Invoke(null, new object[] { model });
        }



        /// <summary>根据条件查询实体类数据集数量信息</summary>
        /// <param name="model">实体类</param>
        ///<return>返回实体类数据集数量信息</return>
        public static int Count(Model model)
        {
            return (int)GetMethodInfo("Count").Invoke(null, new object[] { model });
        }

        /// <summary>根据条件及分页查询实体类数据集信息</summary>
        /// <param name="model">实体类</param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        ///<return>返回实体类数据集信息</return>
        public static List<Model> FindListByPage(Model model, int page, int size)
        {
            return (List<Model>)GetMethodInfo("FindListByPage").Invoke(null, new object[] { model,page,size });
        }


        /// <summary>根据条件及分页查询实体类数据集信息和实体类数据集数量信息</summary>
        /// <param name="model">实体类</param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        ///<return>返回实体类数据集信息和实体类数据集数量信息</return>
        public static ResultModel<Model> FindResultModelByPage(Model model, int page, int size)
        {
            return (ResultModel<Model>)GetMethodInfo("FindResultModelByPage").Invoke(null, new object[] { model, page, size });
        }

#if !(NET20 || NET30 || NET35)
        /// <summary>添加实体类信息</summary>
        /// <param name="model">实体类</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回添加结果,大于0 返回添加成功,小于等于0 返回添加失败 </return>
        public Task<int> InsertAsync(Model model, CancellationToken cancellationToken = default)
        {
            return (Task<int>)GetMethodInfo("InsertAsync").Invoke(null, new object[] { model, cancellationToken });
        }

        /// <summary>修改实体类信息</summary>
        /// <param name="model">实体类</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回修改结果,大于0 返回修改成功,小于等于0 返回修改失败 </return>
        public static Task<int> UpdateAsync(Model model,CancellationToken cancellationToken = default)
        {
            return (Task<int>)GetMethodInfo("UpdateAsync").Invoke(null, new object[] { model, cancellationToken });
        }

        /// <summary>根据id删除实体类信息</summary>
        /// <param name="id">id</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回删除结果,大于0 返回删除成功,小于等于0 返回删除失败 </return>
        public static Task<int> DeleteAsync(Key id,CancellationToken cancellationToken = default)
        {
            return (Task<int>)GetMethodInfo("DeleteAsync").Invoke(null, new object[] { id, cancellationToken });
        }

        /// <summary>根据id删除实体类信息(多删除)</summary>
        /// <param name="ids">id</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回删除结果(多删除),大于0 返回删除成功(多删除),小于等于0 返回删除失败(多删除) </return>
        public static Task<int> DeleteListAsync(Key[] ids, CancellationToken cancellationToken = default)
        {
            return (Task<int>)GetMethodInfo("DeleteListAsync").Invoke(null, new object[] { ids, cancellationToken });
        }

        /// <summary>根据条件查询实体类数据集信息</summary>
        /// <param name="model">实体类</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回实体类数据集信息 </return>
        public static Task<List<Model>> FindListAsync(Model model,CancellationToken cancellationToken = default)
        {
            return (Task<List<Model>>)GetMethodInfo("FindListAsync").Invoke(null, new object[] { model, cancellationToken });
        }


        /// <summary>根据条件查询实体类数据集数量信息</summary>
        /// <param name="model">实体类</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回实体类数据集数量信息</return>
        public static Task<int> CountAsync(Model model,CancellationToken cancellationToken = default)
        {
            return (Task<int>)GetMethodInfo("CountAsync").Invoke(null, new object[] { model, cancellationToken });
        }

        /// <summary>根据条件及分页查询实体类数据集信息</summary>
        /// <param name="model">实体类</param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回实体类数据集信息</return>
        public static Task<List<Model>> FindListByPageAsync(Model model, int page, int size, CancellationToken cancellationToken = default)
        {
            return (Task<List<Model>>)GetMethodInfo("FindListByPageAsync").Invoke(null, new object[] { model,page,size, cancellationToken });
        }

        /// <summary>根据条件及分页查询实体类数据集信息和实体类数据集数量信息</summary>
        /// <param name="model">实体类</param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回实体类数据集信息和实体类数据集数量信息</return>
        public static Task<ResultModel<Model>> FindResultModelByPageAsync(Model model, int page, int size,CancellationToken cancellationToken = default)
        {
            return (Task<ResultModel<Model>>)GetMethodInfo("FindResultModelByPageAsync").Invoke(null, new object[] { model, page, size, cancellationToken });
        }

#endif
    }
    /// <summary>
    /// 公共静态业务逻辑层 不能重叠(无法确定对象) 无法使用(反射可以)
    /// </summary>
    /// <typeparam name="DAL"></typeparam>
    /// <typeparam name="Model"></typeparam>
    /// <typeparam name="Key"></typeparam>
    public class ServiceHelper<DAL, Model, Key> : BLLHelper<DAL, Model, Key>
         where DAL : DALHelper<Model, Key>
        where Model : class, IModel<Key>
    {

    }

}
#endif