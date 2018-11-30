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
		private string responseJson;

		public MainWindow()
		{
			InitializeComponent();
			LoadComboboxCountriesAsync();
		}

		private void comboCountry_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if ((sender as ComboBox).SelectedIndex == 0)
				return;

			comboCity.Visibility = Visibility.Visible;
		}
		private void LoadComboboxCountriesAsync()
		{
			WebRequest request = WebRequest.Create("http://travel.itstep.fun/api/travelApi.php");
			request.Method = "POST"; // для отправки используется метод Post
									 // данные для отправки
			string data = "token=ps_rpo_1&param=getCountries";
			// преобразуем данные в массив байтов
			byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(data);
			// устанавливаем тип содержимого - параметр ContentType
			request.ContentType = "application/x-www-form-urlencoded";
			// Устанавливаем заголовок Content-Length запроса - свойство ContentLength
			request.ContentLength = byteArray.Length;
			
			using (Stream dataStream = request.GetRequestStream())
			{
				dataStream.Write(byteArray, 0, byteArray.Length);
			}

			WebResponse response = request.GetResponse();
			using (Stream stream = response.GetResponseStream())
			{
				using (StreamReader reader = new StreamReader(stream))
				{
					responseJson = reader.ReadToEnd();
				}
			}
			response.Close();
			comboCountry.Items.Clear();
			comboCountry.ItemsSource
				= JsonConvert.DeserializeObject<List<Country>>(responseJson).Select(c => c.countryName).ToList<string>();
		}

		private void comboCity_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if ((sender as ComboBox).SelectedIndex == 0)
				return;

			datagridHotels.Visibility = Visibility.Visible;
		}
	}
}
