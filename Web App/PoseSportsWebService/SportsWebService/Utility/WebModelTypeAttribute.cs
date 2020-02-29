using System;

namespace SportsWebService.Utility
{
	[AttributeUsage(AttributeTargets.Class)]
	public class WebModelTypeAttribute : Attribute
	{
		public Type InputType { get; set; }
		public Type OutputType { get; set; }

		public WebModelTypeAttribute()
		{ }
	}
}