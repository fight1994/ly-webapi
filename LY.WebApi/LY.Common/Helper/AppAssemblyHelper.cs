using System.Reflection;

namespace LY.Common.Helper
{
    public static class AppAssemblyHelper
    {
        private static readonly AppsettingsHelper appsettingsHelper = new();

        public static List<Assembly> GetControllers()
        {
            var fileName = appsettingsHelper.Get<string>("WebApiAssembly");
            return GetAppAssemblyByFileName(fileName);
        }

        public static List<Assembly> GetServices()
        {
            var fileName = appsettingsHelper.Get<string>("ServicesAssembly");
            return GetAppAssemblyByFileName(fileName);
        }

        public static List<Assembly> GetRepository()
        {
            var fileName = appsettingsHelper.Get<string>("RepositoryAssembly");
            return GetAppAssemblyByFileName(fileName);
        }

        public static List<Assembly> GetAppAssembly()
        {
            List<Assembly> result = new();
            var repository = GetRepository();
            var services = GetServices();
            var controllers = GetControllers();

            if (repository != null && repository.Count > 0)
                result.AddRange(repository);
            if (services != null && services.Count > 0)
                result.AddRange(services);
            if (controllers != null && controllers.Count > 0)
                result.AddRange(controllers);

            return result;
        }

        public static List<Assembly> GetAppAssemblyByFileName(string searchFile)
        {
            return new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory).GetFiles(searchFile).Select(o => Assembly.LoadFrom(o.FullName)).ToList();
        }
    }
}
