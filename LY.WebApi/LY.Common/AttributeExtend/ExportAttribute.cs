using Microsoft.Extensions.DependencyInjection;

namespace LY.Common.AttributeExtend
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true, Inherited = false)]
    public class ExportAttribute : Attribute
    {
        /// <summary>
        /// 服务(实现)唯一标识
        /// </summary>
        public string Identifier { get; set; }

        /// <summary>
        /// 生命周期
        /// </summary>
        public ServiceLifetime Lifetime { get; set; } = ServiceLifetime.Transient;

        /// <summary>
        /// 指定服务的接口
        /// </summary>
        public Type Interface { get; set; }

        public ExportAttribute()
        {
        }

        public ExportAttribute(Type serviceType) : this(serviceType, ServiceLifetime.Transient, null)
        {

        }

        public ExportAttribute(ServiceLifetime serviceLifetime) : this(null, serviceLifetime, null)
        {

        }

        private ExportAttribute(Type serviceType, ServiceLifetime serviceLifetime, string identifier)
        {
            Interface = serviceType;
            Lifetime = serviceLifetime;
            Identifier = identifier;
        }

    }
}
