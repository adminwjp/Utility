#if NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0 || NETCOREAPP3_1 || NET5_0 || NETSTANDARD2_0 || NETSTANDARD2_1
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Utility.AspNetCore.Filter
{
    public class EmptySwaggerOperationFilter : IOperationFilter, IDocumentFilter
    {
        public virtual void Apply(OpenApiOperation operation, OperationFilterContext context)
        {

        }

        public virtual void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {

        }
    }
}
#endif