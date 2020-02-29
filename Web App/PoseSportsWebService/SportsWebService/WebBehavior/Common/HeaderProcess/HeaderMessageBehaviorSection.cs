using System;
using System.ServiceModel.Configuration;

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