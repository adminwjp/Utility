using System;
using System.Collections.Generic;
using System.Threading;
using Utility.Validate;

namespace Utility.Pool
{
    /// <summary>
    /// 对象池
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ObjectPool<T> : ObjectPool, IObjectPool<T> where T : class
    {
        private Func<T> create;

        /// <summary>
        /// 创建对象
        /// </summary>
        protected new virtual Func<T> Create {
            get { return create; }
            set { base.Create =()=> value?.Invoke();create = value; }
        }

        /// <summary>
        ///创建  对象池
        /// </summary>
        /// <returns></returns>
        public new IObjectPool<T> Build()
        {
            return this;
        }

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <returns></returns>
        public new T Get()
        {
            return (T)base.Get();
        }

        /// <summary>
        /// 归还对象
        /// 在方法结束前调用 ObjectEntries[i].Object = null 使用户是去对连接对象的引用
        /// 避免再次调用连接
        /// </summary>
        public bool Release(T obj)
        {
            return base.Release(obj);
        }

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <returns></returns>
        object IObjectPool.Get()
        {
            return base.Get();
        }

        /// <summary>
        /// 归还对象
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        bool IObjectPool.Release(object obj)
        {
            return base.Release(obj);
        }
    }

    /// <summary>
    /// 对象池
    /// </summary>
    public class ObjectPool: IObjectPool
    {
        /// <summary>
        /// 对象 集合
        /// </summary>
        protected List<ObjectEntry> ObjectEntries = new List<ObjectEntry>();
        /// <summary>
        /// 
        /// </summary>
        protected virtual Func<object> Create { get; set; }
        /// <summary>
        /// 目前连接对象的总数
        /// </summary>
        protected int _total=0;
        /// <summary>
        /// 目前在用连接对象数量
        /// </summary>
        protected int _useCount=0;
        /// <summary>连接池最少数量10</summary>
        public int MinPoolSize { get; protected set; } = 10;
        /// <summary>连接池最大数量30 </summary>
        public int MaxPoolSize { get; protected set; } = 30;

        /// <summary>连接对象空闲存活时间 100s</summary>
        public double ActiveTime  => 100000;
        /// <summary>
        /// 连接池创建时间
        /// </summary>
        public DateTime StartTime { get; protected set; }

        /// <summary> 目前在用连接对象数量 </summary>
        public int UseCount => this._useCount;

        /// <summary>目前连接对象的总数</summary>
        public int Total => this._total;
        //public int MaxActive { get; set; }
        //public int MinActive { get; set; }
        //public int MaxIdea { get; set; }
        //public int MinIdea { get; set; }

        /// <summary>
        /// 创建  对象池
        /// </summary>
        /// <returns></returns>
        public virtual IObjectPool Build()
        {
            return this;
        }

        /// <summary>
        /// 初始化对象池
        /// </summary>
        protected  virtual void Initial()
        {

            for (int i = 0; i < this.MinPoolSize; i++)
            {
                object obj = this.Create?.Invoke();
                ValidateHelper.ValidateArgumentObjectNull("obj", obj);
                ObjectEntry objectEntry = new ObjectEntry() { Object = obj, State = ObjectState.None,CreateOrUseTime=DateTime.Now };
                this.ObjectEntries.Add(objectEntry);
                Interlocked.Increment(ref _total);
            }
        }
     
        /// <summary>
        /// 释放对象池
        /// </summary>
        public virtual void Dispose()
        {
            if(ObjectEntries.Count>0)
            {
                foreach (var item in ObjectEntries)
                {
                    if(item is IDisposable disposable)
                    {
                        disposable.Dispose();
                    }
                }
            }
          
            ObjectEntries.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual object Get()
        {
            if (this.Total == this.UseCount)
            {
                // 连接已占满
                if (this.Total <= this.MaxPoolSize)
                {
                    object obj = this.Create?.Invoke();
                    ValidateHelper.ValidateArgumentObjectNull("obj", obj);
                    ObjectEntry objectEntry = new ObjectEntry() { Object = obj, State = ObjectState.Active, CreateOrUseTime = DateTime.Now };
                    Interlocked.Increment(ref _useCount);
                    Interlocked.Increment(ref _total);
                    this.ObjectEntries.Add(objectEntry);
                    return obj;
                }
                return null;
            }
            for (int i = 0; i < ObjectEntries.Count; i++)
            {
                if (ObjectEntries[i].State == ObjectState.Idea)
                {
                    // 有空闲连接
                    ObjectEntries[i].State = ObjectState.Active;
                    ObjectEntries[i].IdeaMilliseconds = 0;
                    ObjectEntries[i].CreateOrUseTime = DateTime.Now;
                    Interlocked.Increment(ref _useCount);
                    return ObjectEntries[i].Object;
                }
            }
            return default;
        }

        /// <summary>
        /// 归还连接
        /// 在方法结束前调用 ObjectEntries[i].Object = null 使用户是去对连接对象的引用
        /// 避免再次调用连接
        /// </summary>
        public virtual bool Release( object obj)
        {
            for (int i = 0; i < ObjectEntries.Count; i++)
            {
                if (object.ReferenceEquals(ObjectEntries[i].Object, obj))
                {
                    ObjectEntries[i].State = ObjectState.Idea;
                    var now = DateTime.Now;
                    ObjectEntries[i].IdeaTime = now;
                    ObjectEntries[i].IdeaMilliseconds= now.Subtract(this.StartTime).TotalMilliseconds;
                    Interlocked.Decrement(ref _useCount);
                    ObjectEntries[i].Object = null;
                    return true;
                }
            }
            return false;
        }
    }
}
