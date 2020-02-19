using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin_Tutorial.InfraStructure;

namespace Xamarin_Tutorial.ViewMdels
{
	public abstract class BaseViewModel : INotifyPropertyChanged, ICarryViewPage
	{
		#region IViewPage Impl
		protected Page _coupledViewPage;
		public Page CoupledViewPage { get => _coupledViewPage; set => _coupledViewPage = value; } 

		public void SetViewPage(Page page)
		{
			_coupledViewPage = page;
		}
		#endregion

		#region Events
		public event PropertyChangedEventHandler PropertyChanged;
		#endregion

		#region Constructors
		public BaseViewModel(){}
		#endregion

		#region Abstract Method
		/// <summary>
		/// Page는 null 일 수 있음
		/// </summary>
		/// <returns></returns>
		public abstract Task<Page> ShowViewPage(); 
		#endregion

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
