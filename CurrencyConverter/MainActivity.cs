using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Content;
using Android.Widget;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Threading;

namespace CurrencyConverter
{
	[Activity(Label = "Android Demo",Theme="@style/MyTheme")]
	public class MainActivity : AppCompatActivity
	{

		private TextView convertedvalue;
		private DbRepository repo;
		private List<Currency> curriencies;
		private Spinner fromCurrencySpinner;
		private Spinner toCurrencySpinner;
		private List<Selection> selections;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.Main);
			var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
			SetSupportActionBar(toolbar);
			repo = new DbRepository();
			convertedvalue = FindViewById<TextView>(Resource.Id.convertedCurrencyValue);
		    fromCurrencySpinner = FindViewById<Spinner>(Resource.Id.fromCurrencySpinner);
	        toCurrencySpinner = FindViewById<Spinner>(Resource.Id.toCurrencySpinner);
			EditText fromValue = FindViewById<EditText>(Resource.Id.fromvalue);
			Button convert = (Button)FindViewById(Resource.Id.convert);
			convert.Click += (sender, e) =>
			  {
				  float value = 0.0f;
				  float.TryParse(fromValue.Text,out value);
				  if (Math.Abs(value) > 0 && curriencies!=null && curriencies.Count>0)
				  {
					  // Call to Convert
					  float fromvalue = curriencies[fromCurrencySpinner.SelectedItemPosition].value;
					  float tovalue = curriencies[toCurrencySpinner.SelectedItemPosition].value;
					  var cvalue = (tovalue / fromvalue) * (value);
					  convertedvalue.Text = cvalue + curriencies[toCurrencySpinner.SelectedItemPosition].Name;
				  }
			  };
		}


		public override bool OnCreateOptionsMenu(Android.Views.IMenu menu)
		{
	        base.OnCreateOptionsMenu(menu);
			MenuInflater.Inflate(Resource.Menu.main, menu);
			return true;
		}
		public override bool OnOptionsItemSelected(Android.Views.IMenuItem item)
		{
			 base.OnOptionsItemSelected(item);
			if (item.ItemId == Resource.Id.settings)
			{
				GotoSettingsPage();
				return true;
			}
			return false;
		}
		private void ShowNoSelectionDialog()
		{
			var builder = new Android.Support.V7.App.AlertDialog.Builder(this)
								   .SetTitle("No Selection")
								   .SetMessage("You have not selected any currency. Go to setting and select currency")
			                         .SetPositiveButton("OK", delegate { Console.WriteLine("Yes"); GotoSelectionPage(); })
									 .SetNegativeButton("Cancel", (sender, e) => { Console.WriteLine("Cancel"); })
									 .Create();
			builder.SetCanceledOnTouchOutside(false);

			builder.Show();

		}

		private void GotoSettingsPage()
		{
			var intent = new Intent(this, typeof(SettingsActivity));
			StartActivity(intent);
		}
		private void GotoSelectionPage()
		{
			var intent = new Intent(this, typeof(SelectionActivity));
			StartActivity(intent);
		}
		private  async void GetAllSavedCurrencies()
		{
			List<Currency>allCurrency = await repo.GetAllCurrencies();
			curriencies = new List<Currency>();
			foreach (Currency c in allCurrency)
			{
				foreach (Selection s in selections)
				{
					if (c.Name.Equals(s.name))
					{
						curriencies.Add(c);
					}
				}
			}
			Console.WriteLine("Getting currencies...." + curriencies.Count);
			fromCurrencySpinner.Adapter = new ArrayAdapter<Currency>(this, Resource.Layout.spinner_item, curriencies);
            toCurrencySpinner.Adapter = new ArrayAdapter<Currency>(this, Resource.Layout.spinner_item, curriencies);

		}
		private async void GetSelection(){
			selections=await repo.GetAllSelection();
			if (selections.Count == 0)
			{
				fromCurrencySpinner.Adapter = null;
				toCurrencySpinner.Adapter = null;
				curriencies = null;
				ShowNoSelectionDialog();
			}
			else
			{
				GetAllSavedCurrencies();
			}
		}

		protected override void OnResume()
		{
			base.OnResume();
			GetSelection();
		}





	}
}

