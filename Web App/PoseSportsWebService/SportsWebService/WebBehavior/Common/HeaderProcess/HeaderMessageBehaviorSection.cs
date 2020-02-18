using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Configuration;
using System.Text;
using System.Threading.Tasks;

namespace SportsWebService.WebBehavior.Common.HeaderProcess
{
	public class HeaderMessageBehaviorSection : BehaviorExtensionElement
	{
		public override Type BehaviorType => typeof(HeaderMessageBahavior);

		protected override object CreateBehavior()
		{
			return new HeaderMessageBahavior();
		}
	}
}
