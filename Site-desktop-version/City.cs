using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Site_desktop_version
{
    class City
	{
		public int id { get; set; }
		public string cityName { get; set; }
		public int countryId { get; set; }
		public override string ToString()
		{
			return cityName;
		}
	}
}
