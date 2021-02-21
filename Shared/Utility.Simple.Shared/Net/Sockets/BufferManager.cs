#if !(NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2)
using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace Utility.Net.Sockets
{
    /// <summary>
    ///这个类创建一个大缓冲区并分配给SocketAsyncEventArgs对象以用于插座I/O操作。这使得buffers可以很容易地被重用和防止正在碎片化堆内存。BufferManager类上公开的操作不是线程安全的。
    /// </summary>
    internal class BufferManager
    {
        private int _numBytes;// 缓冲池控制的字节总数
        private byte[] _buffer;//the underlying byte array maintained by the Buffer Manager
        private Utility.Collections.Array<int> _freeIndexPool;//缓冲区管理器维护的底层字节数组
        private int _currentIndex;
        private int _bufferSize;
        public BufferManager(int totalBytes, int bufferSize)
        {
            this._numBytes = totalBytes;
            this._currentIndex = 0;
            this._bufferSize = bufferSize;
            this._freeIndexPool = new Utility.Collections.Array<int>() ;
        }

        /// <summary>
        /// 分配缓冲池使用的缓冲区空间
        /// </summary>
        public virtual void InitBuffer()
        {
            // 创建一个大缓冲区并将其分割 输出到每个SocketAsyncEventArg对象
            this._buffer = new byte[_numBytes];
        }

        /// <summary>
        /// 将缓冲池中的缓冲区分配给 指定的SocketAsyncEventArgs对象
        /// </summary>
        /// <param name="args"></param>
        /// <returns> 如果缓冲区设置成功 true,否则为false</returns>
        public virtual bool SetBuffer(SocketAsyncEventArgs args)
        {
            if (_freeIndexPool.Count > 0)
            {
                args.SetBuffer(this._buffer, this._freeIndexPool.Pop(), this._bufferSize);
            }
            else
            {
                if ((this._numBytes - this._bufferSize) < this._currentIndex)
                {
                    return false;
                }
                args.SetBuffer(this._buffer, this._currentIndex, this._bufferSize);
                this._currentIndex += this._bufferSize;
            }
            return true;
        }

        /// <summary>
        /// <para>从SocketAsyncEventArg对象中删除缓冲区。</para>
        /// <para>这会将缓冲区释放回缓冲池</para>
        /// </summary>
        /// <param name="args"></param>
        public virtual void FreeBuffer(SocketAsyncEventArgs args)
        {
            this._freeIndexPool.Push(args.Offset);
            args.SetBuffer(null, 0, 0);
        }
    }
}
#endif