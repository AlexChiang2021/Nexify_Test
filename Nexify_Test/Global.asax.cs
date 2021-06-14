using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Nexify_Test
{
    // 注意: 如需啟用 IIS6 或 IIS7 傳統模式的說明，
    // 請造訪 http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }

    #region 外掛 AjaxOlny 屬性 class
    /// <summary>
    /// 外掛 AjaxOlny 屬性 class
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class AjaxOnlyAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// 在動作方法執行之前，由 ASP.NET MVC 架構呼叫。
        /// </summary>
        /// <param name="filterContext">篩選內容。</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.HttpContext.Response.StatusCode = 404;
                filterContext.Result = new HttpNotFoundResult();
            }
            else
            {
                base.OnActionExecuting(filterContext);
            }
        }
    }
    #endregion
}