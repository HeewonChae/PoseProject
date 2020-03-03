using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PoseSportsPredict.InfraStructure
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion INotifyPropertyChanged

        #region Attributes

        private readonly Page _coupledPage;
        private bool _isBusy;
        private string _title = string.Empty;

        #endregion Attributes

        #region Proterties

        public Page CoupledPage => _coupledPage;

        public bool IsBusy { get => _isBusy; set => SetValue(ref _isBusy, value); }

        public string Title { get => _title; set => SetValue(ref _title, value); }

        #endregion Proterties

        #region Constructors

        protected BaseViewModel(Page coupledPage)
        {
            _coupledPage = coupledPage;
            _coupledPage.BindingContext = this;
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

        protected bool SetValue<T>(ref T backingStore, T value,
            [CallerMemberName]string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        public virtual void SetBusy(bool isBusy)
        {
            IsBusy = isBusy;

            if (IsBusy)
                UserDialogs.Instance.ShowLoading();
            else
                UserDialogs.Instance.HideLoading();
        }

        #endregion Methods
    }
}