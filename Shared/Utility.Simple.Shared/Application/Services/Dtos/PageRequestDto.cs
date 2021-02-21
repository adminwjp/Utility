using System;

namespace Utility.Application.Services.Dtos
{
    /// <summary>
    /// IPageRequestDto interface:pagination information
    /// </summary>
    public interface IPageRequestDto
    {
        /// <summary>number of pages </summary>
        int Page { get; set; }

        /// <summary>record pre page </summary>
        int Size { get; set; }
    }
    /// <summary>
    /// PageRequestDto:IPageRequestDto interface implement.
    /// pagination information.
    /// </summary>
#if !(NETCOREAPP1_0 || NETCOREAPP1_1 || NETCOREAPP1_2 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
    [Serializable]//remote webservice(interfacer not support) must need, wcf not need
#endif
    public class PageRequestDto:IPageRequestDto
    {
        /// <summary>number of pages </summary>
        public int Page { get; set; }

        /// <summary>record pre page </summary>
        public int Size { get; set; }
    }
}
