using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace SportsWebService.WebBehavior.Server.ServiceInitialize
{
    public class ServiceInitializationBehavior : IServiceBehavior
    {
        private readonly IServiceInitialization _serviceInitialization;

        public ServiceInitializationBehavior(IServiceInitialization serviceInitialization)
        {
            if (serviceInitialization == null)
                throw new ArgumentNullException("serviceInitialization");

            _serviceInitialization = serviceInitialization;
        }

        public void AddBindingParameters(ServiceDescription serviceDescription,
                                         ServiceHostBase serviceHostBase,
                                         Collection<ServiceEndpoint> endpoints,
                                         BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription,
                                          ServiceHostBase serviceHostBase)
        {
            _serviceInitialization.Initialize();
        }

        public void Validate(ServiceDescription serviceDescription,
                             ServiceHostBase serviceHostBase)
        {
        }
    }
}
