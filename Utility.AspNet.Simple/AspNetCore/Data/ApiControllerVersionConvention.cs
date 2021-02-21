#if NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0 || NETCOREAPP3_1 || NET5_0 || NETSTANDARD2_0 || NETSTANDARD2_1
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Utility.AspNetCore.Data
{
    public class ApiControllerVersionConvention : IControllerModelConvention
    {
        public void Apply(ControllerModel controller)
        {
            if (!(controller.ControllerType.IsDefined(typeof(ApiVersionAttribute)) || controller.ControllerType.IsDefined(typeof(ApiVersionNeutralAttribute))))
            {
                if (controller.Attributes is List<object>
                    attributes)
                {
                    attributes.Add(new ApiVersionNeutralAttribute());
                }
            }
        }
    }
}
#endif