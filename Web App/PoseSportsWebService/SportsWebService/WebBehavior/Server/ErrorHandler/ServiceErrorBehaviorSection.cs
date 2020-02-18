using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Configuration;
using System.Text;
using System.Threading.Tasks;

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
