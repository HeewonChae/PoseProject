using PoseSportsPredict.Models.Resources.Common;
using PoseSportsPredict.Utilities.LocalStorage;
using PoseSportsPredict.ViewModels;
using PoseSportsPredict.ViewModels.Common;
using Shiny;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Resources;
using System.Text;
using Xamarin.Forms;

namespace PoseSportsPredict.Logics.LocalizedRes
{
    public class LocalizeStringRes : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly ResourceManager _resourceManager;

        public string this[string key]
        {
            get
            {
                return _resourceManager.GetString(key);
            }
        }

        public LocalizeStringRes(Type resource)
        {
            _resourceManager = new ResourceManager(resource);

            MessagingCenter.Subscribe<SettingsViewModel, CoverageLanguage>(this,
                AppConfig.CULTURE_CHANGED_MSG, OnCultureChanged);
        }

        private void OnCultureChanged(object sender, CoverageLanguage cl)
        {
            CultureInfo.CurrentCulture = new CultureInfo(cl.Id);
            CultureInfo.CurrentUICulture = new CultureInfo(cl.Id);

            LocalStorage.Storage.AddOrUpdateValue<string>(LocalStorageKey.UserLanguageId, cl.Id);
            ShinyHost.Resolve<AppMasterMenuViewModel>().UpdateCultureInfo();

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Item"));
        }
    }
}