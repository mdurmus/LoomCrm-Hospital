﻿using System.Web;
using System.Web.Mvc;

namespace LoomCrm_Hospital
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
