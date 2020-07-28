using System.Collections.ObjectModel;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace SportsWebService.WebBehavior.Server.ErrorHandler
{
    public class ServiceErrorBehavior : IServiceBehavior
    {
        #region IServiceBehavior for Server

        void IServiceBehavior.AddBindingParameters(ServiceDescription serviceDescription,
                ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints,
                BindingParameterCollection bindingParameters)
        {
        }

        void IServiceBehavior.ApplyDispatchBehavior(ServiceDescription desc, ServiceHostBase host)
        {
            foreach (ChannelDispatcher cDispatcher in host.ChannelDispatchers)
            {
                cDispatcher.ErrorHandlers.Add(new ServiceErrorHandler());
            }
        }

        void IServiceBehavior.Validate(ServiceDescription desc, ServiceHostBase host)
        {
        }

        #endregion IServiceBehavior for Server
    }
}