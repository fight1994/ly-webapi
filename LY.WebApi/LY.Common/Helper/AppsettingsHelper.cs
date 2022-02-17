using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace LY.Common.Helper
{
    /// <summary>
    /// appsettings.json操作类
    /// </summary>
    public class AppsettingsHelper
    {
        private IConfiguration Configuration { get; set; }

        public AppsettingsHelper()
        {
            string Path = "appsettings.json";

            Configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .Add(new JsonConfigurationSource { Path = Path, Optional = false, ReloadOnChange = true })//这样的话，可以直接读目录里的json文件，而不是 bin 文件夹下的，所以不用修改复制属性
               .Build();
        }

        /// <summary>
        /// 封装要操作的字符
        /// </summary>
        /// <param name="sections">节点配置</param>
        /// <returns></returns>
        public string Get(params string[] sections)
        {
            try
            {

                if (sections.Any())
                {
                    return Configuration[string.Join(":", sections)];
                }
            }
            catch (Exception) { }

            return "";
        }

        /// <summary>
        /// 递归获取配置信息数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sections"></param>
        /// <returns></returns>
        public List<T> Get<T>(params string[] sections)
        {
            List<T> list = new List<T>();
            // 引用 Microsoft.Extensions.Configuration.Binder 包
            Configuration.Bind(string.Join(":", sections), list);
            return list;
        }

        public T Get<T>(string key)
        {
            return Configuration.GetSection(key).Get<T>();
        }
    }
}
