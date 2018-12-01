using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Site_desktop_version
{
    class API
    {
		private string responseJson;

		string Request(string param)
		{
			WebRequest request = WebRequest.Create("http://travel.itstep.fun/api/");
			request.Method = "POST"; // для отправки используется метод Post
									 // данные для отправки
			string data = "token=ps_rpo_1&param=" + param;
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

			return responseJson;
		}
		public List<Country> getCountries()
		{
			responseJson = Request("getCountries");
			return JsonConvert.DeserializeObject<List<Country>>(responseJson);
		}
		public List<City> getCities(int countryId)
		{
			responseJson = Request("getCities&country=" + countryId);
			return JsonConvert.DeserializeObject<List<City>>(responseJson);
		}
	}
}
