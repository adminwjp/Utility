#if true
//#if NET40 || NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48 || NET481 || NET482
namespace Utility.Wcf
{
    /// <summary>
    /// wcf 实体信息 契约接口(前提必须要有System.ServiceModel.ServiceContract注解),注意方法名不能相同 
    /// 不然 提示签名有多个这样的错误(要么在注解上改名称)
    /// </summary>
   //[System.ServiceModel.ServiceBehavior(InstanceContextMode = System.ServiceModel.InstanceContextMode.Single)]
    public    class Service<DALImpl,Entity,Key> : IService<Entity,Key>  
		where DALImpl:Utility.DAL.IDAL<Entity,Key>
		where Entity:class
    {

        /// <summary>
        /// 
        /// </summary>
        protected DALImpl  DAL { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public Service()
        {

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="dal"></param>
        public Service(DALImpl dal)
        {
            DAL = dal;
        }

        /// <summary>
        /// 添加实体类信息 (契约接口方法必须要有System.ServiceModel.OperationContract注解
        /// 支持web 必须要有 System.ServiceModel.Web.WebGet：httpget System.ServiceModel.Web.WebInvoke:httppost)
        /// </summary>
        /// <param name="obj">实体类</param>
        ///<return>返回添加结果,大于0 返回添加成功,小于等于0 返回添加失败 </return>
        public virtual int Insert(Entity obj)
        {
            return DAL.Insert(obj);
        }

         /// <summary>
         /// 修改实体类信息 
         /// (契约接口方法必须要有System.ServiceModel.OperationContract注解 支持web 必须要有 
         /// System.ServiceModel.Web.WebGet：httpget System.ServiceModel.Web.WebInvoke:httppost)
         /// </summary>
        /// <param name="obj">实体类</param>
        ///<return>返回修改结果,大于0 返回修改成功,小于等于0 返回修改失败 </return>
        public virtual int Update(Entity obj)
        {
            return DAL.Update(obj);
        }

        /// <summary>
        /// 根据id删除实体类信息 
        /// (契约接口方法必须要有System.ServiceModel.OperationContract注解 支持web 必须要有
        /// System.ServiceModel.Web.WebGet：httpget System.ServiceModel.Web.WebInvoke:httppost)
        /// </summary>
        /// <param name="id">id</param>
        ///<return>返回删除结果,大于0 返回删除成功,小于等于0 返回删除失败 </return>
        public virtual int Delete(Key id)
        {
            return DAL.Delete(id);
        }

        /// <summary>
        /// 根据id删除实体类信息(多删除)
        /// (契约接口方法必须要有System.ServiceModel.OperationContract注解 支持web
        /// 必须要有 System.ServiceModel.Web.WebGet：httpget System.ServiceModel.Web.WebInvoke:httppost)
        /// </summary>
        /// <param name="ids">id</param>
        ///<return>返回删除结果(多删除),大于0 返回删除成功(多删除),小于等于0 返回删除失败(多删除) </return>
        public virtual int DeleteList(Key[] ids)
        {
            return DAL.DeleteList(ids);
        }

         /// <summary>
         /// 根据条件查询实体类数据集信息 
         /// (契约接口方法必须要有System.ServiceModel.OperationContract注解 支持web
         /// 必须要有 System.ServiceModel.Web.WebGet：httpget System.ServiceModel.Web.WebInvoke:httppost)
         /// </summary>
        /// <param name="obj">实体类</param>
        ///<return>返回实体类数据集信息 </return>
        public virtual System.Collections.Generic.List<Entity> FindList(Entity obj)
        {
            return DAL.FindList(obj);
        }

        /// <summary>
        /// 根据条件查询实体类数据集数量信息 
        /// (契约接口方法必须要有System.ServiceModel.OperationContract注解 支持web 
        /// 必须要有 System.ServiceModel.Web.WebGet：httpget System.ServiceModel.Web.WebInvoke:httppost)
        /// </summary>
        /// <param name="obj">实体类</param>
        ///<return>返回实体类数据集数量信息</return>
        public virtual int Count(Entity obj)
        {
            return DAL.Count(obj);
        }

        /// <summary>
        /// 根据条件及分页查询实体类数据集信息 
        /// (契约接口方法必须要有System.ServiceModel.OperationContract注解 支持web 
        /// 必须要有 System.ServiceModel.Web.WebGet：httpget System.ServiceModel.Web.WebInvoke:httppost)
        /// </summary>
        /// <param name="obj">实体类</param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        ///<return>返回实体类数据集信息</return>
        public virtual System.Collections.Generic.List<Entity> FindListByPage(Entity obj, int page, int size)
        {
            return DAL.FindListByPage(obj, page, size);
        }

        /// <summary>
        /// 根据条件及分页查询实体类数据集信息和实体类数据集数量信息 
        /// (契约接口方法必须要有System.ServiceModel.OperationContract注解 支持web 
        /// 必须要有 System.ServiceModel.Web.WebGet：httpget System.ServiceModel.Web.WebInvoke:httppost)
        /// </summary>
        /// <param name="obj">实体类</param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        ///<return>返回实体类数据集信息和实体类数据集数量信息</return>
        public virtual Utility.Model.ResultModel<Entity> FindResultModelByPage(Entity obj, int page, int size)
        {
            return DAL.FindResultModelByPage(obj, page, size);
        }

    }

}
#endif