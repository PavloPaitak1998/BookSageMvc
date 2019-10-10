using System.Web;
using System.Web.Mvc;

namespace Server_WEB_Programming.Lab2
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
