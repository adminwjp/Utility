#if !(NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2)
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;

namespace Utility.Net.Sockets
{
    /// <summary>
    /// 表示可重用SocketAsyncEventArgs对象的集合。
    /// </summary>
    internal class SocketAsyncEventArgsPool
    {
        private readonly Utility.Collections.Array<SocketAsyncEventArgs> _pool;
        /// <summary>
        /// 将对象池初始化为指定大小 SocketAsyncEventArgs池可以容纳的对象
        /// </summary>
        /// <param name="capacity"></param>
        internal SocketAsyncEventArgsPool(int capacity)
        {
            this._pool = new Utility.Collections.Array<SocketAsyncEventArgs>(capacity);
        }
        /// <summary>
        /// 将SocketAsyncEventArg实例添加到池中
        /// </summary>
        /// <param name="item">SocketAsyncEventArgs实例</param>
        internal void Push(SocketAsyncEventArgs item)
        {
            lock(this._pool)
            {
                this._pool.Push(item);
            }
        }
        /// <summary>
        /// 池中删除SocketAsyncEventArgs实例
        /// </summary>
        /// <returns>返回从池中移除的对象</returns>
        internal SocketAsyncEventArgs Pop()
        {
            lock (this._pool)
            {
                return this._pool.Pop();
            }
        }
        /// <summary>
        /// 池中的SocketAsyncEventArgs实例数
        /// </summary>
        public int Count
        {
            get { return this._pool.Count; }
        }
    }
}
#endif