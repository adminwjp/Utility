using System.Collections.Generic;
using System.Collections;
using System;

namespace Utility.Collections
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CollectionHelper<T>
    {
        /// <summary>空集合 </summary>
        public static readonly List<T> EmptyList = new List<T>(new T[0]); 
    }
    /// <summary>
    /// tail 变量(indexs) 暂时 没用 先放着 可能后期会用到 
    /// </summary>

    public class CollectionHelper
    {
        private static int[] EmptyIndex = null;

        /// <summary>集合转 list </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static List<T> List<T>(IEnumerable<T> data)
        {
            if (data == null)
            {
                return CollectionHelper<T>.EmptyList;
            }
            List<T> datas = new List<T>(data);
            return datas;
            //using (IEnumerator<T> enumerator = objs.GetEnumerator())
            //{
            //    while (enumerator.MoveNext())
            //    {
            //        datas.Add(enumerator.Current);
            //    }
            //    return datas;
            //}
        }

        /// <summary>集合移除元素 </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public static bool Remove<T>(IList<T> data, T item)
        {
            int index = data.IndexOf(item);
            if (index > -1)
            {
                data.RemoveAt(index);
            }
            return index > -1;
           
        }

        /// <summary>数组包含元素 </summary>
        /// <param name="items"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public static bool Contains<T>(T[] items, T item)
        {
            return Exists(items, item, Comparer<T>.Default);
        }


        /// <summary>复制数组</summary>
        public static void CopyTo<T>(T[] source,T[] target, int arrayIndex,int length)
        {
            arrayIndex = arrayIndex < 0 ? arrayIndex + source.Length : arrayIndex;
            if (arrayIndex < 0 && arrayIndex > length)
            {
                return;
            }
            for (int i = arrayIndex; i < arrayIndex + length && i < target.Length; i++)
            {
                target[i] = source[i];
            }
        }
        /// <summary>复制数组</summary>
        public static void CopyTo<T>(ICollection<T> source, T[] target, int arrayIndex, int length)
        {
            arrayIndex = arrayIndex < 0 ? arrayIndex + source.Count : arrayIndex;
            if (arrayIndex < 0 && arrayIndex > length)
            {
                return;
            }
            using (var iteraotr = source.GetEnumerator())
            {
                int i = arrayIndex;
                for (; iteraotr .MoveNext()&& i < arrayIndex + length && i < target.Length; i++)
                {
                    target[i] = iteraotr.Current;
                }
            }
        }

        /// <summary>
        /// 参与 实际数组 去重
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="comparer"></param>
        /// <returns></returns>
        public static T[] Distinct<T>(T[] items, IEqualityComparer<T> comparer)
        {
            //bool Compare(T x, T y)
            //{
            //    return comparer.Equals(x, y);
            //}
            return Distinct(items,null, comparer);
        }


        /// <summary>
        /// 参与 实际数组 去重
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="comparer"></param>
        /// <returns></returns>
        public static T[] Distinct<T>(T[] items, IComparer<T> comparer)
        {
            //bool Compare(T x, T y)
            //{
            //    return comparer.Compare(x, y)==0;
            //}
            return Distinct(items, comparer,null);
        }


        /// <summary>
        /// 参与 实际数组 去重
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="comparer"></param>
        /// <param name="equalityComparer"></param>
        /// <returns></returns>
        public static T[] Distinct<T>(T[] items , /*Func<T, T, bool> comparer*/IComparer<T> comparer, IEqualityComparer<T> equalityComparer)
        {
            //数据量很大时 复制:浪费 空间 和 时间  更新: 浪费  时间 (移动)
            T[] temps = new T[items.Length];
            int k = 0;
            for (int i = 0; i < items.Length; i++)
            {
                var has = false;
                for (int j = i + 1; j < items.Length; j++)
                {
                    //if (comparer(items[i], items[j]))
                    //{
                    //    has = true;
                    //    break;
                    //}
                    if (comparer != null)
                    {
                        if(comparer.Compare(items[i], items[j]) == 0)
                        {
                            has = true;
                            break;
                        }
                    }else if (equalityComparer != null)
                    {
                        if (equalityComparer.Equals(items[i], items[j]))
                        {
                            has = true;
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                if (!has)
                {
                    temps[++k] = items[i];
                }
            }
            T[] temp = new T[k];
            CopyTo(temp, temps, 0,k);
            return temp;
        }

        /// <summary>清空数组的值 以及实际数量</summary>

        public static void Clear<T>(T[] items, ref int length)
        {
            if (length == 0)
            {
                return;
            }
            T val = default(T);
            for (int i = 0; i < length; i++)
            {
                items[i] = val;
            }
            length = 0;
        }

        /// <summary> 移除元素 不符合条件不做任何操作</summary>
        public static bool Remove<T>(T[] items, ref int length, T item, IEqualityComparer<T> comparer)
        {
            bool Compare(T x, T y)
            {
                return comparer.Equals(x, y);
            }
            return Remove(items, ref length,item, Compare);
        }


        /// <summary> 移除元素 不符合条件不做任何操作</summary>
        public static bool Remove<T>(T[] items,ref int length,T item, IComparer<T> comparer)
        {
            bool Compare(T x, T y)
            {
                return comparer.Compare(x, y) == 0;
            }
            return Remove(items, ref length,item, Compare);
        }

        /// <summary> 移除元素 不符合条件不做任何操作</summary>
        public static bool Remove<T>(T[] items, ref int length, T item, Func<T, T, bool> comparer/*IComparer<T> comparer,IEqualityComparer<T> equalityComparer*/)
        {
            int index = FindIndex(items, item, length, comparer);
            if (index > -1)
            {
                RemoveAt(items, ref length, index);
                return true;
            }
            else
            {
                // do nothing
                return false;
            }
        }
        /// <summary> 移除元素 索引从0开始 -1表示移除最后一个元素</summary>
        public static void RemoveAt<T>(T[] items,ref int length, int index)
        {
            int i = index < 0 ? (index + length) : index;
            if (i >= 0 && i < length)
            {
                for (; i < length-1; i++)
                {
                    items[i] = items[i + 1];
                }
                items[i] = default(T);
                length--;
            }
            else
            {
                //do nothing
            }
        }


        /// <summary>
        /// 根据实际 数组 比较 查询 该 元素 所在位置 未查到 -1
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="item"></param>
        /// <param name="length"></param>
        /// <param name="comparer"></param>
        /// <param name="indexs"></param>
        /// <param name="tail"></param>
        /// <returns></returns>
        public static int FindIndex<T>(T[] items, T item, int length, IEqualityComparer<T> comparer,  int[] indexs=null, int tail = -1)
        {
            bool Compare(T x, T y)
            {
                return comparer.Equals(x, y);
            }
            return FindIndex(items, item, length, Compare,  indexs, tail);
        }

        /// <summary>
        /// 根据实际 数组 比较 查询 该 元素 所在位置 未查到 -1
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="item"></param>
        /// <param name="length"></param>
        /// <param name="comparer"></param>
        /// <param name="indexs"></param>
        /// <param name="tail"></param>
        /// <returns></returns>
        public static int FindIndex<T>(T[] items, T item, int length, IComparer<T> comparer,  int[] indexs=null, int tail = -1)
        {
            // IComparer 未实现 IComparable<T>. 实现 IComparable<T> 或 IComparer<T>就可以了 不然看不到具体异常
            //内部访问不支持异常 
            //提示 这一步出错
            bool Compare(T x, T y)
            {
                return comparer.Compare(x, y) == 0;
            }
            return FindIndex(items,item,length, Compare,  indexs=null,tail);
        }

        /// <summary>
        /// 根据实际 数组 比较 查询 该 元素 所在位置 未查到 -1 asc 
        /// 前提 排序好 的数组 不然 会出现其他情况
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items">数组</param>
        /// <param name="item">元素</param>
        /// <param name="length">实际 数组数量(不是数组数量)</param>
        /// <param name="comparer">比较</param>
        /// <param name="indexs">
        /// tail= -1 说明 队列 没有参与运算 i=0 否则 i=tail indexs 实际数组所在位置
        /// </param>
        /// <example>
        /// example: var a=[1,2,3]; a.dequeue(); [0,2,3] [2,3]  a.dequeue(); [0,0,3] [3]  a.insert(0,2); [2,0,3]  [2,3] //这一样 断线了 怎么 2 0 3 获取 2 3  只能存储实际数组索引
        /// </example>
        /// <param name="tail"> -1 说明 队列 没有参与运算 i=0 否则 i=tail</param>
        /// <returns></returns>
        public static int FindIndex<T>(T[] items, T item, int length, Func<T, T, bool> comparer,/*IComparer<T> comparer,IEqualityComparer<T> equalityComparer,*/  int[] indexs=null, int tail = -1)
        {
            if (tail == -1)
            {
                int i = 0;
                for (; i < i + length && i < items.Length; i++)
                {
                    if (comparer(items[i], item))
                    {
                        return i;
                    }
                    //if (comparer != null)
                    //{
                    //    //必须至少有一个对象实现 IComparable。
                    //    if (comparer.Compare(items[i], item) == 0)
                    //        return i;
                    //}
                    //else if (equalityComparer != null)
                    //{
                    //    if (equalityComparer.Equals(items[i], item))
                    //        return i;
                    //}
                    //else
                    //{
                    //    return -1;
                    //}
                }
                return -1;
            }
            else
            {
                for (int i = 0; i < indexs.Length; i++)
                {
                    int index = indexs[i];
                    if (comparer(items[index], item))
                    {
                        return index;
                    }
                    //if (comparer != null)
                    //{
                    //    if (comparer.Compare(items[index], item) == 0)
                    //        return i;
                    //}
                    //else if (equalityComparer != null)
                    //{
                    //    if (equalityComparer.Equals(items[index], item))
                    //        return i;
                    //}
                    //else
                    //{
                    //    return -1;
                    //}
                }
                return -1;
            }
        }
       

        /// <summary> 根据索引添加 例如：0 第一个记录 -1 最后一个记录 不符合条件不做任何操作</summary>
        /// <param name="items">数组大小不够 扩充大小 数组重新创建 改变其长度 默认原有基础上+10 net 官方默认 list使用1 queue 默认 空 即 4 否则 x 2</param>
        /// <param name="index">插入位置索引</param>
        /// <param name="count">默认数组实际数量 添加成功则自增1</param>
        /// <param name="item">插入元素</param>
        /// <param name="indexs">
        /// tail= -1 说明 队列 没有参与运算 i=0 否则 i=tail indexs 实际数组所在位置
        /// </param>
        /// <example>
        /// example: var a=[1,2,3]; a.dequeue(); [0,2,3] [2,3]  a.dequeue(); [0,0,3] [3]  a.insert(0,2); [2,0,3]  [2,3] //这一样 断线了 怎么 2 0 3 获取 2 3  只能存储实际数组索引
        /// </example>
        /// <param name="resize">数组大小不够 扩充大小 默认原有基础上+1</param>
        /// <param name="tail"> -1 说明 队列 没有参与运算 i=0 否则 i=tail</param>

        public static bool Insert<T>(ref T[] items, int index, ref int count, T item, ref int[] indexs, int resize = 10, int tail = -1)
        {
            return Insert(ref items, index, ref count, item, null, null, ref indexs, resize, tail);
        }

        /// <summary> 根据索引添加 例如：0 第一个记录 -1 最后一个记录 不符合条件不做任何操作</summary>
        /// <param name="items">数组大小不够 扩充大小 数组重新创建 改变其长度 默认原有基础上+10 net 官方默认 list使用1 queue 默认 空 即 4 否则 x 2</param>
        /// <param name="index">插入位置索引</param>
        /// <param name="count">默认数组实际数量 添加成功则自增1</param>
        /// <param name="item">插入元素</param>
        /// <param name="comparer"><see cref="IComparer{T}"/>为null 则 不参与排序. 不为 null 前提 排序好 的数组 不然 会出现其他情况</param>
        /// <param name="indexs">
        /// tail= -1 说明 队列 没有参与运算 i=0 否则 i=tail indexs 实际数组所在位置
        /// </param>
        /// <example>
        /// example: var a=[1,2,3]; a.dequeue(); [0,2,3] [2,3]  a.dequeue(); [0,0,3] [3]  a.insert(0,2); [2,0,3]  [2,3] //这一样 断线了 怎么 2 0 3 获取 2 3  只能存储实际数组索引
        /// </example>
        /// <param name="resize">数组大小不够 扩充大小 默认原有基础上+1</param>
        /// <param name="tail"> -1 说明 队列 没有参与运算 i=0 否则 i=tail</param>
        public static bool Insert<T>(ref T[] items, int index, ref int count, T item, IComparer<T> comparer, ref int[] indexs, int resize = 10, int tail = -1)
        {
            return Insert(ref items, index, ref count, item, comparer, null, ref indexs, resize, tail);
        }

        /// <summary> 根据索引添加 例如：0 第一个记录 -1 最后一个记录 不符合条件不做任何操作</summary>
        /// <param name="items">数组大小不够 扩充大小 数组重新创建 改变其长度 默认原有基础上+10 net 官方默认 list使用1 queue 默认 空 即 4 否则 x 2</param>
        /// <param name="index">插入位置索引</param>
        /// <param name="count">默认数组实际数量 添加成功则自增1</param>
        /// <param name="item">插入元素</param>
        /// <param name="equalityComparer"><see cref="IEqualityComparer{T}"/> 为null 不参与 去重.  不为 null 前提 去重好 的数组 不然 会出现其他情况</param>
        /// <param name="indexs">
        /// tail= -1 说明 队列 没有参与运算 i=0 否则 i=tail indexs 实际数组所在位置
        /// </param>
        /// <example>
        /// example: var a=[1,2,3]; a.dequeue(); [0,2,3] [2,3]  a.dequeue(); [0,0,3] [3]  a.insert(0,2); [2,0,3]  [2,3] //这一样 断线了 怎么 2 0 3 获取 2 3  只能存储实际数组索引
        /// </example>
        /// <param name="resize">数组大小不够 扩充大小 默认原有基础上+1</param>
        /// <param name="tail"> -1 说明 队列 没有参与运算 i=0 否则 i=tail</param>
        public static bool Insert<T>(ref T[] items, int index, ref int count, T item, IEqualityComparer<T> equalityComparer, ref int[] indexs, int resize = 10, int tail = -1)
        {
            return Insert(ref items, index, ref count, item, null, equalityComparer, ref indexs, resize, tail);
        }


        /// <summary> 根据索引添加 例如：0 第一个记录 -1 最后一个记录 不符合条件不做任何操作</summary>
        /// <param name="items">数组大小不够 扩充大小 数组重新创建 改变其长度
        /// 默认原有基础上+10 
        /// net 官方默认 list使用1 
        /// queue 默认 空 即 4 否则 x2 如果 length &lt; array.length+4， length =array.length+4 ;否则 length =array.length*2
        /// stack 默认 空 即 4 否则 x2 </param>
        /// <param name="item">插入元素</param>
        /// <param name="equalityComparer"><see cref="IEqualityComparer{T}"/> 为null 不参与 去重.  不为 null 前提 去重好 的数组 不然 会出现其他情况</param>
        /// <param name="resize">数组大小不够 扩充大小 默认原有基础上+1</param>
        /// <returns></returns>
        public static bool Insert<T>(ref T[] items,  T item, IEqualityComparer<T> equalityComparer, int resize = 10)
        {
            return Insert(ref items, items.Length,  item, equalityComparer, resize);
        }

        /// <summary> 根据索引添加 例如：0 第一个记录 -1 最后一个记录 不符合条件不做任何操作</summary>
        /// <param name="items">数组大小不够 扩充大小 数组重新创建 改变其长度
        /// 默认原有基础上+10 
        /// net 官方默认 list使用1 
        /// queue 默认 空 即 4 否则 x2 如果 length &lt; array.length+4， length =array.length+4 ;否则 length =array.length*2
        /// stack 默认 空 即 4 否则 x2 </param>
        /// <param name="index">插入位置索引 comparer不为 null 则index无效</param>
        /// <param name="item">插入元素</param>
       /// <param name="equalityComparer"><see cref="IEqualityComparer{T}"/> 为null 不参与 去重.  不为 null 前提 去重好 的数组 不然 会出现其他情况</param>
        /// <param name="resize">数组大小不够 扩充大小 默认原有基础上+1</param>
        /// <returns></returns>
        public static bool Insert<T>(ref T[] items, int index,  T item, IEqualityComparer<T> equalityComparer=null, int resize = 10)
        {
            int count= items.Length;
            return Insert(ref items, index, ref count, item, null, equalityComparer, resize);
        }

        /// <summary> 根据索引添加 例如：0 第一个记录 -1 最后一个记录 不符合条件不做任何操作</summary>
        /// <param name="items">数组大小不够 扩充大小 数组重新创建 改变其长度
        /// 默认原有基础上+10 
        /// net 官方默认 list使用1 
        /// queue 默认 空 即 4 否则 x2 如果 length &lt; array.length+4， length =array.length+4 ;否则 length =array.length*2
        /// stack 默认 空 即 4 否则 x2 </param>
        /// <param name="index">插入位置索引 comparer不为 null 则index无效</param>
        /// <param name="count">默认数组实际数量 添加成功则自增1</param>
        /// <param name="item">插入元素</param>
        /// <param name="equalityComparer"><see cref="IEqualityComparer{T}"/> 为null 不参与 去重.  不为 null 前提 去重好 的数组 不然 会出现其他情况</param>
        /// <param name="resize">数组大小不够 扩充大小 默认原有基础上+1</param>
        /// <returns></returns>
        public static bool Insert<T>(ref T[] items, int index, ref int count, T item, IEqualityComparer<T> equalityComparer=null, int resize = 10)
        {
            return Insert(ref items, index, ref count, item, null, equalityComparer,  resize);
        }


        //public static Target Select<Source, Target>(Func<Source, Target> select)
        //{
            
        //}

        /// <summary> 根据索引添加 例如：0 第一个记录 -1 最后一个记录 不符合条件不做任何操作</summary>
        /// <param name="items">数组大小不够 扩充大小 数组重新创建 改变其长度
        /// 默认原有基础上+10 
        /// net 官方默认 list使用1 
        /// queue 默认 空 即 4 否则 x2 如果 length &lt; array.length+4， length =array.length+4 ;否则 length =array.length*2
        /// stack 默认 空 即 4 否则 x2 </param>
        /// <param name="item">插入元素</param>
        /// <param name="comparer"><see cref="IComparer{T}"/>为null 则 不参与排序. 不为 null 前提 排序好 的数组 不然 会出现其他情况 指定位置无效(最大 则添加末尾 如果不只是添加排序后位置)</param>
        /// <param name="resize">数组大小不够 扩充大小 默认原有基础上+1</param>
        /// <returns></returns>
        public static bool Insert<T>(ref T[] items, T item, IComparer<T> comparer, int resize = 10)
        {
            return Insert(ref items, items.Length,  item, comparer, resize);
        }

        /// <summary> 根据索引添加 例如：0 第一个记录 -1 最后一个记录 不符合条件不做任何操作</summary>
        /// <param name="items">数组大小不够 扩充大小 数组重新创建 改变其长度
        /// 默认原有基础上+10 
        /// net 官方默认 list使用1 
        /// queue 默认 空 即 4 否则 x2 如果 length &lt; array.length+4， length =array.length+4 ;否则 length =array.length*2
        /// stack 默认 空 即 4 否则 x2 </param>
        /// <param name="index">插入位置索引 comparer不为 null 则index无效</param>
        /// <param name="item">插入元素</param>
        /// <param name="comparer"><see cref="IComparer{T}"/>为null 则 不参与排序. 不为 null 前提 排序好 的数组 不然 会出现其他情况 指定位置无效(最大 则添加末尾 如果不只是添加排序后位置)</param>
        /// <param name="resize">数组大小不够 扩充大小 默认原有基础上+1</param>
        /// <returns></returns>
        public static bool Insert<T>(ref T[] items, int index, T item, IComparer<T> comparer, int resize = 10)
        {
            int count = items.Length;
            return Insert(ref items, index, ref count, item, comparer, resize);
        }

        /// <summary> 根据索引添加 例如：0 第一个记录 -1 最后一个记录 不符合条件不做任何操作</summary>
        /// <param name="items">数组大小不够 扩充大小 数组重新创建 改变其长度
        /// 默认原有基础上+10 
        /// net 官方默认 list使用1 
        /// queue 默认 空 即 4 否则 x2 如果 length &lt; array.length+4， length =array.length+4 ;否则 length =array.length*2
        /// stack 默认 空 即 4 否则 x2 </param>
        /// <param name="index">插入位置索引 comparer不为 null 则index无效</param>
        /// <param name="count">默认数组实际数量 添加成功则自增1</param>
        /// <param name="item">插入元素</param>
        /// <param name="comparer"><see cref="IComparer{T}"/>为null 则 不参与排序. 不为 null 前提 排序好 的数组 不然 会出现其他情况 指定位置无效(最大 则添加末尾 如果不只是添加排序后位置)</param>
       /// <param name="resize">数组大小不够 扩充大小 默认原有基础上+1</param>
        /// <returns></returns>
        public static bool Insert<T>(ref T[] items, int index, ref int count, T item, IComparer<T> comparer=null, int resize = 10)
        {
            return Insert(ref items, index, ref count, item, comparer, null , resize);
        }

        /// <summary> 根据索引添加 例如：0 第一个记录 -1 最后一个记录 不符合条件不做任何操作</summary>
        /// <param name="items">数组大小不够 扩充大小 数组重新创建 改变其长度
        /// 默认原有基础上+10 
        /// net 官方默认 list使用1 
        /// queue 默认 空 即 4 否则 x2 如果 length &lt; array.length+4， length =array.length+4 ;否则 length =array.length*2
        /// stack 默认 空 即 4 否则 x2 </param>
        /// <param name="index">插入位置索引 comparer不为 null 则index无效</param>
        /// <param name="count">默认数组实际数量 添加成功则自增1</param>
        /// <param name="item">插入元素</param>
        /// <param name="comparer"><see cref="IComparer{T}"/>为null 则 不参与排序. 不为 null 前提 排序好 的数组 不然 会出现其他情况 指定位置无效(最大 则添加末尾 如果不只是添加排序后位置)</param>
        /// <param name="equalityComparer"><see cref="IEqualityComparer{T}"/> 为null 不参与 去重.  不为 null 前提 去重好 的数组 不然 会出现其他情况</param>
        /// <param name="resize">数组大小不够 扩充大小 默认原有基础上+1</param>
        public static bool Insert<T>(ref T[] items, int index, ref int count, T item, IComparer<T> comparer, IEqualityComparer<T> equalityComparer , int resize = 10)
        {
            return Insert(ref items, index, ref count, item, comparer, equalityComparer, ref EmptyIndex, resize, -1);
        }

        /// <summary>
        /// 集合 是否 为 Null
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <returns></returns>
        public static bool IsEmpty<T>(T[] items)
        {
            return items == null || items.Length == 0;
        }

        /// <summary>
        /// 集合 是否 为 Null
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <returns></returns>
        public static bool IsEmpty<T>(ICollection<T> items)
        {
            return items == null || items.Count == 0;
        }

        /// <summary>
        /// 集合 是否 为 Null
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public static bool IsEmpty(ICollection items)
        {
            return items == null || items.Count == 0;
        }


        /// <summary> 根据索引添加 例如：0 第一个记录 -1 最后一个记录 不符合条件不做任何操作</summary>
        /// <param name="items">数组大小不够 扩充大小 数组重新创建 改变其长度
        /// 默认原有基础上+10 
        /// net 官方默认 list使用1 
        /// queue 默认 空 即 4 否则 x2 如果 length &lt; array.length+4， length =array.length+4 ;否则 length =array.length*2
        /// stack 默认 空 即 4 否则 x2 </param>
        /// <param name="index">插入位置索引 comparer不为 null 则index无效</param>
        /// <param name="count">默认数组实际数量 添加成功则自增1</param>
        /// <param name="item">插入元素</param>
        /// <param name="comparer"><see cref="IComparer{T}"/>为null 则 不参与排序. 不为 null 前提 排序好 的数组 不然 会出现其他情况 指定位置无效(最大 则添加末尾 如果不只是添加排序后位置)</param>
        /// <param name="equalityComparer"><see cref="IEqualityComparer{T}"/> 为null 不参与 去重.  不为 null 前提 去重好 的数组 不然 会出现其他情况</param>
        /// <param name="indexs">
        /// tail= -1 说明 队列 没有参与运算 i=0 否则 i=tail indexs 实际数组所在位置
        /// </param>
        /// <example>
        /// example: var a=[1,2,3]; a.dequeue(); [0,2,3] [2,3]  a.dequeue(); [0,0,3] [3]  a.insert(0,2); [2,0,3]  [2,3] //这一样 断线了 怎么 2 0 3 获取 2 3  只能存储实际数组索引
        /// </example>
        /// <param name="resize">数组大小不够 扩充大小 默认原有基础上+1</param>
        /// <param name="tail"> -1 说明 队列 没有参与运算 i=0 否则 i=tail</param>
        public static bool Insert<T>(ref T[] items, int index, ref int count, T item, IComparer<T> comparer, IEqualityComparer<T> equalityComparer, ref int[] indexs, int resize = 10, int tail = -1)
        {

            int i = index < 0 ? (index + count) : index;//实际插入 索引位置 (数组移动浪费时间)
            // i = tail == -1 ? i : tail + i;//队列不影响 如果数组 实际 数量 减少 ,值为 默认值 去指定位置更新 造成了 数组 断 了。如何解决 了 ? 只能存储 数组的索引 ,
            //这样会 加大 空间 和 时间的浪费 多了 几步
            //example: var a=[1,2,3]; a.dequeue(); [0,2,3] [2,3]  a.dequeue(); [0,0,3] [3]  a.insert(0,2); [2,0,3]  [2,3] //这一样 断线了 怎么 2 0 3 获取 2 3  只能存储实际数组索引
            // 实现 去重 
            if (equalityComparer != null)
            {
                //先过滤 是否 有重复 没有 再执行下面的
                if (Exists(items, item, equalityComparer, count, indexs, tail))
                {
                    return false;//hashset 实现 去重 
                }
            }
            //比较 排序 插入指定位置无效 
            if (comparer != null)
            {
                //sortedset
                int arrayIndex = 0;// FindIndex(items, item, count, comparer, ref indexs, tail);//判断是否存在
                bool Compare(T x, T y)
                {
                    return comparer.Compare(x, y) >0;
                }
                arrayIndex = FindIndex(items, item, count, Compare,  indexs, tail);
                if (arrayIndex != -1)
                {
                    i = arrayIndex;//执行该逻辑 插入位置改变
                }
                else
                {
                    i = count;//执行该逻辑
                }
            }
            //数组扩展大小 +10
            if (count == items.Length)
            {
                //要么 list 实现 扩充, 要么 实现  list 和 queue混合使用 扩充
                Resize(ref items, items.Length + resize, indexs, tail);
            }
            if (i >= 0 && i < count)
            { 
                //[1,2,3] 0,0 [0,1,2,3] 
                for (int j = count; j > i; j--)
                {
                    items[j] = items[j - 1];
                }
                items[i] = item;
                ++count;
                //更新 队列 时数据 位置 目前 insert 该方法 list 实现 (queue混合使用 有些数据位置 不可控) 
                if (tail != -1)
                {
                    Resize(ref indexs, indexs.Length + 1);//list 扩充
                    indexs[count - 1] = i;
                }
                return true;
            }
            else if (i == count)
            {
                items[count] = item;
                ++count;
                //更新 队列 时数据 位置 目前 insert 该方法 list 实现 (queue混合使用 有些数据位置 不可控) 
                if (tail != -1)
                {
                    Resize(ref indexs, indexs.Length + 1);//list 扩充
                    indexs[count - 1] = count - 1;
                }
                return true;
            }
            else
            {
                //do nothing
                return false;
            }
        }

        /// <summary>
        /// 原有基础扩充length 默认10 net 官方默认 list使用1 queue 默认 空 即 4 否则 x 2
        /// 参与 数组 扩充(不是实际数组)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items">数组</param>
        /// <param name="length">新数组大小</param>
        /// <param name="tail"> -1 说明 队列 没参与  先进先出 数据 移除即清空(不删除)</param>
        /// <param name="indexs">
        /// tail= -1 说明 队列 没有参与运算 i=0 否则 i=tail indexs 实际数组所在位置
        /// </param>
        /// <example>
        /// example: var a=[1,2,3]; a.dequeue(); [0,2,3] [2,3]  a.dequeue(); [0,0,3] [3]  a.insert(0,2); [2,0,3]  [2,3] //这一样 断线了 怎么 2 0 3 获取 2 3  只能存储实际数组索引
        /// </example>
        public static void Resize<T>(ref T[] items, int length, int[] indexs = null, int tail = -1)
        {
            if (tail == -1)
            {
                //list 使用
                T[] temps = new T[length];
                for (int i = 0; i < items.Length; i++)
                {
                    temps[i] = items[i];
                }
                items = temps;
            }
            else
            {
                //list 和 queue混合使用 ,根据数组索引更新数据
                T[] temps = new T[length];
                for (int i = 0; i < indexs.Length; i++)
                {
                    int index = indexs[i];
                    indexs[i] = i; //index 实际位置以改变 更新
                    temps[i] = items[index];
                }
                items = temps;
            }
        }
        /// <summary>
        /// 参与 实际数组 去重 比较
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="item"></param>
        /// <param name="length">-1: items.Length</param>
        /// <param name="comparer"></param>
        /// <param name="indexs">
        /// tail= -1 说明 队列 没有参与运算 i=0 否则 i=tail indexs 实际数组所在位置
        /// </param>
        /// <example>
        /// example: var a=[1,2,3]; a.dequeue(); [0,2,3] [2,3]  a.dequeue(); [0,0,3] [3]  a.insert(0,2); [2,0,3]  [2,3] //这一样 断线了 怎么 2 0 3 获取 2 3  只能存储实际数组索引
        /// </example>
        /// <param name="tail"></param>
        /// <returns></returns>
        public static bool Exists<T>(T[] items, T item,  IEqualityComparer<T> comparer= null,int length=-1,  int[] indexs=null, int tail = -1)
        {
            comparer = comparer ?? EqualityComparer<T>.Default;
            bool Compare(T x, T y)
            {
                return comparer.Equals(x, y);
            }
            return Exists(items, item, Compare, length,  indexs, tail);
        }


        /// <summary>
        /// 参与 实际数组 去重 比较
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="item"></param>
        /// <param name="length">-1: items.Length</param>
        /// <param name="comparer"></param>
        /// <param name="indexs">
        /// tail= -1 说明 队列 没有参与运算 i=0 否则 i=tail indexs 实际数组所在位置
        /// </param>
        /// <example>
        /// example: var a=[1,2,3]; a.dequeue(); [0,2,3] [2,3]  a.dequeue(); [0,0,3] [3]  a.insert(0,2); [2,0,3]  [2,3] //这一样 断线了 怎么 2 0 3 获取 2 3  只能存储实际数组索引
        /// </example>
        /// <param name="tail"></param>
        /// <returns></returns>
        public static bool Exists<T>(T[] items, T item,  IComparer<T> comparer=null,int length=-1, int[] indexs=null, int tail = -1)
        {
            comparer = comparer ?? Comparer<T>.Default;
            bool Compare(T x,T y)
            {
                return comparer.Compare(x, y) == 0;
            }
            return Exists(items,item,Compare, length,  indexs,tail);
        }


        /// <summary>
        /// 参与 实际数组 去重 比较
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="item"></param>
        /// <param name="length">-1: items.Length</param>
        /// <param name="comparer"></param>
        /// <param name="indexs">
        /// tail= -1 说明 队列 没有参与运算 i=0 否则 i=tail indexs 实际数组所在位置
        /// </param>
        /// <example>
        /// example: var a=[1,2,3]; a.dequeue(); [0,2,3] [2,3]  a.dequeue(); [0,0,3] [3]  a.insert(0,2); [2,0,3]  [2,3] //这一样 断线了 怎么 2 0 3 获取 2 3  只能存储实际数组索引
        /// </example>
        /// <param name="tail"></param>
        /// <returns></returns>
        public static bool Exists<T>(T[] items, T item, Func<T,T,bool> comparer, int length=-1, int[] indexs=null, int tail = -1)
        {
            length=length==-1?items.Length:length;
            if (tail == -1)
            {
                int i = 0;
                for (; i < length; i++)
                {
                    if (comparer(items[i], item))
                    {
                        return true;
                    }
                }
                return false;
            }
            else
            {
                int i = 0;
                for (; i < indexs.Length; i++)
                {
                    int index = indexs[i];
                    if (comparer(items[index], item))
                    {
                        return true;
                    }
                }
                return false;
            }
        }
    }

    /// <summary>
    /// 包装参数
    /// 不然参数过多累死人
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CollectionEntry<T>
    {
        /// <summary>
        /// 数组大小不够 扩充大小 数组重新创建 改变其长度 默认原有基础上+10 net 官方默认 list使用1 queue 默认 空 即 4 否则 x 2
        /// </summary>
        public T[] Items { get; set; }
        /// <summary>
        /// 元素添加指定数组索引位置 默认 -1 ：不做任何操作
        /// </summary>
        public int Index { get; set; } = -1;
        /// <summary>
        /// 数组实际数量(不是数组数量) 添加成功则自增1 
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// hashset 实现 去重 重复的不做任何 操作 否则更新
        /// </summary>
        public IEqualityComparer<T> EqualityComparer { get; set; }
        /// <summary>
        /// 数组大小不够 扩充大小 数组重新创建 改变其长度 默认原有基础上(Items.Length) +10 net 官方默认 list使用 +1 queue 默认 空 即 4 否则 x 2
        /// </summary>

        public int Capacity { get; set; }
        /// <summary>
        /// 暂时无效 -1 说明 队列 没参与 其他说明参与 了 队列先进先出 数组不会更新 (只会更新数据)
        /// </summary>

        public int Tail { get; set; } = -1;

    }

}
