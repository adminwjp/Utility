namespace Utility.Response
{
    /// <summary>
    ///  <see cref="IResponseApi"/> Middleware implement
    /// </summary>
    public class DefaultResponseMiddleware : IResponseMiddleware
    {
        /// <summary>
        /// <see cref="IResponseApi"/> Middleware impltement
        /// </summary>
        /// <param name="response"></param>
        /// <returns>operator succcess</returns>
        bool IResponseMiddleware.Exected(IResponseApi response)
        {
            if (response.Code.ToString().StartsWith("2"))
                response.Success = true;
            else
            {
                response.Success = false;
            }
            return false;
        }
    }
}
