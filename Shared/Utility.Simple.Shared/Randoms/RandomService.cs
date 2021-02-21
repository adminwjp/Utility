using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Threading;

namespace Utility.Randoms
{
    /// <summary>
    /// 随机 服务
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RandomService<T>
    {
        private readonly List<ServiceEntry<T>> _service;
        private int _index;
        private int _weightIndex;
#if !(NET20 || NET30)
        private readonly ReaderWriterLockSlim _readerWriterLockSlim = new ReaderWriterLockSlim();
#endif
        /// <summary>
        /// 随机 服务
        /// </summary>
        /// <param name="service"></param>
        public RandomService(List<T> service)
        {
            this._service = new List<ServiceEntry<T>>(service.Count);
            foreach (var item in service)
            {
                this._service.Add(new ServiceEntry<T>() { Value=item});
            }
        }

        /// <summary>
        /// 随机 服务
        /// </summary>
        /// <param name="service"></param>
        public RandomService(List<ServiceEntry<T>> service)
        {
            this._service = service;
        }

        /// <summary>
        /// 数量
        /// </summary>
        public virtual int Count
        {
            get {
                return this._service.Count;
            }
        }
        /// <summary>
        /// 完全随机轮询
        /// </summary>
        /// <returns></returns>
        public virtual T FullRandom()
        {
            if (this.Count == 0)
            {
                return default(T);
            }
            return this._service[RandomHelper.Random.Next(this.Count)].Value;
        }
        /// <summary>
        /// 加权随机轮询
        /// </summary>
        /// <returns></returns>
        public virtual T WeightRound()
        {
            if (this.Count == 0)
            {
                return default(T);
            }
            Interlocked.CompareExchange(ref this._index, 0, this.Count);
            int num = this._weightIndex % this.Count;
            T data = Get(num);
            Interlocked.Increment(ref this._weightIndex);
            return data;
        }
        /// <summary>
        /// 平滑加权轮询
        /// </summary>
        /// <returns></returns>
        public virtual T SmoothWeightRound()
        {
#if !(NET20 || NET30)
            try
            {
                this._readerWriterLockSlim.EnterReadLock();
                return SimpleSmoothWeightRound();
            }
            finally
            {
                this._readerWriterLockSlim.ExitReadLock();
            }
#else 
            lock (this._service)
            {
                return SimpleSmoothWeightRound();
            }
#endif
        }

        /// <summary>
        /// 平滑加权轮询
        /// </summary>
        /// <returns></returns>
        private T SimpleSmoothWeightRound()
        {
            ServiceEntry<T> data = null;
            //  int allWeight = this._service.Select(it => it.Weight).Sum();
            int allWeight = 0;
            foreach (var item in this._service)
            {
                allWeight += item.Weight;
            }

            foreach (var item in this._service)
            {
                if (data == null || item.CurrentWeight > data.CurrentWeight)
                {
                    data = item;
                }
            }
            data.CurrentWeight -= allWeight;
            foreach (var item in this._service)
            {
                data.CurrentWeight = data.CurrentWeight + data.Weight;
            }
            return data.Value;
        }
        /// <summary>
        /// 完全轮询
        /// </summary>
        /// <returns></returns>
        public virtual T FullRound()
        {
            if (this.Count == 0)
            {
                return default(T);
            }
            Interlocked.CompareExchange(ref this._index, 0, this.Count);
            T data = Get(this._index);
            Interlocked.Increment(ref this._index);
            return data;
        }
        /// <summary>
        /// 负载均衡算法中的哈希算法 java  Map：TreeMap
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public virtual T HashService(T data)
        {
            int clientHash = data.GetHashCode();
            return Get(Math.Abs(clientHash ==0? 0:this.Count % clientHash));
        }
        private T Get(int index)
        {
#if !(NET20 || NET30)
            try
            {
                this._readerWriterLockSlim.EnterReadLock();
                T data = this._service[index].Value;
                return data;
            }
            finally
            {
                this._readerWriterLockSlim.ExitReadLock();
            }
#else
            lock (this._service)
            {
                T data = this._service[index].Value;
                return data;
            }
#endif
        }
      
    }

    /// <summary>
    ///  服务 实体
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ServiceEntry<T>
    {
        /// <summary>
        /// 权重
        /// </summary>
        public int Weight { get; set; }

        /// <summary>
        /// 权重
        /// </summary>
        public int CurrentWeight { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public T Value { get; set; }
        /// <summary>
        /// 使用 次数
        /// </summary>
        public int UseCount { get; set; }
    }
}
