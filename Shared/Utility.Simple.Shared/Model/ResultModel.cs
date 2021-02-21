using System;
using System.Collections.Generic;

namespace Utility.Model
{
    /// <summary>结果模型 </summary>
    public interface IResultModel<T>
    {
        /// <summary>数据集合 </summary>
        List<T> Data { get; set; }

        /// <summary>记录模型 </summary>
        PageModel Result { get; set; }
    }
    /// <summary>结果模型 </summary>
#if !(NETCOREAPP1_0 || NETCOREAPP1_1 || NETCOREAPP1_2 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
    [Serializable]//remote(不支持泛型) webservice(接口不支持) 必须要 wcf 没影响
#endif
    public class ResultModel<T>:IResultModel<T>
    {
        /// <summary>
        /// 
        /// </summary>
        public ResultModel()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="datas"></param>
        /// <param name="count"></param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        public ResultModel(List<T> datas,int count,int page,int size)
        {
            Data = datas;
            Result = new PageModel() { Records = count, Total = count == 0 ? 0 : count % size == 0 ? count / size : (count / size + 1), Size = size, Page = page };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tuple"></param>
        /// <param name="page"></param>
        /// <param name="size"></param>

        public ResultModel(Tuple<List<T>,int> tuple, int page, int size)
        {
            Data = tuple.Item1;
            Result = new PageModel() { Records = tuple.Item2, Total = tuple.Item2 == 0 ? 0 : tuple.Item2 % size == 0 ? tuple.Item2 / size : (tuple.Item2 / size + 1), Size = size, Page = page };
        }
        /// <summary>数据集合 </summary>
        public List<T> Data { get; set; }

        /// <summary>记录模型 </summary>
        public PageModel Result { get; set; }
    }
}
