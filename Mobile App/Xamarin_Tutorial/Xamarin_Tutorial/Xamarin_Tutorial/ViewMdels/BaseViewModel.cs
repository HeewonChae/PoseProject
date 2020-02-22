using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin_Tutorial.InfraStructure;

namespace Xamarin_Tutorial.ViewMdels
{
	public abstract class BaseViewModel : INotifyPropertyChanged, ICarryView
	{
		#region ICarryView Impl

		protected Page _coupledView;
		public Page CoupledView { get => _coupledView; set => _coupledView = value; }

		public void SetView(Page page)
		{
			_coupledView = page;
			_coupledView.BindingContext = this;
		}

		#endregion ICarryView Impl

		#region Events

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion Events

		#region Constructors

		public BaseViewModel()
		{
		}

		#endregion Constructors

		#region Abstract Method

		/// <summary>
		/// Page는 null 일 수 있음
		/// </summary>
		/// <returns></returns>
		public abstract Task<Page> ShowView();

		#endregion Abstract Method

		protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(
						this,
						new PropertyChangedEventArgs(propertyName));
		}

		protected void SetValue<T>(ref T backingField, T value, [CallerMemberName] string propertyName = null)
		{
			if (EqualityComparer<T>.Default.Equals(backingField, value))
				return;

			backingField = value;
			OnPropertyChanged(propertyName);
		}
	}
}