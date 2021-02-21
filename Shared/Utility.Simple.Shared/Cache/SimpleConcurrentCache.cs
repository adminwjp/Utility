#if !(NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
#if !(NET20 || NET30 || NET35 || NETSTANDARD1_0)

using System.Collections.Concurrent;


namespace Utility.Cache
{
    /// <summary>
    /// 
    /// </summary>
    public class SimpleConcurrentCache : SimpleCache, ICacheContent
    {
        //  public static readonly SimpleConcurrentCache Empty = new SimpleConcurrentCache();//prevent thread start-up

        /// <summary>
        /// no param constractor ,default new ConcurrentDictionary &lt;string,ValueEntry&gt;()
        /// </summary>
        public SimpleConcurrentCache() : base(new ConcurrentDictionary<string, ValueEntry>())
        {

        }

    }
}
#endif
#endif