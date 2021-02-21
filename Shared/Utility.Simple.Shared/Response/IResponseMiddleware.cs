namespace Utility.Response
{
    /// <summary>
    /// <see cref="IResponseApi"/> Middleware
    /// </summary>
    public interface IResponseMiddleware
    {
        /// <summary>
        /// <see cref="IResponseApi"/> Middleware impltement
        /// </summary>
        /// <param name="response"></param>
        /// <returns>operator succcess</returns>
        bool Exected(IResponseApi response);
    }
}
