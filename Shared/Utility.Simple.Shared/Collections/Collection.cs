using System;
using System.Collections;
using System.Collections.Generic;
#if !(NETCOREAPP1_0 || NETCOREAPP1_1 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
using System.Runtime.Serialization;
#endif
using System.Text;
using System.Threading;

namespace Utility.Collections
{
    /// <summary>
    /// key 是唯一键
    /// </summary>
    /// <typeparam name="Key"></typeparam>
    /// <typeparam name="Value"></typeparam>
#if !(NETCOREAPP1_0 || NETCOREAPP1_1 || NETCOREAPP1_2 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
    [Serializable]
#endif
    public class Collection<Key,Value>
        : IDictionary<Key, Value>, ICollection<KeyValuePair<Key, Value>>, IEnumerable<KeyValuePair<Key, Value>>,
        IEnumerable, IDictionary, ICollection
//#if Net45 || Net451 || Net452 || Net46 || Net461 || Net462 || Net47 || Net471 || Net472 || Net48 || NETCOREAPP1_0 || NETCOREAPP1_1 || NETCOREAPP1_2 || NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0 || NETCOREAPP3_1 || NET5_0 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6 || NETSTANDARD2_0 || NETSTANDARD2_1
   
        //, IReadOnlyDictionary<Key, Value>, IReadOnlyCollection<KeyValuePair<Key, Value>>
//#endif
        //,ISerializable, IDeserializationCallback
    {
        private Key[] keys;
        private Value[] values;
        private int capacity;
        private int version;
        private int size;

        /// <summary>
        /// 比较 查询 sortedlist SortedDictionary(内部使用了 TreeSet:SortedSet )  怎么感觉 一样的 (内部实现方式不同) 
        /// </summary>
        private IComparer<Key> comparer;
        private IComparer<Key> defaultComparer;
        /// <summary>
        /// 值去重 比较 为 null 不去重 
        /// </summary>
        private IEqualityComparer<Value> equalityComparer;

        /// <summary>
        ///   Dictionary
        /// </summary>
        private IEqualityComparer<Key> equalityComparerKey;
#region sortlist 

        /// <summary>
        /// 
        /// </summary>
        public Collection():this(10)
        {
           
        }
 
        /// <summary>
        /// 
        /// </summary>
        /// <param name="capacity"></param>
        public Collection(int capacity):this(capacity,(IComparer<Key>)null)
        {
            
        }

        /// <summary>
        /// sortedlist SortedDictionary(内部使用了 TreeSet:SortedSet )
        /// </summary>
        /// <param name="comparer"></param>
        public Collection(IComparer<Key> comparer) : this(10, comparer)
        {
        }

        /// <summary>
        /// sortedlist SortedDictionary(内部使用了 TreeSet:SortedSet )
        /// </summary>
        /// <param name="capacity"></param>
        /// <param name="comparer"></param>
        public Collection(int capacity, IComparer<Key> comparer)
        {
            this.capacity = capacity;
            if (capacity <= 0)
            {
                this.capacity = 100;
            }
            keys = new Key[this.capacity];
            values = new Value[this.capacity];
            this.comparer = comparer?? Comparer<Key>.Default;
        }
#endregion sortlist

#region Dictionary
        /// <summary>
        /// Dictionary
        /// </summary>
        /// <param name="equalityComparer"></param>
        public Collection(IEqualityComparer<Key> equalityComparer) : this(10, equalityComparer)
        {
        }

        /// <summary>
        /// Dictionary
        /// </summary>
        /// <param name="capacity"></param>
        /// <param name="equalityComparer"></param>
        public Collection(int capacity, IEqualityComparer<Key> equalityComparer)
        {
            this.capacity = capacity;
            if (capacity <= 0)
            {
                this.capacity = 100;
            }
            keys = new Key[this.capacity];
            values = new Value[this.capacity];
            this.equalityComparerKey = equalityComparer ?? EqualityComparer<Key>.Default;
        }
#endregion Dictionary

#region  sortlist

        /// <summary>
        /// 
        /// </summary>
        /// <param name="equalityComparer"></param>
        public Collection(IEqualityComparer<Value> equalityComparer) : this(10,(IComparer<Key>)null,equalityComparer)
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="comparer"></param>
        /// <param name="equalityComparer"></param>
        public Collection(IComparer<Key> comparer, IEqualityComparer<Value> equalityComparer) : this(10,comparer,equalityComparer)
        {
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="capacity"></param>
        /// <param name="comparer"></param>
        /// <param name="equalityComparer"></param>
        public Collection(int capacity, IComparer<Key> comparer, IEqualityComparer<Value> equalityComparer) : this(capacity,comparer)
        {
            this.equalityComparer = equalityComparer;
        }
#endregion sortlist


#region Dictionary

        /// <summary>
        /// 
        /// </summary>
        /// <param name="equalityComparer"></param>
        /// <param name="equalityComparerValue"></param>
        public Collection(IEqualityComparer<Key> equalityComparer, IEqualityComparer<Value> equalityComparerValue) : this(10, equalityComparer,equalityComparerValue)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="capacity"></param>
        /// <param name="equalityComparer"></param>
        /// <param name="equalityComparerValue"></param>
        public Collection(int capacity, IEqualityComparer<Key> equalityComparer, IEqualityComparer<Value> equalityComparerValue):this(capacity,equalityComparer)
        {
            this.equalityComparer = equalityComparerValue;
        }
#endregion Dictionary


        /// <summary>
        /// 数量
        /// </summary>
        public int Count => size;

        /// <summary>
        /// 版本
        /// </summary>
        public int Version => version;

        /// <summary>
        /// 
        /// </summary>
        protected virtual IComparer<Key> DefaultComparer {
            get { 
                if (defaultComparer == null) 
                { 
                    defaultComparer = comparer ?? Comparer<Key>.Default; 
                }
                return defaultComparer;
            }
        }
        private class CollectionKeys :Utility.Collections.Array<Key>, ICollection<Key>, ICollection
        {
            private Collection<Key, Value> _datas;
            public CollectionKeys(Collection<Key, Value> keys):base(keys.ToKeys())
            {
                this._datas = keys;
            }

            public override object SyncRoot => this._datas.SyncRoot;

        }
        private class CollectionValues : Utility.Collections.Array<Value>, ICollection<Value>, ICollection
        {
            private Collection<Key, Value> _datas;
            public CollectionValues(Collection<Key, Value> keys) : base(keys.ToValues())
            {
                this._datas = keys;
            }

            public override object SyncRoot => this._datas.SyncRoot;

        }
        /// <summary>
        /// 
        /// </summary>
        public  virtual ICollection<Key> Keys => new CollectionKeys(this);

        /// <summary>
        /// 
        /// </summary>
        public virtual ICollection<Value> Values => new CollectionValues(this);

        /// <summary>
        /// 
        /// </summary>
        public virtual bool IsReadOnly => false;

        ICollection IDictionary.Keys => (ICollection)Keys;

        ICollection IDictionary.Values => (ICollection)Values;

        /// <summary>
        /// 
        /// </summary>
        public virtual bool IsFixedSize => false;

        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public virtual bool IsSynchronized => false;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public  virtual object this[object key] { get => key is Key k?this[k]:default(Value); 
            set {
                if (key is Key k && value is Value v)
                 this[k] = v;
            } 
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual Value this[Key key] { get => GetValue(key); set => Add(key,value); }


        /// <summary>
        /// add
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public virtual void Add(Key key,Value value)
        {
            if(InnerInsert(key, value))
            {
                ++version;
            }
        }
        
        /// <summary>
        ///  exists key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual bool ContainsKey(Key key)
        { 
            // 键 值
            if (equalityComparerKey != null)
            {
                return CollectionHelper.Exists(keys, key,  equalityComparerKey, size);
            }
            //排序 键 值
            return CollectionHelper.Exists(keys, key,DefaultComparer, size);
        }

        /// <summary>
        /// exists value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual bool ContainsValue(Value value)
        {
            if (equalityComparer != null)
            {
                return CollectionHelper.Exists(values, value,  equalityComparer, size);
            }
            return CollectionHelper.Exists(values, value, Comparer<Value>.Default, size);
        }

        /// <summary>
        /// remove  key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual bool Remove(Key key)
        {
            return Remove(IndexOf(key));
        }

        /// <summary>
        /// remove index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public virtual bool Remove(int index)
        {
            if (index > -1)
            {
                int l = size;
                CollectionHelper.RemoveAt(keys, ref l, index);
                l = size;
                CollectionHelper.RemoveAt(values, ref l, index);
                size = l;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual int IndexOf(Key key)
        {
            int index = -1;
            // 键 值
            if (equalityComparerKey != null)
            {
                index = CollectionHelper.FindIndex(keys, key, size, equalityComparerKey);
            }
            else
            {
                //排序 键 值
                index = CollectionHelper.FindIndex(keys, key, size, DefaultComparer);
            }
            return index;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual Key[] ToKeys()
        {
            Key[] ks = new Key[size];
            CollectionHelper.CopyTo(keys, ks, 0, size);
            return ks;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual Value[] ToValues()
        {
            Value[] vs = new Value[size];
            CollectionHelper.CopyTo(values, vs, 0, size);
            return vs;
        }

        /// <summary>
        /// add
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual bool InnerInsert(Key key, Value value)
        {
            int ii = IndexOf(key);
            if (ii>-1)
            {
                values[ii] = value;//更新
                return false;
            }
            int index = size;
            //排序 键 值
            if (comparer != null)
            {
                bool Compare(Key x, Key y)
                {
                    return comparer.Compare(x, y) > 0;
                }
                int arrayIndex = CollectionHelper.FindIndex(keys, key, size, Compare);
                index = arrayIndex == -1 ? index : arrayIndex;
            }
            //是否 添加成功 
            bool res = CollectionHelper.Insert(ref values, index, ref size, value, null, equalityComparer, ref EmptyIndex, 10, -1);
            if (res)
            {
                size--;//调用时以自增

                //找到了 排序后 插入位置
                CollectionHelper.Insert(ref keys, index, ref size, key, null, null, ref EmptyIndex, 10, -1);//必定成功 如果失败 说明 有 bug
            }
            return res;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public  virtual Value GetValue(Key key)
        {
            TryGetValue(key, out Value value);
            return value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public   virtual bool TryGetValue(Key key, out Value value)
        {
            int index = IndexOf(key);
            if (index > -1)
            {
                value = values[index];
                return true;
            }
            value = default(Value);
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public virtual void Add(KeyValuePair<Key, Value> item)
        {
            Add(item.Key,item.Value);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Clear()
        {
            var k = default(Key);
            var v = default(Value);
            for (int i = 0; i < size; i++)
            {
                keys[i] = k;
                values[i] = v;
            }
            size = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public virtual bool Contains(KeyValuePair<Key, Value> item)
        {
            int index = IndexOf(item.Key);
            if (index > -1)
            {
                return (equalityComparer?? EqualityComparer<Value>.Default).Equals(values[index], item.Value);
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        public virtual void CopyTo(KeyValuePair<Key, Value>[] array, int arrayIndex)
        {
            if (array == null || array.Length == 0) return;

            for (int i = arrayIndex; i < array.Length&&i<size; i++)
            {
                array[i] = new KeyValuePair<Key, Value>(keys[i],values[i]);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public virtual bool Remove(KeyValuePair<Key, Value> item)
        {
            int index = IndexOf(item.Key);
            if (index > -1)
            {
                if((equalityComparer ?? EqualityComparer<Value>.Default).Equals(values[index], item.Value))
                {
                    return Remove(index);
                }
            }
            return false;
        }
        private struct Enumerator : IEnumerator<KeyValuePair<Key, Value>>, IEnumerator
        {
            private Collection<Key, Value> _datas;
            private int _index;
            public Enumerator(Collection<Key,Value> keys)
            {
                this._datas = keys;
                _index = -1;
            }
            public KeyValuePair<Key, Value> Current
            {
                get
                {
                    if (this._index < this._datas.Count)
                    {
                        return  new KeyValuePair<Key, Value>(this._datas.keys[this._index], this._datas.values[this._index]);
                    }
                    return default(KeyValuePair<Key, Value>);
                }
            }

            object IEnumerator.Current => this.Current;

            public void Dispose()
            {
                _index = -1;
            }

            public bool MoveNext()
            {
                this._index++;
                if (this._index < this._datas.Count)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            public void Reset()
            {
                _index = -1;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerator<KeyValuePair<Key, Value>> GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual bool Contains(object key)
        {
            if (key is Key k)
            {
                return ContainsKey(k);
            }
            return false;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public virtual void Add(object key, object value)
        {
            if (key is Key k&& value is Value v)
            {
                Add(k,v);
            }
        }
        private class DictionaryEnumerator : IDictionaryEnumerator, IEnumerator
        {
            private Collection<Key, Value> _datas;
            private int _index;
            public DictionaryEnumerator(Collection<Key, Value> keys)
            {
                this._datas = keys;
                _index = -1;
            }
            public object Key
            {
                get
                {
                    if (this._index < this._datas.Count)
                    {
                        return this._datas.keys[this._index];
                    }
                    return default(Key);
                }
            }

            public object Value
            {
                get
                {
                    if (this._index < this._datas.Count)
                    {
                        return this._datas.values[this._index];
                    }
                    return default(Value);
                }
            }

            public DictionaryEntry Entry
            {
                get
                {
                    if (this._index < this._datas.Count)
                    {
                        return new DictionaryEntry(this._datas.keys[this._index], this._datas.values[this._index]);
                    }
                    return default(DictionaryEntry);
                }
            }

            public object Current
            {
                get
                {
                    return Entry;
                }
            }

            public bool MoveNext()
            {
                this._index++;
                if (this._index < this._datas.Count)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            public void Reset()
            {
                _index = -1;
            }
        }
        IDictionaryEnumerator IDictionary.GetEnumerator()
        {
            return new DictionaryEnumerator(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        public  void Remove(object key)
        {
            if(key is Key k)
            {
                Remove(k);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <param name="index"></param>
        public void CopyTo(Array array, int index)
        {
            if(array is KeyValuePair<Key, Value>[] ar)
            {
                CopyTo(ar,index);
            }
        }

        private  int[] EmptyIndex = null;
        private object _syncRoot;
    }


    /// <summary>
    /// 安全 键 值 集合
    /// </summary>
    /// <typeparam name="Key"></typeparam>
    /// <typeparam name="Value"></typeparam>
    public class ConcurrentCollection<Key, Value>: Collection<Key, Value>
    {
        #region sortlist 

        /// <summary>
        /// 
        /// </summary>
        public ConcurrentCollection() : base()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="capacity"></param>
        public ConcurrentCollection(int capacity) : base(capacity, (IComparer<Key>)null)
        {

        }

        /// <summary>
        /// sortedlist SortedDictionary(内部使用了 TreeSet:SortedSet )
        /// </summary>
        /// <param name="comparer"></param>
        public ConcurrentCollection(IComparer<Key> comparer) : base( comparer)
        {
        }

        /// <summary>
        /// sortedlist SortedDictionary(内部使用了 TreeSet:SortedSet )
        /// </summary>
        /// <param name="capacity"></param>
        /// <param name="comparer"></param>
        public ConcurrentCollection(int capacity, IComparer<Key> comparer):base(capacity,comparer)
        {
          
        }
        #endregion sortlist

        #region Dictionary
        /// <summary>
        /// Dictionary
        /// </summary>
        /// <param name="equalityComparer"></param>
        public ConcurrentCollection(IEqualityComparer<Key> equalityComparer) : base( equalityComparer)
        {
        }

        /// <summary>
        /// Dictionary
        /// </summary>
        /// <param name="capacity"></param>
        /// <param name="equalityComparer"></param>
        public ConcurrentCollection(int capacity, IEqualityComparer<Key> equalityComparer):base(capacity,equalityComparer)
        {
           
        }
        #endregion Dictionary

        #region  sortlist

        /// <summary>
        /// 
        /// </summary>
        /// <param name="equalityComparer"></param>
        public ConcurrentCollection(IEqualityComparer<Value> equalityComparer) : base(equalityComparer)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="comparer"></param>
        /// <param name="equalityComparer"></param>
        public ConcurrentCollection(IComparer<Key> comparer, IEqualityComparer<Value> equalityComparer) : base(comparer, equalityComparer)
        {
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="capacity"></param>
        /// <param name="comparer"></param>
        /// <param name="equalityComparer"></param>
        public ConcurrentCollection(int capacity, IComparer<Key> comparer, IEqualityComparer<Value> equalityComparer) : base(capacity, comparer,equalityComparer)
        {
        }
        #endregion sortlist


        #region Dictionary

        /// <summary>
        /// 
        /// </summary>
        /// <param name="equalityComparer"></param>
        /// <param name="equalityComparerValue"></param>
        public ConcurrentCollection(IEqualityComparer<Key> equalityComparer, IEqualityComparer<Value> equalityComparerValue) : base(equalityComparer, equalityComparerValue)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="capacity"></param>
        /// <param name="equalityComparer"></param>
        /// <param name="equalityComparerValue"></param>
        public ConcurrentCollection(int capacity, IEqualityComparer<Key> equalityComparer, IEqualityComparer<Value> equalityComparerValue) : base(capacity, equalityComparer,equalityComparerValue)
        {
        }
        #endregion Dictionary

      
        /// <summary>
        /// remove index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public override bool Remove(int index)
        {
            lock (base.SyncRoot) return Remove(index);
        }

    

        /// <summary>
        /// add
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool InnerInsert(Key key, Value value)
        {
            lock (base.SyncRoot)
                return InnerInsert(key, value);
        }
    }
}
