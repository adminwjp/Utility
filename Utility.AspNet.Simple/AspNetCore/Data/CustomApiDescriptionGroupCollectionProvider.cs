#if NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0 || NETCOREAPP3_1 || NET5_0 || NETSTANDARD2_0 || NETSTANDARD2_1
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Utility.AspNetCore.Data
{
    public class CustomApiDescriptionGroupCollectionProvider : IApiDescriptionGroupCollectionProvider
    {
        private readonly IActionDescriptorCollectionProvider _actionDescriptorCollectionProvider;

        private readonly IApiDescriptionProvider[] _apiDescriptionProviders;

        private ApiDescriptionGroupCollection _apiDescriptionGroups;

        //
        public ApiDescriptionGroupCollection ApiDescriptionGroups
        {
            get
            {
                ActionDescriptorCollection actionDescriptors = _actionDescriptorCollectionProvider.ActionDescriptors;
                if (_apiDescriptionGroups == null || _apiDescriptionGroups.Version != actionDescriptors.Version)
                {
                    _apiDescriptionGroups = GetCollection(actionDescriptors);
                }
                return _apiDescriptionGroups;
            }
        }

        //
        // 摘要:
        //     /// Creates a new instance of Microsoft.AspNetCore.Mvc.ApiExplorer.ApiDescriptionGroupCollectionProvider.
        //     ///
        //
        // 参数:
        //   actionDescriptorCollectionProvider:
        //     /// The Microsoft.AspNetCore.Mvc.Infrastructure.IActionDescriptorCollectionProvider.
        //     ///
        //
        //   apiDescriptionProviders:
        //     /// The System.Collections.Generic.IEnumerable`1. ///
        public CustomApiDescriptionGroupCollectionProvider(IActionDescriptorCollectionProvider actionDescriptorCollectionProvider, IEnumerable<IApiDescriptionProvider> apiDescriptionProviders)
        {
            //new DefaultActionDescriptorCollectionProvider();
            _actionDescriptorCollectionProvider = actionDescriptorCollectionProvider;
            _apiDescriptionProviders = (from item in apiDescriptionProviders
                                        orderby item.Order
                                        select item).ToArray();
        }

        private ApiDescriptionGroupCollection GetCollection(ActionDescriptorCollection actionDescriptors)
        {
            ApiDescriptionProviderContext apiDescriptionProviderContext = new ApiDescriptionProviderContext(actionDescriptors.Items);
            IApiDescriptionProvider[] apiDescriptionProviders = _apiDescriptionProviders;
            for (int i = 0; i < apiDescriptionProviders.Length; i++)
            {
                //new DefaultApiDescriptionProvider();
                if (apiDescriptionProviders[i].GetType().FullName == "Microsoft.AspNetCore.Mvc.ApiExplorer.DefaultApiDescriptionProvider")
                    continue;

                apiDescriptionProviders[i].OnProvidersExecuting(apiDescriptionProviderContext);
            }
            for (int num = _apiDescriptionProviders.Length - 1; num >= 0; num--)
            {
                _apiDescriptionProviders[num].OnProvidersExecuted(apiDescriptionProviderContext);
            }
            return new ApiDescriptionGroupCollection((from d in apiDescriptionProviderContext.Results
                                                      group d by d.GroupName into g
                                                      select new ApiDescriptionGroup(g.Key, g.ToArray())).ToArray(), actionDescriptors.Version);
        }
    }
    internal class ParameterDefaultValue
    {

        private static readonly Type _nullable = typeof(Nullable<>);

        public static bool TryGetDefaultValue(System.Reflection.ParameterInfo parameter, out object defaultValue)
        {
            bool flag = true;
            defaultValue = null;
            bool flag2;
            try
            {
                flag2 = parameter.HasDefaultValue;
            }
            catch (FormatException) when (parameter.ParameterType == typeof(DateTime))
            {
                flag2 = true;
                flag = false;
            }
            if (flag2)
            {
                if (flag)
                {
                    defaultValue = parameter.DefaultValue;
                }
                if (defaultValue == null && parameter.ParameterType.IsValueType)
                {
                    defaultValue = Activator.CreateInstance(parameter.ParameterType);
                }
                if (defaultValue != null && parameter.ParameterType.IsGenericType && parameter.ParameterType.GetGenericTypeDefinition() == _nullable)
                {
                    Type underlyingType = Nullable.GetUnderlyingType(parameter.ParameterType);
                    if (underlyingType != null && underlyingType.IsEnum)
                    {
                        defaultValue = Enum.ToObject(underlyingType, defaultValue);
                    }
                }
            }
            return flag2;
        }
    }
}
#endif