using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Site_desktop_version
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private API Api;
		List<Country> countries;
		List<City> cities;

		public MainWindow()
		{
			InitializeComponent();
			Api = new API();
			Loaded += MainWindow_Loaded;
		}

		private void MainWindow_Loaded(object sender, RoutedEventArgs e)
		{
			LoadCountries();
			LoadCities();
			LoadHotels();
		}

		void LoadCountries()
		{
			countries = Api.getCountries();
			comboCountry.ItemsSource = countries;
			datagridAddedCountries.ItemsSource = countries;
			comboAddedCountries.ItemsSource = countries;
		}
		void LoadCities()
		{
			if (countries == null)
				LoadCountries();

			cities = Api.getCities();
			datagridAddedCities.ItemsSource = cities.Join(countries, 
				c => c.countryId, s => s.id,
				(c, s) => new { c.id, c.cityName, s.countryName });
		}
		void LoadHotels()
		{
			if (countries == null)
				LoadCountries();

			if (cities == null)
				LoadCities();

			var hotels = Api.getHotels().Join(cities,
				h => h.cityId, c => c.id,
				(h, c) => new { h.id, h.hotelName, c.cityName, h.countryId }).Join(countries,
				h => h.countryId, c => c.id,
				(h, c) => new { h.id, contryCity = h.cityName + " : " + c.countryName, h.hotelName });

			datagridAddedHotels.ItemsSource = hotels;
			
		}

		private void comboCountry_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (comboCountry.SelectedItem != null)
			{
				int selectedCountryId = (comboCountry.SelectedItem as Country).id;
				//	comboCity.ItemsSource = Api.getCities(selectedCountryId);
				comboCity.ItemsSource = cities.Where(c => c.countryId == selectedCountryId);
				comboCity.Visibility = Visibility.Visible;
			}
		}

		private void comboCity_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (comboCity.SelectedItem != null)
			{
				datagridHotels.ItemsSource = Api.getHotels((comboCity.SelectedItem as City).id);
				datagridHotels.Visibility = Visibility.Visible;
			}
		}

		private void btnAddCountry_Click(object sender, RoutedEventArgs e)
		{
			if(!string.IsNullOrWhiteSpace(txtAddCountryName.Text))
			{
				if(Api.AddCountry(txtAddCountryName.Text))
				{
					LoadCountries();
					txtAddCountryName.Clear();
				}
				else
				{
					var messageQueue = SnackbarCountry.MessageQueue;
					Task.Run(() => messageQueue.Enqueue("Error"));
				}
			}
		}

		private void btnDelCountry_Click(object sender, RoutedEventArgs e)
		{
			if(datagridAddedCountries.SelectedItems.Count > 0)
			{
				foreach (var item in datagridAddedCountries.SelectedItems)
				{
					Api.RemoveCountry((item as Country).id);
				}
				LoadCountries();
			}
		}

		private void btnAddCity_Click(object sender, RoutedEventArgs e)
		{
			if (!string.IsNullOrWhiteSpace(txtAddCityName.Text) && comboAddedCountries.SelectedItem != null)
			{
				if (Api.AddCity(txtAddCityName.Text, (comboAddedCountries.SelectedItem as Country).id))
				{
					LoadCities();
					txtAddCityName.Clear();
				}
				else
				{
					var messageQueue = SnackbarCities.MessageQueue;
					Task.Run(() => messageQueue.Enqueue("Error"));
				}
			}
		}

		private void btnDelCity_Click(object sender, RoutedEventArgs e)
		{
			if (datagridAddedCities.SelectedItems.Count > 0)
			{
				foreach (var item in datagridAddedCities.SelectedItems)
				{
					Api.RemoveCity((item as City).id);
				}
				LoadCities();
			}
		}

		private void btnRegister_Click(object sender, RoutedEventArgs e)
		{
			if(string.IsNullOrWhiteSpace(txtLogin.Text) ||
				string.IsNullOrWhiteSpace(txtEmail.Text) ||
				string.IsNullOrWhiteSpace(txtPassword.Text))
				return;

			if(Api.AddUser(txtLogin.Text, txtPassword.Text, txtEmail.Text))
			{
				AddedUserMessage.Visibility = Visibility.Visible;
			}
		}

		private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			AddedUserMessage.Visibility = Visibility.Hidden;
		}
	}
}
