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

        public bool IsPageSwitched { get; private set; }

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

        public void SetIsPageSwitched(bool isPageSwitched)
        {
            IsPageSwitched = isPageSwitched;
        }
    }
}