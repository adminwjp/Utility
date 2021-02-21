using System;
using System.Collections.Generic;

namespace Utility.Application.Services.Dtos
{
    /// <summary>IResultDto interface:result information </summary>
    public interface IResultDto<T>
    {
        /// <summary>data collection </summary>
        IList<T> Data { get; set; }

        /// <summary>record result information </summary>
        IPageResultDto Result { get; set; }
    }
    /// <summary>
    /// ResultDto:IResultDto interface implement.
    /// result information .
    ///  remote not support generic type .
    /// </summary>
#if !(NETCOREAPP1_0 || NETCOREAPP1_1 || NETCOREAPP1_2 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
    [Serializable]//remote webservice(interfacer not support) must need, wcf not need
#endif
    public class ResultDto<T>: IResultDto<T>
    {
        /// <summary>
        /// no param constractor
        /// </summary>
        public ResultDto()
        {

        }

        /// <summary>
        /// have param constractor
        /// </summary>
        /// <param name="data">data collection</param>
        /// <param name="page">number of pages</param>
        /// <param name="size">record pre page</param>
        /// <param name="record">total record</param>
        public ResultDto(List<T> data,int page,int size,int record)
        {
            Data = data;
            Result = new PageResultDto() { Records =record, Total = record == 0 ? 0 : record % size == 0 ? record / size : (record / size + 1), Size = size, Page = page };

        }

        /// <summary>
        /// have param constractor
        /// </summary>
        /// <param name="tuple">data collection and total record</param>
        /// <param name="page">number of pages</param>
        /// <param name="size">record pre page</param>
        public ResultDto(Tuple<List<T>, int> tuple, int page, int size):this(tuple.Item1,page,size,tuple.Item2)
        {
          
        }

        /// <summary>data collection </summary>
        public List<T> Data { get; set; }

        /// <summary>record result information </summary>
        public PageResultDto Result { get; set; }



        /// <summary>data collection </summary>
        IList<T> IResultDto<T>.Data { get => Data; set => Data=value==null?null:(value is List<T> ? (List<T>)value : new List<T>(value)); }

        /// <summary>record result information </summary>
        IPageResultDto IResultDto<T>.Result { get => Result; set => Result=value==null?null:(value is PageResultDto?(PageResultDto)value:new PageResultDto(value.Page,value.Size,value.Total,value.Records)); }
    }
}
