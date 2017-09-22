using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading;

namespace CurrencyConverter
{
	public class CurrencyRepository
	{
		/*
		 * Asyn Method to Get All Currency Conversion Rates
		 * 
		 * */
		public async Task<Dictionary<string,float>> GetCurrencyAsync(string url)
		{
			var client = new HttpClient();
			try
			{
				var result = await client.GetAsync(url);
				if (result.IsSuccessStatusCode)
				{
					var content = await result.Content.ReadAsStringAsync();
					CurrencyResponse res = JsonConvert.DeserializeObject<CurrencyResponse>(content);
					return res.rates;
				}
				else
				{
					Console.WriteLine("Network Call Fail");
					return null;
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.StackTrace);
				return null;
			}

		}
	}
}
