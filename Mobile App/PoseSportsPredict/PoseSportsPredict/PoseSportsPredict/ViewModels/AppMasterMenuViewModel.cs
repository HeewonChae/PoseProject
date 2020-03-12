using PoseSportsPredict.InfraStructure;
using PoseSportsPredict.Models;
using PoseSportsPredict.ViewModels.Base;
using PoseSportsPredict.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PoseSportsPredict.ViewModels
{
    public class AppMasterMenuViewModel : NavigableViewModel
    {
        #region BaseViewModel

        public override bool OnPrepareView(params object[] datas)
        {
            if (datas == null)
                return true;

            var sportsCategories = new List<MasterMenuItem>();
            foreach (var data in datas)
            {
                if (data is NavigableViewModel viewModel)
                {
                    sportsCategories.Add(new MasterMenuItem
                    {
                        Title = viewModel.CoupledPage.Title,
                        IconSource = viewModel.CoupledPage.IconImageSource.ToString().Replace("File: ", ""),
                    });
                }
            }

            SportsCategories = sportsCategories;

            return true;
        }

        #endregion BaseViewModel

        #region Fields

        private List<MasterMenuItem> _sportsCategories;

        #endregion Fields

        #region Properties

        public List<MasterMenuItem> SportsCategories { get => _sportsCategories; set => SetValue(ref _sportsCategories, value); }

        #endregion Properties

        #region Constructors

        public AppMasterMenuViewModel(AppMasterMenuPage page) : base(page)
        {
        }

        #endregion Constructors
    }
}