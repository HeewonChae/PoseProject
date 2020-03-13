using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;
using XF.Material.Forms.UI.Dialogs;

namespace PoseSportsPredict.ViewModels.Base
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

        private bool _isBusy;

        #endregion Attributes

        #region Proterties

        public bool IsBusy { get => _isBusy; set => SetValue(ref _isBusy, value); }

        #endregion Proterties

        #region Abstract Method

        /// <summary>
        /// 컨텍스트가 바인딩 될때 한번 호출
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        public virtual bool OnInitializeView(params object[] datas)
        {
            return true;
        }

        /// <summary>
        /// 페이지가 오픈될 때 마다 매번 호출
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        public virtual void OnAppearing(params object[] datas)
        {
        }

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

        public virtual void SetIsBusy(bool isBusy)
        {
            IsBusy = isBusy;
        }

        #endregion Methods
    }
}