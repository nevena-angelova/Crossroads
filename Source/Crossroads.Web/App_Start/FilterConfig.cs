﻿using System.Web.Mvc;

namespace Crossroads.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());

            //filters.Add(new LogUserActionTimeFilter());
        }
    }
}