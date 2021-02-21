using System;

namespace Utility.Response
{
    /// <summary>
    ///IResponseApi:common api interface 
    /// </summary>
    public interface IResponseApi
    {
        /// <summary> 
        /// wether operator succcess
        /// </summary>
        bool Success { get; set; }

        /// <summary> 
        /// operator information
        /// </summary>
        string Message { get; set; }

        /// <summary>
        ///  code
        /// </summary>

        int Code { get; set; }
    }

    /// <summary>
    ///BaseResponseApi:common api .
    ///<see cref="IResponseApi"/> interface  implement
    /// </summary>
#if !(NETCOREAPP1_0 || NETCOREAPP1_1 || NETCOREAPP1_2 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
    [Serializable]//remote webservice(interfacer not support) must need, wcf not need
#endif
    public abstract class BaseResponseApi:IResponseApi
    {
        /// <summary> 
        /// wether operator succcess
        /// </summary>
        public bool Success { get; set; }

        /// <summary> 
        /// operator information
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        ///  code
        /// </summary>

        public  int Code { get; set; }
    }
}
