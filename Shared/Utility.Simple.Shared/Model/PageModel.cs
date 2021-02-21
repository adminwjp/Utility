using System;

namespace Utility.Model
{
    /// <summary>记录模型 </summary>
    public interface IPageModel
    {
        /// <summary>页数 </summary>
        int Page { get; set; }

        /// <summary>每页记录 </summary>
        int Size { get; set; }

        /// <summary>总页数 </summary>
        int Total { get; set; }

        /// <summary>总记录 </summary>
        int Records { get; set; }

    }
    /// <summary>记录模型 </summary>
#if !(NETCOREAPP1_0 || NETCOREAPP1_1 || NETCOREAPP1_2 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
    [Serializable]//remote webservice(接口不支持) 必须要 wcf 没影响
#endif
    public class PageModel: IPageModel
    {
        /// <summary>页数 </summary>
        public int Page { get; set; }

        /// <summary>每页记录 </summary>
        public int Size { get; set; }

        /// <summary>总页数 </summary>
        public int Total { get; set; }

        /// <summary>总记录 </summary>
        public int Records { get; set; }
        
    }
}
