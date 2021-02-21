//基于 Asp.Net 区域. 文件 结构 必须 这样 划分

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Http;
//using System.Web.Mvc;

//namespace Utility.AspNet.Areas.Admin
//{
//    public class AminAreaRegistration : AreaRegistration
//    {
//        public override string AreaName => "Admin";

//        public override void RegisterArea(AreaRegistrationContext context)
//        {
//            context.Routes.MapHttpRoute(
//                name: "DefaultAdminApi",
//                routeTemplate: "api/admin/{controller}/{action}",
//                defaults: new string[] { "Utility.AspNet.Areas.Admin.Controllers" }
//            );
//        }
//    }
//}