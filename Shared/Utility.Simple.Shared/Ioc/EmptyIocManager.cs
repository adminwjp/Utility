using System;
using System.Collections.Generic;
using System.Text;

namespace Utility.Ioc
{
    /// <summary>
    /// default ioc
    /// </summary>
    public class EmptyIocManager : IIocManager
    {
        /// <summary>
        /// not implement
        /// </summary>
        public static readonly EmptyIocManager Empty = new EmptyIocManager();

        /// <summary>
        /// 
        /// </summary>
        protected EmptyIocManager()
        {

        }

        /// <summary>
        /// add transitent
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="ImplT"></typeparam>
        public virtual  void AddTransient<T, ImplT>() where ImplT : class
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// add scope
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="ImplT"></typeparam>
        public virtual void AddScoped<T, ImplT>() where ImplT : class
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// add single instanc
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="ImplT"></typeparam>
        public virtual  void SingleInstance<T, ImplT>() where ImplT : class
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// get object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public virtual T Get<T>()
        {
            throw new NotImplementedException();
        }
    }
}
