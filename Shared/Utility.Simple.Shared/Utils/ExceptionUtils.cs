using System;
using System.Collections.Generic;
using System.Text;

namespace Utility.Utils
{
    /// <summary>
    /// 
    /// </summary>
    public class ExceptionUtils
    {  
        /// <summary>
       /// check exception
       /// </summary>
       /// <param name="action"></param>
        public static void CheckException(Action action)
        {
            try
            {
                action();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// check exception
        /// </summary>
        /// <param name="action"></param>
        /// <param name="logAction"></param>
        public static void CheckException(Action action, Action<Exception> logAction)
        {
            try
            {
                action();
            }
            catch (Exception e)
            {
                logAction(e);
            }
        }
    }
}
