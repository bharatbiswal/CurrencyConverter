
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;

namespace CurrencyConverter
{
	[Activity(Label = "Currency", MainLauncher=true, Icon = "@drawable/currency_icon",Theme="@style/SplashTheme")]

	public class SplashActivit: Activity
	{
		DbRepository repo;
		protected  override void OnCreate(Bundle savedInstanceState)
		{
			 base.OnCreate(savedInstanceState);
             repo = new DbRepository();
			 GetAllCurrency();
		}
		public async void GetAllCurrency()
		{
				string url = "http://api.fixer.io/latest?base=EUR";
				Dictionary<string, float> currencyMap = await new CurrencyRepository().GetCurrencyAsync(url);
			   if (currencyMap != null)
			   {

				List<Currency> curreinecies = new List<Currency>();
				foreach (KeyValuePair<string, float> entry in currencyMap)
				{
					curreinecies.Add(new Currency(entry.Key, entry.Value));
				}
				await repo.InsertCurrencies(curreinecies);
				var intent = new Intent(this, typeof(MainActivity));
				StartActivity(intent);
				StartSyncService();
				Finish();
			  }
			else
			{
                StartSyncService();
				var curriencies=await repo.GetAllCurrencies();
				if (curriencies != null && curriencies.Count > 0)
				{
					var intent = new Intent(this, typeof(MainActivity));
					StartActivity(intent);
					Finish();
				}
				else
				{
					Toast.MakeText(this, "Check your internate connection please", ToastLength.Short).Show();
				}
			}
			  
		}
		private void StartSyncService()
		{
			long syncDurationInMinutes=CurrencyManager.Instance.GetSyncTime(this)*60000;
			var alarmMgr = (AlarmManager)GetSystemService(Context.AlarmService);
			Intent intent = new Intent(this, typeof(SyncService));
			var alarmIntent = PendingIntent.GetService(this, 0, intent, 0);
			alarmMgr.SetInexactRepeating(AlarmType.ElapsedRealtimeWakeup, SystemClock.ElapsedRealtime() + syncDurationInMinutes, syncDurationInMinutes, alarmIntent);
            
		}


						
	}


}
