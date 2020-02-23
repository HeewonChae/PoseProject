﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin_Tutorial.InfraStructure;
using Xamarin_Tutorial.Models;

namespace Xamarin_Tutorial.ViewMdels
{
	public class LandDetailViewModel : BaseViewModel
	{
		#region Attributes

		private string _name;
		private string _capital;
		private double _population;
		private double _area;
		private string _region;

		#endregion Attributes

		#region Properties

		public string CountryName { get => _name; set => SetValue(ref _name, value); }
		public string Capital { get => _capital; set => SetValue(ref _capital, value); }
		public double Population { get => _population; set => SetValue(ref _population, value); }
		public double Area { get => _area; set => SetValue(ref _area, value); }
		public string Region { get => _region; set => SetValue(ref _region, value); }

		#endregion Properties

		#region BaseViewModel Impl

		public override Task<bool> PrepareView(params object[] data)
		{
			if (data == null || data.Length == 0)
				return Task.FromResult(false);

			if (data.First() is CountryItem countryData)
			{
				CountryName = countryData.Name;
				Capital = countryData.Capital;
				Population = countryData.Population;
				Area = countryData.Area.HasValue ? countryData.Area.Value : 0.0;
				Region = countryData.Region;
			}
			else
				return Task.FromResult(false);

			return Task.FromResult(true);
		}

		#endregion BaseViewModel Impl
	}
}