using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace PoseSportsPredict.ViewModels.Base
{
    /// <summary>
    /// Only for PageContent
    /// </summary>
    public abstract class NavigableViewModel : BaseViewModel
    {
        private Page _coupledPage;
        public Page CoupledPage => _coupledPage;

        protected NavigableViewModel()
        {
        }

        protected NavigableViewModel(Page coupledPage)
        {
            _coupledPage = coupledPage;
            if (_coupledPage != null)
                _coupledPage.BindingContext = this;
        }

        protected void SetCoupledPage(Page page)
        {
            _coupledPage = page;
            if (_coupledPage != null)
                _coupledPage.BindingContext = this;
        }
    }
}