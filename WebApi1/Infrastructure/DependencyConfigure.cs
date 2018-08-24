using Autofac;
using Autofac.Integration.WebApi;
using Entity;
using Repository;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace WebApi1.Infrastructure
{
    internal class DependencyConfigure
    {
      
        public static void Initialize(HttpConfiguration config)
        {
            Initialize(config, RegisterServices(new ContainerBuilder()));
        }

        public static void Initialize(HttpConfiguration config, IContainer container)
        {
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }


        private static IContainer RegisterServices(ContainerBuilder builder)
        {

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterType<EmployeeContext>()
                   .As<IEmployeeContext>()
                   .InstancePerRequest();
            builder.RegisterGeneric(typeof(GenericRepository<>))
                  .As(typeof(IRepository<>))
                  .InstancePerRequest();
            
            builder.RegisterAssemblyTypes(typeof(WebApiApplication).Assembly).PropertiesAutowired();

            //deal with your dependencies here
            builder.RegisterType<EmployeeService>().As<IEmployeeService>();

            return builder.Build();
        }
    }
}