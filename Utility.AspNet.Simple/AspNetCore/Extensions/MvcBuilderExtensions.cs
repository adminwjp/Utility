#if NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0 || NETCOREAPP3_1 || NET5_0 || NETSTANDARD2_0 || NETSTANDARD2_1
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;
using Utility.Json;

namespace Utility.AspNetCore.Extensions
{
    /// <summary>
    /// MvcBuilder 扩展类
    /// </summary>
    public static class MvcBuilderExtensions
    {
        /// <summary>
        /// json字符串大小写原样输出
        /// </summary>
        public static readonly IContractResolver ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver();

        /// <summary>
        /// json 转换 默认 ABC a_b_c 忽略循环 时间 格式 yyyy-MM-dd HH:mm:ss
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="contractResolver">默认为null</param>
        /// <returns></returns>
        public static IMvcBuilder AddJson(this IMvcBuilder builder, IContractResolver contractResolver=null )
        {
#if !(NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2)
            //全局配置Json序列化处理 方案1
            return builder.AddNewtonsoftJson(options =>
            {
               // options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                //忽略循环引用
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                //使用 AbC ab_c
                options.SerializerSettings.ContractResolver = contractResolver??JsonContractResolver.ObjectResolverJson;
                //设置时间格式
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            }
          )
          .AddXmlSerializerFormatters();
#else
            return builder;
#endif
            //全局配置Json序列化处理  方案2
            //  .AddJsonOptions(options =>
            //  {
            //      options.JsonSerializerOptions.MaxDepth = 10;
            //       options.JsonSerializerOptions.PropertyNamingPolicy = JsonPropertyNamingPolicy.CamelCase;

            //  }
            //)

        }
    }
}
#endif