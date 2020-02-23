using System.Linq;
using System.Threading.Tasks;
using Xamarin_Tutorial.InfraStructure;
using Xamarin_Tutorial.Models;

namespace Xamarin_Tutorial.ViewMdels
{
	public class CurrencyViewModel : BaseViewModel
	{
		#region Attributes

		private string _code;
		private string _name;
		private string _symbol;

		#endregion Attributes

		#region Properties

		public string Code { get => _code; set => SetValue(ref _code, value); }
		public string Name { get => _name; set => SetValue(ref _name, value); }
		public string Symbol { get => _symbol; set => SetValue(ref _symbol, value); }

		#endregion Properties

		#region BaseViewModel Impl

		public override Task<bool> PrepareView(params object[] data)
		{
			if (data == null || data.Length == 0)
				return Task.FromResult(false);

			if (data.First() is CountryItem countryData)
			{
				Code = countryData.Currencies.FirstOrDefault()?.Code ?? "";
				Name = countryData.Currencies.FirstOrDefault()?.Name ?? "";
				Symbol = countryData.Currencies.FirstOrDefault()?.Symbol ?? "";
			}
			else
				return Task.FromResult(false);

			return Task.FromResult(true);
		}

		#endregion BaseViewModel Impl
	}
}