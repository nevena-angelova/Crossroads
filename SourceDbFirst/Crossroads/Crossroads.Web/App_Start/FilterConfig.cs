using Crossroads.Web.Infrastructure.Account;
using Crossroads.Web.Infrastructure.ActionFilters;
using System.Web.Mvc;

namespace Crossroads.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());

            filters.Add(new LogUserActionTimeFilter());
        }
    }
}
