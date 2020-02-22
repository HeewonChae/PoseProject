using Xamarin.Forms.Platform.UWP;
using Xamarin_Tutorial;
using Xamarin_Tutorial.UWP;

[assembly: ExportRenderer(typeof(CustomView), typeof(CustomRenderer_UWP))]

namespace Xamarin_Tutorial.UWP
{
	public class CustomRenderer_UWP : ViewRenderer<CustomView, Xamarin.Forms.Platform.UWP.EntryCellTextBox>
	{
		protected override void OnElementChanged(ElementChangedEventArgs<CustomView> e)
		{
			base.OnElementChanged(e);

			var ectb = new EntryCellTextBox();

			SetNativeControl(ectb);
		}
	}
}