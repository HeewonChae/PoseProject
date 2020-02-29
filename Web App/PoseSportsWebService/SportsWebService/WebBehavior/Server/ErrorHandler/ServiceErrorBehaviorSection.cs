using System;
using System.ServiceModel.Configuration;

namespace SportsWebService.WebBehavior.Server.ErrorHandler
{
	public class ServiceErrorBehaviorSection : BehaviorExtensionElement
	{
		public override Type BehaviorType => typeof(ServiceErrorBehavior);

		protected override object CreateBehavior()
		{
			return new ServiceErrorBehavior();
		}
	}
}