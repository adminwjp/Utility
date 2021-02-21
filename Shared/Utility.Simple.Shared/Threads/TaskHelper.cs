#if !(NET20 || NET30 || NET35 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
using System;
using System.Threading.Tasks;
using System.Threading;


namespace Utility.Threads
{
    /// <summary>
    /// 
    /// </summary>
    public class TaskHelper
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly Task CompletedTask = new Task(() => { }, (TaskCreationOptions)16384);


        /// <summary>
        /// task wrapper 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        public static Task<T> TaskCompletionSource<T>(Func<T> func, CancellationToken cancellation = default(CancellationToken))
        {
            return  TaskCompletionSourceAsync(func,cancellation);
        }
        /// <summary>
        /// task wrapper 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        public static  Task<T> TaskCompletionSourceAsync<T>(Func<T> func, CancellationToken cancellation = default(CancellationToken))
        {
            if (cancellation.IsCancellationRequested) return  new Task<T>(()=>default(T));
            TaskCompletionSource<T> taskCompletionSource = new TaskCompletionSource<T>();
            try
            {
                taskCompletionSource.SetResult(func());
            }
            catch (System.Exception e)
            {
                taskCompletionSource.SetException(e);
            }
            return  taskCompletionSource.Task;
        }
    }
}
#endif
