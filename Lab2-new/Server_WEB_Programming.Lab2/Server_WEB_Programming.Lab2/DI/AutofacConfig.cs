using Autofac;
using Autofac.Integration.Mvc;
using AutoMapper;
using Server_WEB_Programming.Lab2.Dal.DataBase;
using Server_WEB_Programming.Lab2.Dal.UoW.Implementations;
using Server_WEB_Programming.Lab2.Dal.UoW.Interfaces;
using System.Collections.Generic;
using System.Data.Entity;
using System.Web.Mvc;

namespace Server_WEB_Programming.Lab2.DI
{
    public class AutofacConfig
    {
        public static void ConfigureContainer()
        {
            var builder = new ContainerBuilder();

            builder
                .RegisterControllers(typeof(MvcApplication).Assembly);

            builder
                .RegisterType<UnitOfWork>()
                .As<IUnitOfWork>();

            builder.RegisterAssemblyTypes()
                .AssignableTo(typeof(Profile))
                .As<Profile>();

            builder.Register(c => new MapperConfiguration(cfg =>
            {
                foreach (var profile in c.Resolve<IEnumerable<Profile>>())
                {
                    cfg.AddProfile(profile);
                }
            }))
                .AsSelf()
                .SingleInstance();

            builder.Register(c => c.Resolve<MapperConfiguration>().CreateMapper(c.Resolve))
                .As<IMapper>()
                .InstancePerLifetimeScope();

            var container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}