using PoseSportsPredict.InfraStructure;
using PoseSportsPredict.ViewModels.Base;
using PoseSportsPredict.Views.Football;
using Shiny;
using System.Threading.Tasks;
using Xamarin.Forms;
using XF.Material.Forms.UI;

namespace PoseSportsPredict.ViewModels.Football
{
    public class FootballMainViewModel : NavigableViewModel
    {
        #region NavigableViewModel

        public override bool OnPrepareView(params object[] datas)
        {
            var mainPage = this.CoupledPage as FootballMainPage;

            var matchesPage = ShinyHost.Resolve<FootballMatchesTabViewModel>().CoupledPage;
            var matchesNavPage = new MaterialNavigationPage(matchesPage)
            {
                Title = matchesPage.Title,
                IconImageSource = matchesPage.IconImageSource,
            };
            mainPage.Children.Add(matchesNavPage);

            var leaugesPage = ShinyHost.Resolve<FootballLeaguesViewModel>().CoupledPage;
            var leaguesNavPage = new MaterialNavigationPage(leaugesPage)
            {
                Title = leaugesPage.Title,
                IconImageSource = leaugesPage.IconImageSource,
            };
            mainPage.Children.Add(leaguesNavPage);
            return true;
        }

        #endregion NavigableViewModel

        #region Constructors

        public FootballMainViewModel(FootballMainPage page) : base(page)
        {
        }

        #endregion Constructors
    }
}