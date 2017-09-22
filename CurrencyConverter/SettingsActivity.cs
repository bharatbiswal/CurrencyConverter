
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V7.App;
using Toolbar = Android.Support.V7.Widget.Toolbar;

namespace CurrencyConverter
{
	[Activity(Label = "SettingsActivity")]
	public class SettingsActivity : AppCompatActivity
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.settings_layout);
			Toolbar toolBar = FindViewById<Toolbar>(Resource.Id.toolbar);
			SetSupportActionBar(toolBar);
			SupportActionBar.Title = "Settings";
			SupportActionBar.SetDisplayHomeAsUpEnabled(true);
			FindViewById(Resource.Id.syncSettings).Click += (sender,e) => { openSyncSettingDialog();};
			FindViewById(Resource.Id.selectionSettings).Click += (sender, e) => {
													var intent = new Intent(this, typeof(SelectionActivity));
													StartActivity(intent);
												
			                                     };

			// Create your application here
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

		private void openSyncSettingDialog()
		{
			new SyncDialogFragment().Show(SupportFragmentManager,"SYNC");
		}
	}
}
