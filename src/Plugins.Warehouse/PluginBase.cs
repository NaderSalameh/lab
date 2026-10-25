using Microsoft.Xrm.Sdk;
using System;

namespace Plugins.Warehouse
{
    public interface ILocalPluginContext
    {
        IPluginExecutionContext PluginExecutionContext { get; }
        IOrganizationServiceFactory OrgSvcFactory { get; }
        ITracingService TracingService { get; }
    }

    internal sealed class LocalPluginContext : ILocalPluginContext
    {
        public LocalPluginContext(
            IPluginExecutionContext pluginExecutionContext,
            IOrganizationServiceFactory organizationServiceFactory,
            ITracingService tracingService)
        {
            PluginExecutionContext = pluginExecutionContext ?? throw new ArgumentNullException(nameof(pluginExecutionContext));
            OrgSvcFactory = organizationServiceFactory ?? throw new ArgumentNullException(nameof(organizationServiceFactory));
            TracingService = tracingService ?? throw new ArgumentNullException(nameof(tracingService));
        }

        public IPluginExecutionContext PluginExecutionContext { get; }
        public IOrganizationServiceFactory OrgSvcFactory { get; }
        public ITracingService TracingService { get; }
    }

    public abstract class PluginBase : IPlugin
    {
        protected PluginBase(Type pluginType)
        {
            if (pluginType == null)
            {
                throw new ArgumentNullException(nameof(pluginType));
            }
        }

        public void Execute(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
            {
                throw new ArgumentNullException(nameof(serviceProvider));
            }

            var context = serviceProvider.GetService(typeof(IPluginExecutionContext)) as IPluginExecutionContext;
            var serviceFactory = serviceProvider.GetService(typeof(IOrganizationServiceFactory)) as IOrganizationServiceFactory;
            var tracingService = serviceProvider.GetService(typeof(ITracingService)) as ITracingService;

            if (context == null || serviceFactory == null || tracingService == null)
            {
                throw new InvalidPluginExecutionException("The plugin execution context is unavailable.");
            }

            ExecuteDataversePlugin(new LocalPluginContext(context, serviceFactory, tracingService));
        }

        protected abstract void ExecuteDataversePlugin(ILocalPluginContext localPluginContext);
    }
}
