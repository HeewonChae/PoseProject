using Android.Content;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Xamarin_Tutorial;
using Xamarin_Tutorial.Droid;

[assembly: ExportRenderer(typeof(CustomView), typeof(CustomRenderer_android))]

namespace Xamarin_Tutorial.Droid
{
	public class CustomRenderer_android : ViewRenderer<CustomView, Android.Widget.DatePicker>
	{
		private readonly Context _context;

		public CustomRenderer_android(Context context) : base(context)
		{
			_context = context;
		}

		protected override void OnElementChanged(ElementChangedEventArgs<CustomView> e)
		{
			base.OnElementChanged(e);

			var datePicker = new Android.Widget.DatePicker(_context);

			SetNativeControl(datePicker);
		}
	}
}