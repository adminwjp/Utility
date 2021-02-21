#if NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48
using Swashbuckle.Application;
using System.Configuration;
using System.Reflection;
using System.Web;
using System.Web.Http;
using Utility.AspNet;

//[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]
namespace Utility.AspNet
{
    public class SwaggerConfig
    {
        public static string SwaggerVersion;
        public static string SwaggerTitle;
        /// <summary>
        /// 没有 这 swagger 显示 不出来 
        /// </summary>
        public static Assembly Assembly { get; set; }
        static SwaggerConfig()
        {
            
        }
        public static void Register()
        {
            if (string.IsNullOrEmpty(SwaggerVersion))
            {
                SwaggerVersion = ConfigurationManager.AppSettings["SwaggerVersion"];
                if (string.IsNullOrEmpty(SwaggerVersion))
                {
                    SwaggerVersion = "V1";
                }
            }
            if (string.IsNullOrEmpty(SwaggerTitle))
            {
                SwaggerTitle = ConfigurationManager.AppSettings["SwaggerTitle"];
                if (string.IsNullOrEmpty(SwaggerTitle))
                {
                    SwaggerTitle = "WebApi";
                }
            }
            Assembly = Assembly ?? typeof(SwaggerConfig).Assembly;
            GlobalConfiguration.Configuration
                .EnableSwagger(c =>
                {
                    c.SingleApiVersion(SwaggerVersion,SwaggerTitle);

                    // If you annotate Controllers and API Types with
                    // Xml comments (http://msdn.microsoft.com/en-us/library/b2s063f7(v=vs.110).aspx), you can incorporate
                    // those comments into the generated docs and UI. You can enable this by providing the path to one or
                    // more Xml comment files.

                })
                .EnableSwaggerUi(c =>
                {
                    // Use this option to control how the Operation listing is displayed.
                    // It can be set to "None" (default), "List" (shows operations for each resource),
                    // or "Full" (fully expanded: shows operations and their details).
                    //  展开所有接口列表
                    c.DocExpansion(DocExpansion.List);
                    c.DocumentTitle(SwaggerTitle);//显示在浏览器的tab上的标题  
                   // c.InjectJavaScript(Assembly, "cms.WebApi.Scripts.Swagger.swagger_lang.js");//汉化js
                });
        }


    }
}
#endif