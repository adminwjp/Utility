//#if NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0 || NETCOREAPP3_1 || NET5_0 || NETSTANDARD2_0 || NETSTANDARD2_1
//using Microsoft.ServiceFabric.Services.Runtime;
//using System;
//using System.Diagnostics;
//using System.Threading;
//using System.Threading.Tasks;

//namespace OcelotApplicationService
//{
//    internal static class Program
//    {
//        /// <summary>
//        /// This is the entry point of the service host process.
//        /// </summary>
//        private static void Main()
//        {
//            try
//            {
//                // The ServiceManifest.XML file defines one or more service type names.
//                // Registering a service maps a service type name to a .NET type.
//                // When Service Fabric creates an instance of this service type,
//                // an instance of the class is created in this host process.

//                ServiceRuntime.RegisterServiceAsync("OcelotApplicationServiceType",
//                    context => new ApiGateway(context)).GetAwaiter().GetResult();

//                ServiceEventSource.Current.ServiceTypeRegistered(Process.GetCurrentProcess().Id, typeof(ApiGateway).Name);

//                // Prevents this host process from terminating so services keeps running. 
//                Thread.Sleep(Timeout.Infinite);
//            }
//            catch (Exception e)
//            {
//                ServiceEventSource.Current.ServiceHostInitializationFailed(e.ToString());
//                throw;
//            }
//        }
//    }
//}
//#endif