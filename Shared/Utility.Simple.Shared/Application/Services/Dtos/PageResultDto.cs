using System;

namespace Utility.Application.Services.Dtos
{
    /// <summary>IPageResultDto interface :record result information </summary>
    public interface IPageResultDto
    {
        /// <summary>number of pages </summary>
        int Page { get; set; }

        /// <summary>record pre page </summary>
        int Size { get; set; }

        /// <summary>total  page </summary>
        int Total { get; set; }

        /// <summary>total records </summary>
        int Records { get; set; }

    }

    /// <summary>
    /// PageResultDto:IPageResultDto interface implement .
    /// record result information 
    /// </summary>
#if !(NETCOREAPP1_0 || NETCOREAPP1_1 || NETCOREAPP1_2 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
    [Serializable]//remote webservice(interfacer not support) must need, wcf not need
#endif
    public class PageResultDto : IPageResultDto
    {
        /// <summary>
        /// no param constractor
        /// </summary>
        public PageResultDto()
        {

        }

        /// <summary>
        ///has param constractor
        /// </summary>
        /// <param name="page">number of pages</param>
        /// <param name="size">record pre page</param>
        /// <param name="total">total page </param>
        /// <param name="records">total records</param>
        public PageResultDto(int page, int size, int total, int records)
        {
             Page = page;
            Size = size;
            Total = total;
            Records = records;
        }

        /// <summary>number of pages </summary>
        public int Page { get; set; }

        /// <summary>record pre page </summary>
        public int Size { get; set; }

        /// <summary>total page</summary>
        public int Total { get; set; }

        /// <summary>total records </summary>
        public int Records { get; set; }


    }
}
