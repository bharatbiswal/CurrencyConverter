
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using Android.Util;

namespace CurrencyConverter
{
	[Activity(Label = "SecondScreen")]
public class SelectionActivity : AppCompatActivity
	{
		private CurrencySelectionAdapter adapter;
		private List<Currency> curriencies;
		private DbRepository repo;
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.selection_layout);
			var toolBar = FindViewById<Toolbar>(Resource.Id.toolbar);
			SetSupportActionBar(toolBar);
			SupportActionBar.SetDisplayHomeAsUpEnabled(true);
			SupportActionBar.Title = "Select Currency";
			var dataView = FindViewById<RecyclerView>(Resource.Id.currencyListView);
			adapter = new CurrencySelectionAdapter();
			dataView.SetAdapter(adapter);
			dataView.SetLayoutManager(new LinearLayoutManager(this));
			repo = new DbRepository();
			Button saveButton = FindViewById<Button>(Resource.Id.save);
			saveButton.Click += async (sender, e) =>
			  {
				  var selectedIdxs = adapter.GetSelectedItemArray();
				if (selectedIdxs != null && selectedIdxs.Size() >= 2)
				  {
					  List<Selection> selections = new List<Selection>();
					  for (int i = 0; i < selectedIdxs.Size(); i++)
					  {
						  bool value = selectedIdxs.Get(selectedIdxs.KeyAt(i));
						  if (value)
						  {
							  Currency c = curriencies[selectedIdxs.KeyAt(i)];
							  Selection selection = new Selection(true, c.Name);
							  selections.Add(selection);
						  }
					  }
					  await repo.SaveSelection(selections);
					  Finish();
				  }
				  else
				  {
					  Toast.MakeText(this, "Please select at leaset two currency from the list", ToastLength.Short).Show();
				  }
				 
			  };
			 GetAllCurrencies();

		}
		public override bool OnOptionsItemSelected(IMenuItem item)
		{
			if (item.ItemId == Android.Resource.Id.Home)
			{
				Finish();
				return true;
			}
			return base.OnOptionsItemSelected(item);
		}
		private async void GetAllCurrencies()
		{
			curriencies = await repo.GetAllCurrencies();
			List<Selection>selections=await repo.GetAllSelection();
			SparseBooleanArray booleanArray = new SparseBooleanArray();
			foreach(Currency c in curriencies){
				foreach (Selection s in selections)
				{
					if (s.name.Equals(c.Name))
					{
						booleanArray.Put(curriencies.IndexOf(c), true);
					}
				}
			}
			adapter.SetData(curriencies,booleanArray);



		}
					


	}
}
