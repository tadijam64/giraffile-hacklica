using AutoMapper;
using Girafile.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Girafile
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Mapper.Initialize(cfg =>
            {
                //cfg.CreateMap<List<DocumentDTO2>, List<Document>>();
                //cfg.CreateMap<List<Document>, List<DocumentDTO2>>();
                cfg.CreateMap<DocumentDTO2,Document>();
                cfg.CreateMap<Document, DocumentDTO2>();
            });
        }
    }
}
