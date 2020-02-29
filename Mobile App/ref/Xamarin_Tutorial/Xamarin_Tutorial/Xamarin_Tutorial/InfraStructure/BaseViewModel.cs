using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Xamarin_Tutorial.InfraStructure
{
	public abstract class BaseViewModel : INotifyPropertyChanged
	{
		#region Attributes

		private Page _coupledView;

		#endregion Attributes

		#region Proterties

		public Page CoupledView => _coupledView;

		#endregion Proterties

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
		public abstract Task<bool> PrepareView(params object[] data);

		#endregion Abstract Method

		#region Methods

		public void SetCoupledView(Page page)
		{
			_coupledView = page;
			_coupledView.BindingContext = this;
		}

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

		#endregion Methods
	}
}