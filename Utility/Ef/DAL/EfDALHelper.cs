//#if !(NET10 || NET11 || NET20 || NET30 || NET35 || NETCOREAPP1_0 || NETCOREAPP1_1 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
#if  NET40 ||NET45 || NET451 || NET452 || NET46 ||NET461 || NET462|| NET47 || NET471 || NET472|| NET48 ||  NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0 || NETCOREAPP3_1 || NET5_0 || NETSTANDARD2_0 || NETSTANDARD2_1
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Utility.DAL;
using Utility.Ef.Uow;
using Utility.Model;

namespace Utility.Ef.DAL
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="Model"></typeparam>
    /// <typeparam name="Key"></typeparam>
    public class EfDALHelper<Model,Key>  where Model : class, IModel<Key>
    {
        /// <summary>
        /// 
        /// </summary>
        public static EfUnitWork EfUnitWork { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class EfDALHelper 
    {
        /// <summary>
        /// 
        /// </summary>
        public static EfUnitWork EfUnitWork { get; set; }
    }

    /// <summary>数据访问层接口基类</summary>
    public class EfDALHelper<Model>  where Model : class
    {
        /// <summary>
        /// 
        /// </summary>
        public static EfUnitWork EfUnitWork { get; set; }

        /// <summary>
        /// 
        /// </summary>
        protected readonly static Func<Model, Expression<Func<Model, bool>>> WhereEmpty = (it) => null;


        private static Func<Model, Expression<Func<Model, bool>>> where;

        /// <summary>
        /// 
        /// </summary>
        public static Func<Model, Expression<Func<Model, bool>>> Where { get => where?? WhereEmpty; set => where = value; }

        /// <summary>添加实体类信息</summary>
        /// <param name="obj">实体类</param>
        ///<return>返回添加结果,大于0 返回添加成功,小于等于0 返回添加失败 </return>
        public static int Insert(Model obj)
        {
            return DALHelper.Insert(obj);
        }

        /// <summary>修改实体类信息</summary>
        /// <param name="obj">实体类</param>
        ///<return>返回修改结果,大于0 返回修改成功,小于等于0 返回修改失败 </return>
        public static int Update(Model obj)
        {
           return DALHelper.Update(obj);
        }


        /// <summary>根据条件查询实体类数据集数量信息</summary>
        /// <param name="obj">实体类</param>
        ///<return>返回实体类数据集数量信息</return>
        public static int Count(Model obj)
        {
            var count = EfUnitWork.Count(Where(obj));
            return count;
        }

        /// <summary>根据条件查询实体类数据集信息</summary>
        /// <param name="obj">实体类</param>
        ///<return>返回实体类数据集信息</return>
        public static List<Model> FindList(Model obj)
        {
            var datas = EfUnitWork.FindList(Where(obj));
            return datas;
        }

        /// <summary>根据条件及分页查询实体类数据集信息</summary>
        /// <param name="obj">实体类</param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        ///<return>返回实体类数据集信息</return>
        public static List<Model> FindListByPage(Model obj, int page=1, int size=10)
        {
            Expression<Func<Model, bool>> where = Where(obj);
            return EfUnitWork.FindListByPage(where,page, size);
        }

        /// <summary>根据条件及分页查询实体类数据集信息和实体类数据集数量信息</summary>
        /// <param name="obj">实体类</param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        ///<return>返回实体类数据集信息和实体类数据集数量信息</return>
        public static ResultModel<Model> FindResultModelByPage(Model obj, int page=1, int size=10)
        {
            Expression<Func<Model, bool>> where = Where(obj);
            return FindResultModelByPage(where, page, size);
        }

        /// <summary>根据条件及分页查询实体类数据集信息和实体类数据集数量信息</summary>
        /// <param name="where">条件</param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        ///<return>返回实体类数据集信息和实体类数据集数量信息</return>
        public static ResultModel<Model> FindResultModelByPage(Expression<Func<Model, bool>> where=null, int page=1, int size=10)
        {
            var tuple = EfUnitWork.FindTupleByPage(where, page, size);
            ResultModel<Model> result = new ResultModel<Model>(tuple,page,size);
            return result;
        }


#region async
        /// <summary>添加实体类信息</summary>
        /// <param name="obj">实体类</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回添加结果,大于0 返回添加成功,小于等于0 返回添加失败 </return>
        public static  Task<int> InsertAsync(Model obj, CancellationToken cancellationToken = default)
        {
            return DALHelper.InsertAsync(obj, cancellationToken);
        }

        /// <summary>修改实体类信息</summary>
        /// <param name="obj">实体类</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回修改结果,大于0 返回修改成功,小于等于0 返回修改失败 </return>
        public static  Task<int> UpdateAsync(Model obj, CancellationToken cancellationToken = default)
        {
            return DALHelper.UpdateAsync(obj, cancellationToken);
        }


        /// <summary>根据条件查询实体类数据集数量信息</summary>
        /// <param name="obj">实体类</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回实体类数据集数量信息</return>
        public static Task<int> CountAsync(Model obj, CancellationToken cancellationToken = default)
        {
            var count = EfUnitWork.CountAsync(Where(obj), cancellationToken);
            return count;
        }

        /// <summary>根据条件查询实体类数据集信息</summary>
        /// <param name="obj">实体类</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回实体类数据集信息</return>
        public static Task<List<Model>> FindListAsync(Model obj, CancellationToken cancellationToken = default)
        {
           return EfUnitWork.FindListAsync(Where(obj),cancellationToken);
        }

        /// <summary>根据条件及分页查询实体类数据集信息</summary>
        /// <param name="obj">实体类</param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回实体类数据集信息</return>
        public static Task<List<Model>> FindListByPageAsync(Model obj, int page, int size, CancellationToken cancellationToken = default)
        {
            return EfUnitWork.FindListByPageAsync(Where(obj),page, size,cancellationToken);
        }

        /// <summary>根据条件及分页查询实体类数据集信息和实体类数据集数量信息</summary>
        /// <param name="obj">实体类</param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回实体类数据集信息和实体类数据集数量信息</return>
        public static  Task<ResultModel<Model>> FindResultModelByPageAsync(Model obj, int page, int size, CancellationToken cancellationToken = default)
        {
            Expression<Func<Model, bool>> where = Where(obj);
            return  FindResultModelByPageAsync(where, page, size, cancellationToken);
        }


        /// <summary>根据条件及分页查询实体类数据集信息和实体类数据集数量信息</summary>
        /// <param name="where">条件</param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回实体类数据集信息和实体类数据集数量信息</return>
        public static
#if !NET40
            async
#endif
            Task<ResultModel<Model>> FindResultModelByPageAsync(Expression<Func<Model, bool>> where, int page, int size, CancellationToken cancellationToken = default)
        {
           
#if !NET40
            var data =await EfUnitWork.FindTupleByPageAsync(where, page, size, cancellationToken);
            ResultModel<Model> result = new ResultModel<Model>(data,page,size);
            return result;
#else
            var data =  EfUnitWork.FindTupleByPage(where, page, size);
            ResultModel<Model> result = new ResultModel<Model>(data,page, size);
            return new Task<ResultModel<Model>>(() => result);
#endif

        }
#endregion async
    }
}
#endif