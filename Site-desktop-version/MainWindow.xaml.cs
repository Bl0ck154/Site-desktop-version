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

		public MainWindow()
		{
			InitializeComponent();
			Api = new API();
			Loaded += MainWindow_Loaded;
		}

		private void MainWindow_Loaded(object sender, RoutedEventArgs e)
		{
			comboCountry.ItemsSource = Api.getCountries();
		}

		private void comboCountry_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (comboCountry.SelectedItem != null)
			{
				comboCity.ItemsSource = Api.getCities((comboCountry.SelectedItem as Country).id);
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
				Api.AddCountry(txtAddCountryName.Text);
			}
		}

		private void btnDelCountry_Click(object sender, RoutedEventArgs e)
		{

		}
	}
}
