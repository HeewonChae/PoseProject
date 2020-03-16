using PoseSportsPredict.InfraStructure;
using PoseSportsPredict.Models;
using System.Linq;
using Xamarin.Forms;

namespace PoseSportsPredict.Logics.Common
{
    internal class ShellSearchHandler : SearchHandler
    {
        public IDataStore<Item> DataStore => DependencyService.Get<IDataStore<Item>>();

        protected override async void OnQueryChanged(string oldValue, string newValue)
        {
            base.OnQueryChanged(oldValue, newValue);

            if (string.IsNullOrWhiteSpace(newValue))
            {
                ItemsSource = null;
            }
            else
            {
                ItemsSource = (await DataStore.GetItemsAsync())
                    .Where(elem => elem.Text.ToLower().Contains(newValue.ToLower()))
                    .ToList<Item>();
            }
        }

        protected override void OnItemSelected(object item)
        {
            base.OnItemSelected(item);

            //// Note: strings will be URL encoded for navigation (e.g. "Blue Monkey" becomes "Blue%20Monkey"). Therefore, decode at the receiver.
            //await (App.Current.MainPage as Xamarin.Forms.Shell).GoToAsync($"monkeydetails?name={((Animal)item).Name}");
        }
    }
}