using CaptchaMvc.Infrastructure;
using CaptchaMvc.Interface;
using Crossroads.Web.Infrastructure.Account;
using System;
using System.Collections.Concurrent;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Crossroads.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public const string MultipleParameterKey = "_multiple_";

        private static readonly ConcurrentDictionary<int, ICaptchaManager> CaptchaManagers =
            new ConcurrentDictionary<int, ICaptchaManager>();

        protected void Application_Start()
        {
            AutoMapperConfig.Execute();


            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());

            CaptchaUtils.CaptchaManagerFactory = GetCaptchaManager;
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            MyPrincipal principal = new MyPrincipal();
            HttpContext.Current.User = principal;
        }

        private static ICaptchaManager GetCaptchaManager(IParameterContainer parameterContainer)
        {
            int numberOfCaptcha;
            if (parameterContainer.TryGet(MultipleParameterKey, out numberOfCaptcha))
                return CaptchaManagers.GetOrAdd(numberOfCaptcha, CreateCaptchaManagerByNumber);

            //If not found parameter return default manager.
            return CaptchaUtils.CaptchaManager;
        }

        private static ICaptchaManager CreateCaptchaManagerByNumber(int i)
        {
            var captchaManager = new DefaultCaptchaManager(new SessionStorageProvider());
            captchaManager.ImageElementName += i;
            captchaManager.InputElementName += i;
            captchaManager.TokenElementName += i;
            captchaManager.ImageUrlFactory = (helper, pair) =>
            {
                var dictionary = new RouteValueDictionary();
                dictionary.Add(captchaManager.TokenParameterName, pair.Key);
                dictionary.Add(MultipleParameterKey, i);
                return helper.Action("Generate", "DefaultCaptcha", dictionary);
            };
            captchaManager.RefreshUrlFactory = (helper, pair) =>
            {
                var dictionary = new RouteValueDictionary();
                dictionary.Add(MultipleParameterKey, i);
                return helper.Action("Refresh", "DefaultCaptcha", dictionary);
            };
            return captchaManager;
        }
    }
}
