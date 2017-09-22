
using System;

using Android.App;
using Android.Content;
using Android.OS;
using System.Collections.Generic;

namespace CurrencyConverter
{
	[Service(Label = "SyncService")]
	[IntentFilter(new String[] { "com.yourname.SyncService" })]
	public class SyncService : IntentService
	{
		IBinder binder;

		protected override void OnHandleIntent(Intent intent)
		{
			// Perform your service logic here
			SyncCurrencies();
		}

		public override IBinder OnBind(Intent intent)
		{
			binder = new SyncServiceBinder(this);
			return binder;
		}

		private async void SyncCurrencies()
		{

			Console.WriteLine("SyncService Started........");
			Dictionary<string,float>dictionary= await new CurrencyRepository().GetCurrencyAsync("http://api.fixer.io/latest?base=EUR");
			if (dictionary != null && dictionary.Count > 0)
			{
                    List<Currency> curreinecies = new List<Currency>();
					foreach (KeyValuePair<string, float> entry in dictionary)
					{
						curreinecies.Add(new Currency(entry.Key, entry.Value));
					}
				    await new DbRepository().InsertCurrencies(curreinecies);
				    Console.WriteLine(curreinecies.Count + "Items Written to Data base");
				   Console.WriteLine("Next Sync Will take place in" + CurrencyManager.Instance.GetSyncTime(this)+" Minutes");
			}
		}
	}

	public class SyncServiceBinder : Binder
	{
		readonly SyncService service;

		public SyncServiceBinder(SyncService service)
		{
			this.service = service;
		}

		public SyncService GetSyncService()
		{
			return service;
		}
	}
}
