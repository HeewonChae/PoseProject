using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Xamarin_Tutorial.InfraStructure
{
	public interface ICarryViewPage
	{
		Page CoupledViewPage { get; set; }
		void SetViewPage(Page page);
	}
}
