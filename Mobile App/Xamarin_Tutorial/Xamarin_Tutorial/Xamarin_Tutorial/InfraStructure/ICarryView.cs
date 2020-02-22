using Xamarin.Forms;

namespace Xamarin_Tutorial.InfraStructure
{
	public interface ICarryView
	{
		Page CoupledView { get; set; }

		void SetView(Page page);
	}
}