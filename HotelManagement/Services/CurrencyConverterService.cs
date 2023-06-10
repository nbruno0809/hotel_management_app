using Microsoft.Net.Http.Headers;
using System;
using System.Security.Policy;

namespace HotelManagement.Services
{
	public class CurrencyConverterService
	{
		private readonly IConfiguration configuration;

		public CurrencyConverterService(IConfiguration configuration) { 
			this.configuration = configuration;
		}	

		public async Task<double> ConvertAsync(string targetCode,double value)
		{
            var apiUrl = configuration["CurrencyApi:Url"];
            var apiKey = configuration["CurrencyApi:ApiKey"];
            var baseCode = configuration["CurrencyApi:Base"];

            if (apiKey == null || apiUrl == null || baseCode == null)
            {
				throw new Exception($"Missing configurtion data\n" +
				$"{nameof(apiUrl)}: {apiUrl}\n" +
				$"{nameof(apiKey)}: {apiKey}\n" +
				$"{nameof(baseCode)}: {baseCode}");
            }

			Uri url = new Uri($"{apiUrl}?apiKey={apiKey}&base={baseCode}&to={targetCode}&amount={value}");
            var client = new HttpClient();
			var res = await client.GetAsync(url);

			if (res != null && res.IsSuccessStatusCode)
			{ 
				var content = await res.Content.ReadFromJsonAsync<Response>();

				if (content != null)
				{
					return content.Converted;
				} else
				{
					throw new Exception("Parse response from JSON was unsuccesfull");
				}
			} else
			{
				throw new Exception(res.StatusCode.ToString());
			}

		}

		public List<string> GetCodes()
		{
			var codes = configuration["CurrencyApi:Currencies"];
			if (codes == null)
			{
				return new List<string> { "USD","GBP" };
			} else
			{
				return codes.Split(',').ToList();
			}
        }

		private class Response
        {
			public string Base { get; set; } = null!;
			public string To { get; set; } = null!;
			public double Amount { get; set; }
			public double Converted { get; set; }
			public double Rate { get; set; }

			public double? LastUpdate { get; set; }
		}
	}
}
