using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace Utility.Collections
{
    /// <summary>
    /// 同一个数组:
    /// 实现 原理 不能 复制过来(主要 queue 实现原理) ,不然无法 使用(根本实现不了(同一个数组 实现 难度较大 复杂度 高.没实现过))
    /// 功能原理 复制 过来 (简单).
    /// 顺序 线性表 支持 list(线性表扩充+1 实际+10) stack(先进后出) queue(先进先出) 或 SortedSet(排序表) 和HashSet(去重表)  效率 比 官方(细分 组合 )慢. 因为 这是组合(全部扔到一起 类似 js) 吧 
    /// </summary>
    /// <typeparam name="T"></typeparam>
#if !(NETCOREAPP1_0 || NETCOREAPP1_1 || NETCOREAPP1_2 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
    [Serializable]
#endif
    public class Array<T> :
#if !(NET20 || NET30 || NET35 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
        ISet<T>,
#endif
        ICollection<T>, IEnumerable<T>, IEnumerable, IList<T>,
#if Net45 || Net451 || Net452 || Net46 || Net461 || Net462 || Net47 || Net471 || Net472 || Net48 || NETCOREAPP1_0 || NETCOREAPP1_1 || NETCOREAPP1_2 || NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0 || NETCOREAPP3_1 || NET5_0 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6 || NETSTANDARD2_0 || NETSTANDARD2_1
        IReadOnlyCollection<T>, IReadOnlyList<T>, 
#endif
        ICollection, IList
    {
        /// <summary>
        /// 
        /// </summary>
        private int _head=0;//queue 头部

        ///// <summary>
        ///// queue 尾部
        ///// </summary>
        //private int _tail;

        /// <summary>
        /// 锁 默认 不使用 不安全的
        /// </summary>
        private object _syncRoot;

        /// <summary>
        /// 数组缓存
        /// </summary>
        protected T[] Items;
        /// <summary>
        ///QueueTail!=-1 时才有效 list queue 组合 使用 时 有效
        /// </summary>
        protected int[] Indexs = new int[1];
        /// <summary>
        /// 数组实际数量 list stack 可以用于 索引.一旦 掺杂 queue 话 索引 不好确定 了(组合话,各种实现原理不同,受影响)
        /// </summary>
        protected int Length;
        private IComparer<T> comparer2;


        /// <summary>
        /// 空数组
        /// </summary>
        protected readonly T[] EmptyArray = new T[0];

        /// <summary>
        /// 默认分配数组大小
        /// </summary>
        protected const int DefaultCapacity = 4;

        /// <summary>
        /// 是队列?(先进先出) 否则栈 (先进后出)
        /// </summary>
        public bool IsQueue { get; set; }

        /// <summary>
        /// 空集合
        /// </summary>
        public static readonly Utility.Collections.Array<T> Empty = new Array<T>(0);

        /// <summary>
        ///不为 null hashset
        /// </summary>
        protected IEqualityComparer<T> equalityComparer { get; set; }
        /// <summary>
        ///不为 null SortedSet
        /// </summary>
        protected IComparer<T> comparer { get; set; }

        /// <summary>
        /// 
        /// </summary>
        protected IComparer<T> DefaultComparer { 
            get 
            {
                if (comparer2 == null)
                {
                    comparer2 = comparer ?? Comparer<T>.Default;
                }
                return comparer2;
            } 
        }

        /// <summary>
        /// 初始化默认数组空间为capacity  SortedSet  HashSet
        /// <para>capacity &lt; 0 则为10</para>
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="comparer">类似 SortedSet</param>
        /// <param name="equalityComparer">类似 HashSet</param>
        internal Array(IEnumerable<T> collection, IComparer<T> comparer, IEqualityComparer<T> equalityComparer)
        {
            this.comparer = comparer;
            this.equalityComparer = equalityComparer;
            if (collection == null)
            {
                return;
            }

            ICollection<T> collection2 = collection as ICollection<T>;
            if (collection2 != null)
            {
                int count = collection2.Count;
                Items = new T[count];
                collection2.CopyTo(Items, 0);
                Length = count;
            }
            else
            {
                Length = 0;
                Items = new T[DefaultCapacity];
                foreach (T item in collection)
                {
                    Add(item);
                }
            }

        }


        /// <summary>
        /// 初始化默认数组空间为capacity  SortedSet  HashSet
        /// <para>capacity &lt; 0 则为10</para>
        /// </summary>
        /// <param name="capacity"></param>
        /// <param name="comparer">类似 SortedSet</param>
        /// <param name="equalityComparer">类似 HashSet</param>
        internal Array(int capacity, IComparer<T> comparer, IEqualityComparer<T> equalityComparer)
        {
            if (capacity <= 0)
            {
                capacity = 10;
            }
            Items = new T[capacity];
            this.comparer = comparer;
            this.equalityComparer = equalityComparer;
        }


        /// <summary>
        /// 初始化默认数组空间为capacity   HashSet
        /// <para>capacity &lt; 0 则为10</para>
        /// </summary>
        /// <param name="capacity"></param>
        /// <param name="equalityComparer">类似 HashSet</param>
        internal Array(int capacity,  IEqualityComparer<T> equalityComparer) : this(capacity, null, equalityComparer)
        {
           
        }
        /// <summary>
        /// SortedSet 初始化默认数组空间为capacity  
        /// <para>capacity &lt; 0 则为10</para>
        /// </summary>
        /// <param name="capacity"></param>
        /// <param name="comparer">类似 SortedSet</param>
        public Array( int capacity, IComparer<T> comparer) :this(capacity,comparer,null)
        {

        }

        /// <summary>
        /// list
        /// </summary>
        /// <param name="capacity"></param>
        public Array(int capacity) : this(capacity, null, null)
        {

        }
        /// <summary>
        /// list
        /// </summary>
        public Array() : this(10)
        {

        }

        /// <summary>
        ///SortedSet  初始化默认数组空间为capacity  
        /// <para>capacity &lt; 0 则为10</para>
        /// </summary>
        /// <param name="comparer">类似 SortedSet</param>
        public Array(IComparer<T> comparer) : this(10, comparer, null)
        {

        }

        /// <summary>
        /// HashSet 初始化默认数组空间为capacity    
        /// <para>capacity &lt; 0 则为10</para>
        /// </summary>
        /// <param name="equalityComparer">类似 HashSet</param>
        public Array(IEqualityComparer<T> equalityComparer) : this(10, null, equalityComparer)
        {
           
        }

        /// <summary>
        ///SortedSet  初始化默认数组空间为 capacity 10   
        /// <para>capacity &lt;= 0 则为10</para>
        /// </summary>
        /// <param name="collection"></param>
        public Array(IEnumerable<T> collection) : this(collection, null, null)
        {

        }

        /// <summary>
        ///SortedSet  初始化默认数组空间为capacity    
        /// <para>capacity &lt; 0 则为10</para>
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="comparer">类似 SortedSet</param>
        public Array(IEnumerable<T> collection, IComparer<T> comparer) : this(collection,comparer,null)
        {
           
        }
        /// <summary>
        ///HashSet  初始化默认数组空间为capacity    
        /// <para>capacity &lt; 0 则为10</para>
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="equalityComparer">类似 HashSet</param>
        public Array(IEnumerable<T> collection,  IEqualityComparer<T> equalityComparer) : this(collection, null, equalityComparer)
        {
            

        }

        /// <summary> 根据索引获取或更新 例如：0 第一个记录 -1 最后一个记录 </summary>
        /// <param name="index">索引位置 &lt; 0 则 索引等于 index + Count </param>
        /// <returns></returns>
        public virtual T this[int index]
        {
            get
            {
                int i = index < 0 ? (index + this.Length) : index;
                if (i >= 0 && i < this.Length)
                {
                    return this.Items[i];
                }
                else
                {
                    return default(T);
                }
            }
            set
            {
                int i = index < 0 ? (index + this.Length) : index;
                if (i >= 0 )
                {
                    if(i < this.Length)
                    {
                        this.Items[index] = value;
                    }
                    else
                    {
                        Insert(Length, value);
                    }
                }
                else
                {
                    //do nothing
                }
            }
        }

        object IList.this[int index]
        {
            get => this[index];
            set
            {
                if (value is T)
                    this[index] = (T)value;
                else
                {
                    //do nothing 
                }
            }
        }

        /// <summary>集合数量 </summary>
        public virtual int Count => this.Length;

        /// <summary>集合是否可读 默认false </summary>

        public virtual bool IsReadOnly => false;

        /// <summary>集合是否异步 默认false </summary>

        public virtual bool IsSynchronized => false;

        /// <summary>集合锁 默认null </summary>
        public virtual object SyncRoot
        {
            get
            {
                if (_syncRoot == null)
                {
                    Interlocked.CompareExchange<object>(ref _syncRoot, new object(), (object)null);
                }
                return _syncRoot;
            }
        }

        /// <summary>集合是否固定大小 默认false </summary>

        public virtual bool IsFixedSize => false;

        #region list

        /// <summary>添加元素 数组不够 原有基础扩充10</summary>
        public virtual void Add(T item)
        {
            Insert(this.Length, item);
        }

        /// <summary> 原有基础扩充length 默认10 net 官方默认使用1</summary>
        private void Resize(int length)
        {
            CollectionHelper.Resize(ref Items, length);
        }

        /// <summary>添加元素 数组不够 原有基础扩充10(前提元素同一类型 否则不做任何操作)</summary>
        public virtual int Add(object value)
        {
            if (value is T)
            {
                Add((T)value);
                return 1;
            }
            else
            {
                // do nothing
                return -1;
            }
        }

        /// <summary>清空数组的值 以及实际数量</summary>

        public virtual void Clear()
        {
            CollectionHelper.Clear(Items, ref Length);
        }

        /// <summary>是否存在元素</summary>
        public virtual bool Contains(T item)
        {
            return IndexOf(item) > -1;
        }

        /// <summary>是否存在元素 (前提元素同一类型 否则不做任何操作)</summary>
        public virtual bool Contains(object value)
        {
            if (value is T)
            {
                return Contains((T)value);
            }
            else
            {
                // do nothing
                return false;
            }
        }

        /// <summary>
        /// 复制数组
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        public virtual void CopyTo(T[] array, int arrayIndex)
        {
            CollectionHelper.CopyTo(Items, array, arrayIndex, Length);
        }

        /// <summary>
        /// 复制数组
        /// </summary>
        /// <returns></returns>
        public virtual T[] ToArray()
        {
            T[] items = new T[this.Length];
            CopyTo(items, 0);
            return items;
        }

        /// <summary>
        /// 复制数组
        /// </summary>
        /// <param name="array"></param>
        /// <param name="index"></param>
        public virtual void CopyTo(Array array, int index)
        {
            if (index < 0 && index >= array.Length)
            {
                return;
            }
            Array.Copy(this.Items, array, index);
        }

        /// <summary>
        /// foreach 实现
        /// </summary>
        /// <returns></returns>
        public  virtual IEnumerator<T> GetEnumerator()
        {
            return new Enumerator(this);
        }

        private struct Enumerator : IEnumerator<T>, IEnumerator
        {
            private readonly Array<T> _datas;
            private int _index;
            public Enumerator(Array<T> datas)
            {
                this._datas = datas;
                this._index = -1;
            }
            public object Current
            {
                get
                {
                    if (this._index < this._datas.Count)
                    {
                        return this._datas[this._index];
                    }
                    return default(T);
                }
            }

            T IEnumerator<T>.Current => (T)this.Current;

            public void Dispose()
            {
                //do nothing 如果 清除下次执行 linq [].Select 时 没有数据了 
                //  _datas.Clear();
                this._index = -1;
            }

            public bool MoveNext()
            {
                this._index++;
                if (this._index < this._datas.Count)
                {
                    //this._index++;//该方式出现其他问题 数据多了
                    return true;
                }
                else
                {
                    return false;
                }
            }

            public void Reset()
            {
                this._index = -1;
            }
        }

        /// <summary>是否存在元素 </summary>
        public virtual int IndexOf(T item)
        {
            return CollectionHelper.FindIndex(Items, item, Length, DefaultComparer);
        }

        /// <summary>是否存在元素 (前提元素同一类型 否则不做任何操作)</summary>
        public virtual int IndexOf(object value)
        {
            if (value is T item)
            {
                return IndexOf(item);
            }
            else
            {
                // do nothing
                return -1;
            }
        }
        /// <summary> 根据索引添加 例如：0 第一个记录 -1 最后一个记录 不符合条件不做任何操作</summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        public virtual void Insert(int index, T item)
        {
            CollectionHelper.Insert(ref Items,index,ref Length,item,comparer,equalityComparer,ref Indexs,10,-1);
        }

        /// <summary> 根据索引添加 例如：0 第一个记录 -1 最后一个记录 不符合条件不做任何操作 (前提元素同一类型 否则不做任何操作)</summary>
        public virtual void Insert(int index, object value)
        {
            if (value is T item)
            {
                Insert(index, item);
            }
            else
            {
                // do nothing
            }
        }

        /// <summary> 移除元素 不符合条件不做任何操作</summary>
        public virtual bool Remove(T item)
        {
           return CollectionHelper.Remove(Items, ref Length, item, DefaultComparer);
        }

        /// <summary> 移除元素 不符合条件不做任何操作 (前提元素同一类型 否则不做任何操作)</summary>
        public virtual void Remove(object value)
        {
            if (value is T item)
            {
                Remove(item);
            }
            else
            {
                // do nothing
            }
        }


        /// <summary> 移除元素 索引从0开始 -1表示移除最后一个元素</summary>
        public virtual void RemoveAt(int index)
        {
            CollectionHelper.RemoveAt(Items, ref Length, index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
        #endregion  list

        #region queue 先进先出
        /// <summary>
        /// 排序 无效, 去重 无效
        /// 不 执行 ICompare 类似 SortedSet(排序集合) ,
        /// 不 执行 IEqualityComparer  类似 HashSet(去重集合)
        /// </summary>
        /// <param name="item"></param>
        public virtual void Enqueue(T item)
        {
            // [1,2,3,4]
            //add 5 [1,2,3,4,5] dequeue 1 [2,3,4,5]

            CollectionHelper.Insert(ref Items, Length, ref Length,item,null,null,ref Indexs, (int)((long)Items.Length * 200L / 100),-1) ;//list
            return;

            //queue 实现 原理 这里不能用以下代码 
            //if (Length == Items.Length)
            //{
            //    //官方默认使用方式
            //    int num = (int)((long)Items.Length * 200L / 100);
            //    if (num < Items.Length + 4)
            //    {
            //        num = Items.Length + 4;
            //    }
            //    Resize(num);
            //}
            ////list stack 已 操作 如果 已有 数据 覆盖 了 咋 办 0 有数据咋办
            ////暂时不管 它 _tail

            //Items[_tail] = item;//不能更新或添加(list statck 受影响) 

            ////Insert(_tail, item);//只能添加这样 可能 造成 数组 无限 扩充 queue 使用了  list stack   不使用空间了 ?


            //_tail = (_tail + 1) % Items.Length ;
            //QueueTail = _tail;//参与 queue 操作

            //Length++;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual T Dequeue()
        {

            if (Count == 0)
            {
                return default(T);
            }
            var val = Items[0];
            RemoveAt(0);
            return val;

            //T result = Items[_head];
            //Items[_head] = default(T);
            //_head = (_head + 1) % Items.Length;

            //RemoveAt(_head - 1);//先进先出 必须移除 不然 无法 统一 操作。 已实现 Length--; (queue 实际 原理不用 删除)

            ////Length--;
            //return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual T Peek()
        {
            if (Count == 0)
            {
                return default(T);
            }
            //如果数组 删除了 找不到怎么办 则需要更新 queue  _head
            if (IsQueue)
            {
                return Items[_head];//先进先出
            }
            else
            {
                //默认使用该方式
                return Items[Length - 1];//但栈 先进后出
            }
        }

        #endregion queue 先进先出

        #region  栈 先进后出

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual T Pop()
        {
            if (Length == 0)
            {
                return default(T);
            }
            T result = Items[--Length];
            Items[Length] = default(T);
            return result;
        }

        /// <summary>
        /// 排序 无效, 去重 无效
        /// 不 执行 ICompare 类似 SortedSet(排序集合) ,
        /// 不 执行 IEqualityComparer  类似 HashSet(去重集合)
        /// </summary>
        /// <param name="item"></param>
        public virtual void Push(T item)
        {
            CollectionHelper.Insert(ref Items, Length, ref Length, item, null, null, ref Indexs, (Items.Length == 0) ? 4 : (2 * Items.Length),-1);//list
            //if (Length == Items.Length)
            //{
            //    //官方默认实现
            //    //T[] array = new T[(Items.Length == 0) ? 4 : (2 * Items.Length)];
            //    //Array.Copy(Items, 0, array, 0, Items);
            //    //Items = array;

            //    Resize((Items.Length == 0) ? 4 : Items.Length * 2);
            //}
            //Items[++Length] = item;//只能 更新 不然 添加 会造成 其他情况
        }

        #endregion 栈 先进后出

        #region set

#if !(NET20 || NET30 || NET35 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
        bool ISet<T>.Add(T item)
        {
            if(!Contains(item))
            {
               Add(item);
               return true;
            }
            return false;
        }
#endif

        private int InternalGetHashCode(T item)
        {
            if (item == null)
            {
                return 0;
            }
            return (equalityComparer != null ? equalityComparer.GetHashCode(item) : item.GetHashCode()) & int.MaxValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        public virtual void UnionWith(IEnumerable<T> other)
        {
            if (other == null)
            {
                return;
            }

            foreach (T item in other)
            {
                Add(item);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        public virtual void IntersectWith(IEnumerable<T> other)
        {
            InsertRange(new List<T>(other));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        public virtual void ExceptWith(IEnumerable<T> other)
        {
            if (other == null)
            {
                return;
            }
            if (other == this)
            {
                Clear();
            }
            else
            {
                foreach (T item in other)
                {
                    Remove(item);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        public virtual void SymmetricExceptWith(IEnumerable<T> other)
        {
            UnionWith(other);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public virtual bool IsSubsetOf(IEnumerable<T> other)
        {
            return false;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public virtual bool IsProperSubsetOf(IEnumerable<T> other)
        {
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public virtual bool IsSupersetOf(IEnumerable<T> other)
        {
            return false;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public virtual bool IsProperSupersetOf(IEnumerable<T> other)
        {

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public virtual bool Overlaps(IEnumerable<T> other)
        {
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public virtual bool SetEquals(IEnumerable<T> other)
        {
            return false;
        }

        #endregion set

        #region insert collection

        /// <summary>
        /// 
        /// </summary>
        /// <param name="collection"></param>
        public virtual void InsertRange(ICollection collection)
        {
            InsertRange(Length, collection);
        }
        /// <summary>
        /// <summary>
        /// 指定位置 添加(更新)元素  类型不是 泛型 T  则不做任何操作
        /// 要么 执行 ICompare 类似 SortedSet(排序集合) ,
        /// 要么 执行 IEqualityComparer  类似 HashSet(去重集合)
        /// </summary>
        /// </summary>
        /// <param name="index"></param>
        /// <param name="collection"></param>
        public virtual void InsertRange(int index, ICollection collection)
        {
            //数组为空 不做任何操作
            if (collection == null || collection.Count <= 0)
            {
                return;
            }
            index = index < 0 ? index + Length : index;
            //添加位置不存在或超出范围不做任何操作
            if (index < 0 || index + collection.Count >= Count)
            {
                return;
            }
            int currentLength = Length;//更新当前数量 
            IEnumerator iterator = collection.GetEnumerator();
            while (iterator.MoveNext())
            {
                if (iterator.Current is T)
                {
                    Insert(index, iterator.Current);
                    if (currentLength < Length)
                    {
                        ++index;
                        currentLength = Length;// 说明添加成功  更新当前数量 
                    }
                }
            }

        }

        /// <summary>
        /// 指定位置 添加(更新)元素 
        /// 要么 执行 ICompare 类似 SortedSet(排序集合) ,
        /// 要么 执行 IEqualityComparer  类似 HashSet(去重集合)
        /// </summary>
        /// <param name="index"></param>
        /// <param name="items"></param>
        public virtual void InsertRange(int index, T[] items)
        {
            //数组为空 不做任何操作
            if (items == null || items.Length <= 0)
            {
                return;
            }
            index = index < 0 ? index + Length : index;
            //添加位置不存在或超出范围不做任何操作
            if (index < 0 || index  > Count)
            {
                return;
            }
            int currentLength = Length;//更新当前数量 为添加前 数量
            for (int i = 0; i < items.Length; i++)
            {
                Insert(index, items[i]);
                if (currentLength < Length)
                {
                    ++index;
                    currentLength = Length;// 说明添加成功  更新当前数量 为添加前 数量
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        public virtual void InsertRange(T[] items)
        {
            InsertRange(Length, items);
        }

        /// <summary>
        /// 指定位置 添加(更新)元素  类型不是 泛型 T 则不做任何操作
        /// 要么 执行 ICompare 类似 SortedSet(排序集合) ,
        /// 要么 执行 IEqualityComparer  类似 HashSet(去重集合)
        /// </summary>
        /// <param name="index"></param>
        /// <param name="collection"></param>
        public virtual void SetRange(int index, ICollection collection)
        {
            InsertRange(index, collection);
        }

        /// <summary>
        /// 开始位置 添加(更新)元素 
        /// </summary>
        /// <param name="collection"></param>

        public virtual void SetRange(ICollection collection)
        {
            SetRange(0, collection);
        }

        /// <summary>
        /// 指定位置 更新元素
        /// 要么 执行 ICompare 类似 SortedSet(排序集合) ,
        /// 要么 执行 IEqualityComparer  类似 HashSet(去重集合)
        /// </summary>
        /// <param name="index"></param>
        /// <param name="items"></param>
        public virtual void SetRange(int index, T[] items)
        {
            InsertRange(index, items);
        }
        /// <summary>
        /// 开始位置 更新元素
        /// 要么 执行 ICompare 类似 SortedSet(排序集合) ,
        /// 要么 执行 IEqualityComparer  类似 HashSet(去重集合)
        /// </summary>
        /// <param name="items"></param>

        public virtual  void SetRange(T[] items)
        {
            SetRange(0, items);
        }
        #endregion insert collection


        /// <summary>
        /// 排序
        /// </summary>
        public virtual void Sort()
        {
            if(Length>1000)
            {
                SortHelper.BinaryInsertionSort(Items, DefaultComparer, Length);
            }
            else
            {
                SortHelper.QuickSort(Items, DefaultComparer, Length);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public virtual int FindIndex(T item)
        {
            if(equalityComparer!=null)
            {
                return CollectionHelper.FindIndex(Items, item, Length, equalityComparer);
            }
            else
            {
                return CollectionHelper.FindIndex(Items, item, Length, DefaultComparer);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public virtual bool Exists(T item)
        {
            return CollectionHelper.Exists(Items, item, equalityComparer ?? EqualityComparer<T>.Default, Length);
        }
    }

    /// <summary>
    /// 这里 直接继承, 但原生 是 组合 使用
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ConcurrentArray<T> : Array<T>
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        public override void Insert(int index, object value)
        {
            lock (SyncRoot)
                base.Insert(index, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        public override void Insert(int index, T item)
        {
            lock (SyncRoot)
                base.Insert(index, item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public override void Remove(object value)
        {
            lock (SyncRoot)
                base.Remove(value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public override bool Remove(T item)
        {
            lock (SyncRoot)
                return base.Remove(item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        public override void RemoveAt(int index)
        {
            lock (SyncRoot)
                base.RemoveAt(index);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public override void Enqueue(T item)
        {
            lock (SyncRoot)
                base.Enqueue(item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public override void Push(T item)
        {
            lock (SyncRoot)
                base.Push(item);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public void TryDequeue(out T item)
        {
            lock (SyncRoot)
            {
                T temp = base.Dequeue();
                item = temp;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public enum LinkedFlag
    {
        /// <summary>
        /// 单链
        /// </summary>
        Single=0x0,
        /// <summary>
        /// 多链
        /// </summary>
        Many=0x1,
        /// <summary>
        /// 循环
        /// </summary>
        Loop=0x2
    }

}
