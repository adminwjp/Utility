#if !(NET10 || NET11 || NET20 || NET30 || NET35 || NET40 || NETCOREAPP1_0 || NETCOREAPP1_1 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
//#if NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48 || NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0 || NETCOREAPP3_1 || NET5_0 || NETSTANDARD2_0 || NETSTANDARD2_1
using AutoMapper;
using System;

namespace Utility.ObjectMapping
{

    /// <summary>
    ///<see cref="IObjectMapper"/>  interface  implement.
    ///be based on AutoMapper  implement
    /// </summary>
    public class AutoMapperObjectMapper:IObjectMapper
    {
        /// <summary>
        /// new AutoMapperObjectMapper();
        /// </summary>
        public static readonly AutoMapperObjectMapper Empty = new AutoMapperObjectMapper();

        private  IMapper _mapper=null;//IMapper

        /// <summary>
        /// Mapper
        /// </summary>
        public Mapper Mapper => (Mapper)_mapper;



        /// <summary>
        /// object mapping configuration
        /// </summary>
        /// <param name="configure">configuration</param>

#if NET45 || NET451 || NET452 || NET46
        public virtual void Init(Action<IMapperConfiguration> configure)
        {
            MapperConfiguration config = new MapperConfiguration(cfg => {
                configure.Invoke(cfg);
            });
            _mapper = config.CreateMapper(); 
        }
#else
        public void Init(Action<IMapperConfigurationExpression> configure)
        {
            IConfigurationProvider config = new MapperConfiguration(cfg => {
                configure.Invoke(cfg);
            });
            _mapper = config.CreateMapper();
        }
#endif
      


        /// <summary>
        /// according source entity mapping  target entity
        /// </summary>
        /// <typeparam name="SoucreEntity">source entity</typeparam>
        /// <typeparam name="TargetEntity">target entity</typeparam>
        /// <param name="source">source entity</param>
        /// <returns>return target entity</returns>
        public  virtual TargetEntity Map<SoucreEntity, TargetEntity>(SoucreEntity source)
        {
            return MapTo<TargetEntity>(source);
        }

        /// <summary>
        /// according source entity mapping  target entity
        /// </summary>
        /// <typeparam name="TargetEntity">target entity</typeparam>
        /// <param name="source">source entity</param>
        /// <returns>return target entity</returns>
        public virtual TargetEntity Map<TargetEntity>(object source)
        {
            return MapTo<TargetEntity>(source);
        }

        /// <summary>
        /// according source entity mapping  target entity
        /// </summary>
        /// <typeparam name="T">target entity</typeparam>
        /// <param name="obj">source entity</param>
        /// <returns>return target entity</returns>
        public virtual T MapTo<T>(object obj)
        {
            if (obj == null) return default(T);
            return _mapper.Map<T>(obj);
        }


        /// <summary>
        /// according source entity mapping  target entity
        /// </summary>
        /// <param name="destination"></param>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination">target entity</typeparam>
        /// <param name="obj">source entity</param>
        /// <returns>return target entity</returns>
        public virtual TDestination Map<TSource, TDestination>(TSource obj, TDestination destination)
        {
            if (obj == null||destination==null) return default(TDestination);
            return  _mapper.Map<TSource, TDestination>(obj, destination);
        }
    }
}
#endif