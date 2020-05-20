﻿using GalaSoft.MvvmLight.Command;
using PoseSportsPredict.InfraStructure;
using PoseSportsPredict.Logics.LocalizedRes;
using PoseSportsPredict.Models.Resources.Common;
using PoseSportsPredict.Resources;
using PoseSportsPredict.Utilities.LocalStorage;
using PoseSportsPredict.ViewModels.Base;
using PoseSportsPredict.Views.Common;
using Shiny;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using XF.Material.Forms.UI;

using ComboBox = Syncfusion.XForms.ComboBox;

namespace PoseSportsPredict.ViewModels.Common
{
    internal class SettingsViewModel : NavigableViewModel, IIconChange
    {
        #region NavigableViewModel

        public override bool OnInitializeView(params object[] datas)
        {
            IsSelected = false;

            // Check Interface Language
            var coverageLanguages = CoverageLanguage.CoverageLanguages.Values.OrderBy(elem => elem.Name).ToList();
            Ddl_Groups = new ObservableList<CoverageLanguage>(coverageLanguages);

            LocalStorage.Storage.GetValueOrDefault<string>(LocalStorageKey.UserLanguageId, out string userLanguageId);
            Ddl_selectedIndex = coverageLanguages.FindIndex(elem => elem.Id.Equals(userLanguageId));

            return base.OnInitializeView(datas);
        }

        public override void OnAppearing(params object[] datas)
        {
            IsSelected = true;

            base.OnAppearing(datas);
        }

        public override void OnDisAppearing(params object[] datas)
        {
            IsSelected = false;
        }

        #endregion NavigableViewModel

        #region IIconChange

        private bool _isSelected;

        public NavigationPage NavigationPage { get; set; }

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    NavigationPage.IconImageSource = CurrentIcon;
                }
            }
        }

        public string CurrentIcon { get => IsSelected ? "ic_setting_selected.png" : "ic_setting_unselected.png"; }

        #endregion IIconChange

        #region Fields

        private int _ddl_selectedIndex;
        private ObservableList<CoverageLanguage> _ddl_Groups;

        #endregion Fields

        #region Properties

        public int Ddl_selectedIndex { get => _ddl_selectedIndex; set => SetValue(ref _ddl_selectedIndex, value); }
        public ObservableList<CoverageLanguage> Ddl_Groups { get => _ddl_Groups; set => SetValue(ref _ddl_Groups, value); }

        #endregion Properties

        #region Commands

        public ICommand LanguageSelectionChangedCommand { get => new RelayCommand<ComboBox.SelectionChangedEventArgs>((e) => LanguageSelectionChanged(e)); }

        private void LanguageSelectionChanged(ComboBox.SelectionChangedEventArgs arg)
        {
            if (IsBusy)
                return;

            SetIsBusy(true);

            var seletedLanguage = Ddl_Groups[Ddl_selectedIndex];
            MessagingCenter.Send(this, AppConfig.CULTURE_CHANGED_MSG, seletedLanguage);

            SetIsBusy(false);
        }

        #endregion Commands

        #region Constructors

        public SettingsViewModel(SettingsPage page) : base(page)
        {
            NavigationPage = new MaterialNavigationPage(this.CoupledPage)
            {
                Title = LocalizeString.Settings,
                IconImageSource = CurrentIcon,
            };

            if (OnInitializeView())
            {
                this.CoupledPage.Disappearing += (s, e) => OnDisAppearing();
            }
        }

        #endregion Constructors
    }
}