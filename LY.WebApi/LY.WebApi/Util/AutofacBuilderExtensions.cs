using Autofac;
using Autofac.Extras.DynamicProxy;
using LY.Common.AttributeExtend;
using LY.Common.Helper;
using System.Reflection;

namespace LY.WebApi.Util
{
    public static class AutofacBuilderExtensions
    {
        /// <summary>
        /// 通过特性判断，将不同生命周期的服务注入容器
        /// </summary>
        /// <param name="builder">拓展WebApplicationBuilder</param>
        public static void AddAppServices(this WebApplicationBuilder builder)
        {
            var appAssemblies = AppAssemblyHelper.GetAppAssembly();
            foreach (var assembly in appAssemblies)
            {
                foreach (var type in assembly.GetTypes())
                {
                    foreach (var serviceAttribute in type.GetCustomAttributes<ExportAttribute>())
                    {
                        if (serviceAttribute != null)
                        {
                            var iInterface = serviceAttribute.Interface;

                            //类型检查,如果 type 不是 iInterface 的实现或子类或本身
                            //运行时 type 将无法解析为 iInterface 的实例
                            if (iInterface != null && !iInterface.IsAssignableFrom(type))
                            {
                                iInterface = null;
                            }

                            if (iInterface == null)
                            {
                                iInterface = type;
                            }

                            switch (serviceAttribute.Lifetime)
                            {
                                case ServiceLifetime.Singleton:
                                    builder.Host.ConfigureContainer<ContainerBuilder>(container =>
                                    {
                                        container.RegisterType(type).As(iInterface)
                                        .AsImplementedInterfaces()
                                        .EnableInterfaceInterceptors()
                                        .SingleInstance();
                                    });
                                    break;

                                case ServiceLifetime.Scoped:
                                    builder.Host.ConfigureContainer<ContainerBuilder>(container =>
                                    {
                                        container.RegisterType(type).As(iInterface)
                                        .AsImplementedInterfaces()
                                        .EnableInterfaceInterceptors()
                                        .InstancePerLifetimeScope();
                                    });
                                    break;

                                case ServiceLifetime.Transient:
                                    builder.Host.ConfigureContainer<ContainerBuilder>(container =>
                                    {
                                        container.RegisterType(type).As(iInterface)
                                        .AsImplementedInterfaces()
                                        .EnableInterfaceInterceptors()
                                        .InstancePerDependency();
                                    });
                                    break;

                                default:
                                    break;
                            }
                        }
                    }
                }
            }
        }
    }
}
