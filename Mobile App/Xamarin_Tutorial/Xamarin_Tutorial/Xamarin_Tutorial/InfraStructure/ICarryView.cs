using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Xamarin_Tutorial.InfraStructure
{
	public interface ICarryView
	{
		Page CoupledView { get; set; }
		void SetView(Page page);
	}
}
