//net45 - net48 netcoreapp2.0 - net5.0 netstandard2.0 - netstandard2.1
#if !(NET10 || NET11 || NET20 || NET30 || NET35 || NET40 || NETCOREAPP1_0 || NETCOREAPP1_1 || NETCOREAPP1_2 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
//#if NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48 || NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0 || NETCOREAPP3_1 || NET5_0 || NETSTANDARD2_0 || NETSTANDARD2_1
using System;

namespace Utility.Zk
{
    using org.apache.zookeeper;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using static org.apache.zookeeper.Watcher.Event;
    using static org.apache.zookeeper.ZooDefs;

    /**
Will not attempt to authenticate using SASL (unknown error)
127.0.0.1       localhost
windows/System32/drivers/etc/hosts
*/

    ///<summary>
    ///老是出现 这种情况  引发类型为“org.apache.zookeeper.KeeperException+ConnectionLossException”的异常。
    /// </summary>
    public class ZooKeeperHelper:IDisposable
    {    
        
        /// <summary> 定义zookeeper实例 </summary>
        private  ZooKeeper _zooKeeper = null;
        /// <summary> 对象锁 </summary>
        private static readonly object _obj = new object();
        /// <summary>
        /// 节点监听事件
        /// </summary>
        private readonly static Watcher emptyNodesWatch = new DefaultNodesWatch();
        private string url;//地址
        private Watcher watcher;//观察节点
        /// <summary>
        /// 初始化地址 
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="watcher">观察节点</param>
        public ZooKeeperHelper(string url= "127.0.0.1:2181", Watcher watcher=null)
        {
            this.url = url;
            this.watcher = watcher ?? emptyNodesWatch;
            this._zooKeeper = new ZooKeeper(url, 10000, watcher);
           
        }
        /// <summary>
        /// 关闭
        /// </summary>
        public  virtual void Close()
        {
            this._zooKeeper.closeAsync();
        }

        /// <summary>
        /// 连接状态
        /// </summary>
        void CheckConenctopnState()
        {
            var state = this._zooKeeper.getState();
            if (state != ZooKeeper.States.CONNECTED)
            {
                Close();
                //老是出现 这种情况  引发类型为“org.apache.zookeeper.KeeperException + ConnectionLossException”的异常。
                //内部 才能使用 需要使用反射 重连 
                this._zooKeeper = new ZooKeeper(url, 10000, watcher);
            }
        }

        /// <summary>
        /// 创建节点
        /// </summary>
        /// <param name="data">格式 /a/b</param>
        /// <returns></returns>
        public  virtual bool Create(string data)
        {
            if (Exists(data))
            {
                return true;
            }
           CheckConenctopnState();
            var str=this._zooKeeper.createAsync(data, new byte[0], Ids.OPEN_ACL_UNSAFE, CreateMode.PERSISTENT).Result;
            //data==str
            Console.WriteLine("create {0}--{1} ",data,str);
            return data == str;
        }

        /// <summary>
        /// 获取节点数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public virtual string GetData(string data)
        {
            CheckConenctopnState();
            var str = this._zooKeeper.getDataAsync(data, null).Result.Data;
            Console.WriteLine("get data {0}--{1}", data, str);
            return str == null?string.Empty:Encoding.UTF8.GetString(str);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public virtual bool Delete(string data)
        {
            if (!Exists(data))
            {
                return false;
            }
            CheckConenctopnState();
            var str = this._zooKeeper.deleteAsync(data);
            Console.WriteLine("delete {0}--{1}", data, str);
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        public virtual void Children(string node)
        {
            CheckConenctopnState();
            var stat = this._zooKeeper.existsAsync(node, true);
            this._zooKeeper.getChildrenAsync(node, true);
        }

       

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public virtual bool Exists(string data)
        {
            CheckConenctopnState();
            var stat = this._zooKeeper.existsAsync(data, false).Result;
            Console.WriteLine("exists {0}--{1}", data, stat);
            return stat != null;
        }
        static ZooKeeperHelper zooKeeperHelper;
        /// <summary>
        /// 初始化zookeeper实例
        /// </summary>
        /// <returns></returns>
        public   static ZooKeeperHelper Instance
        {
            get
            {
                if (zooKeeperHelper == null)
                {
                    lock (_obj)
                    {
                        if (zooKeeperHelper == null)
                        {
                            zooKeeperHelper = new ZooKeeperHelper();
                            return zooKeeperHelper;
                        }
                    }
                }
                return zooKeeperHelper;
            }
        }
        /// <summary>
        /// 更新服务节点
        /// </summary>
        /// <param name="node"></param>
        /// <param name="json"></param>
        public virtual bool ServerNodeUpdate(string node,string json)
        {
            CheckConenctopnState();
            var stat = _zooKeeper.existsAsync(node, true).Result;
            if (stat != null)
            {
                CheckConenctopnState();
                _zooKeeper.setDataAsync(node, Encoding.UTF8.GetBytes(json), -1);
                return true;
            }
            else
            {
                throw new Exception("检测服务节点失败");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Dispose()
        {
            this.Close();
        }
    }

    /// <summary>
    /// 节点监听
    /// </summary>
    public class DefaultNodesWatch : Watcher
    {
        /// <summary>
        /// 节点监听事件
        /// </summary>
        /// <param name="event"></param>
        public override Task process(WatchedEvent @event)
        {

            return Task.Factory.StartNew(() => {
                if (KeeperState.SyncConnected == @event.getState())
                {
                  
                }

                if (@event.get_Type() == EventType.NodeDeleted)
                {
                 
                }
                string path = @event.getPath();
                if (!string.IsNullOrEmpty(path))
                {
                    Thread.Sleep(3000);
                }
            });
        }
    }
}
#endif