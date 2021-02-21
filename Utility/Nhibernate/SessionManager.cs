#if !(NET10 || NET11 || NET20 || NET30 || NET35 || NETCOREAPP1_0 || NETCOREAPP1_1 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
//#if NET40 ||NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48  || NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0 || NETCOREAPP3_1 || NET5_0 || NETSTANDARD2_0 || NETSTANDARD2_1
using NHibernate;
using NHibernate.Context;
using System;

namespace Utility.Nhibernate
{
    /// <summary>
    /// 
    /// </summary>
    public  class SessionManager
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sessionFactory"></param>
        public static void BindSession(ISessionFactory sessionFactory)
        {
            if (!CurrentSessionContext.HasBind(sessionFactory))
            {
                CurrentSessionContext.Bind(sessionFactory.OpenSession());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sessionFactory"></param>
        /// <returns></returns>
        public static ISession GetCurrentSession(ISessionFactory sessionFactory)
        {
           // BindSession(sessionFactory);
            ISession session= sessionFactory.GetCurrentSession();
            return session;
        }

        /// <summary>
        /// 
        /// </summary>
        public interface ISessionManager
        {
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            ISession Get();

            /// <summary>
            /// 
            /// </summary>
            /// <param name="session"></param>
            void Set(ISession session);
        }

        /// <summary>
        /// 
        /// </summary>
        public class WebNHSession : ISessionManager
        {
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public ISession Get()
            {
                throw new NotImplementedException();
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="session"></param>
            public void Set(ISession session)
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public class NHSession: ISessionManager
        {
            [ThreadStatic]
            private static ISession _threadSession;


            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public ISession Get()
            {
                if (_threadSession != null)
                {
                    if (_threadSession.IsConnected)
                    {
                        _threadSession.Reconnect();
                    }
                }
                return _threadSession;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="session"></param>
            public void Set(ISession session)
            {
                if (_threadSession.IsConnected)
                {
                    session.Disconnect();
                }
                _threadSession = session;
            }
        }
    }
}
#endif