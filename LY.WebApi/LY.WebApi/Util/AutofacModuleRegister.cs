using Autofac;
using Autofac.Extras.DynamicProxy;
using log4net;
using System.Reflection;

namespace LY.WebApi.Util
{
    public class AutofacModuleRegister : Autofac.Module
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(AutofacModuleRegister));

        protected override void Load(ContainerBuilder builder)
        {
            var basePath = AppContext.BaseDirectory;
            //builder.RegisterType<UserServices>().As<IUserServices>();


            //builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly());
            //// 程序集扫描 RegisterAssemblyTypes 
            //builder.RegisterAssemblyTypes(typeof(BaseRepository<>).Assembly)
            //    .Where(t => t.Name.EndsWith("Repository"))
            //    .InstancePerLifetimeScope().AsImplementedInterfaces();

            #region 带有接口层的服务注入

            ////加载Repository
            //var Repository = Assembly.Load("*.Repository");
            //builder.RegisterAssemblyTypes(Repository)
            //    .Where(t => t.Name.EndsWith("Repository"))
            //    .AsImplementedInterfaces();
            ////加载Services
            //var IService = Assembly.Load("*.IServices");
            //var Service = Assembly.Load("*.Services");
            //builder.RegisterAssemblyTypes(IService, Service)
            //    .Where(t => t.Name.EndsWith("Services"))
            //    .AsImplementedInterfaces();



            var servicesDllFile = Path.Combine(basePath, "Fac.Services.dll");
            var repositoryDllFile = Path.Combine(basePath, "Fac.Repository.dll");

            if (!(File.Exists(servicesDllFile) && File.Exists(repositoryDllFile)))
            {
                var msg = "Repository.dll和service.dll 丢失，因为项目解耦了，所以需要先F6编译，再F5运行，请检查 bin 文件夹，并拷贝。";
                log.Error(msg);
                throw new Exception(msg);
            }

            var cacheType = new List<Type>();
            //builder.RegisterGeneric(typeof(BaseRepository<>)).As(typeof(IBaseRepository<>)).InstancePerDependency();//注册仓储
            //builder.RegisterAssemblyTypes(Assembly.GetAssembly(typeof(BaseRepository<>)))
            //    .EnableClassInterceptors();

            // 获取 Repository.dll 程序集服务，并注册
            var assemblysRepository = Assembly.LoadFrom(repositoryDllFile);
            builder.RegisterAssemblyTypes(assemblysRepository)
                   .AsImplementedInterfaces()
                   .InstancePerDependency()
                   .InterceptedBy(cacheType.ToArray());

            // 获取 Service.dll 程序集服务，并注册
            var assemblysServices = Assembly.LoadFrom(servicesDllFile);
            builder.RegisterAssemblyTypes(assemblysServices)
                      .AsImplementedInterfaces()
                      .InstancePerDependency()
                      .EnableInterfaceInterceptors()
                      .InterceptedBy(cacheType.ToArray());


            #endregion

            #region 没有接口的单独类，启用class代理拦截

            ////只能注入该类中的虚方法，且必须是public
            ////这里仅仅是一个单独类无接口测试，不用过多追问
            //builder.RegisterAssemblyTypes(Assembly.GetAssembly(typeof(UserServices)))
            //    .EnableClassInterceptors();
            #endregion

            #region 单独注册一个含有接口的类，启用interface代理拦截

            //不用虚方法
            //builder.RegisterType<AopService>().As<IAopService>()
            //   .AsImplementedInterfaces()
            //   .EnableInterfaceInterceptors()
            //   .InterceptedBy(typeof(BlogCacheAOP));

            //builder.RegisterType<SingleInstanceServices>().As<ISingleInstanceServices>()
            //.AsImplementedInterfaces()
            //.EnableInterfaceInterceptors()
            //.SingleInstance();
            #endregion

        }
    }
}
