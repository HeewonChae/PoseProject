using System.Collections.ObjectModel;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace SportsWebService.WebBehavior.Common.HeaderProcess
{
	public class HeaderMessageBahavior : IServiceBehavior//, IEndpointBehavior
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
				foreach (EndpointDispatcher eDispatcher in cDispatcher.Endpoints)
				{
					eDispatcher.DispatchRuntime.MessageInspectors.Add(new HeaderMessageInspector());
				}
			}
		}

		void IServiceBehavior.Validate(ServiceDescription desc, ServiceHostBase host)
		{
		}

		#endregion IServiceBehavior for Server

		#region IEndpointBehavior for Client

		//public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
		//{
		//    var channelDispatcher = endpointDispatcher.ChannelDispatcher;
		//    if (channelDispatcher == null) return;
		//    foreach (var ed in channelDispatcher.Endpoints)
		//    {
		//        var inspector = new HeaderMessageInspector();
		//        ed.DispatchRuntime.MessageInspectors.Add(inspector);
		//    }
		//}

		//void IEndpointBehavior.AddBindingParameters(ServiceEndpoint endpoint,
		//     System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
		//{
		//}

		//void IEndpointBehavior.ApplyClientBehavior(ServiceEndpoint endpoint,
		//         System.ServiceModel.Dispatcher.ClientRuntime clientRuntime)
		//{
		//    var inspector = new HeaderMessageInspector();
		//    clientRuntime.MessageInspectors.Add(inspector);
		//}

		//void IEndpointBehavior.Validate(ServiceEndpoint endpoint)
		//{
		//}

		#endregion IEndpointBehavior for Client
	}
}