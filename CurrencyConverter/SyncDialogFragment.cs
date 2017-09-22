using System;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using AlarmManager = Android.App.AlarmManager;
using Context = Android.Content.Context;
using Intent = Android.Content.Intent;
using PendingIntent = Android.App.PendingIntent;
using AlarmType = Android.App.AlarmType;
using Android.OS;

namespace CurrencyConverter
{
	public class SyncDialogFragment:DialogFragment
	{
		public SyncDialogFragment()
		{
		}
		public override Android.Views.View OnCreateView(Android.Views.LayoutInflater inflater, Android.Views.ViewGroup container, Android.OS.Bundle savedInstanceState)
		{
			 base.OnCreateView(inflater, container, savedInstanceState);
			var view = inflater.Inflate(Resource.Layout.sync_dialog_layout, container, false);
			RadioGroup radioGroup = view.FindViewById<RadioGroup>(Resource.Id.syncGroup);
			radioGroup.FindViewById<RadioButton>(CurrencyManager.Instance.GetselectedSyncId(Activity)).Checked = true;
			view.FindViewById(Resource.Id.cancel).Click += (sender, e) => { Dismiss(); };
						view.FindViewById(Resource.Id.save).Click += (sender, e) =>
									{
							// store as unit minute
							long syncTime = 5;
							switch (radioGroup.CheckedRadioButtonId)
							{
								case Resource.Id.sync5:
									syncTime = 5;
									break;
								case Resource.Id.sync30:
									syncTime = 30;
									break;
								case Resource.Id.sync1Hour:
									syncTime = 60;
									break;
								case Resource.Id.sync12hour:
									syncTime = 720;
									break;
								case Resource.Id.sync24Hour:
									syncTime = 1440;
									break;

							}
							CurrencyManager.Instance.SaveSyncTime(syncTime, Activity, radioGroup.CheckedRadioButtonId);
							StartSyncService();
							Dismiss();			
						};
						return view;
			}

		public override void OnStart()
		{
			base.OnStart();
			WindowManagerLayoutParams layouparam = Dialog.Window.Attributes;
			layouparam.Width = ViewGroup.LayoutParams.MatchParent;
			layouparam.Height = ViewGroup.LayoutParams.MatchParent;
			Dialog.Window.Attributes = layouparam;
		}
	private void StartSyncService()
	{
		long syncDurationInMinutes = CurrencyManager.Instance.GetSyncTime(Activity) * 60000;
		Console.WriteLine("Sync Will Start after" + syncDurationInMinutes);
		var alarmMgr = (AlarmManager)Activity.GetSystemService(Context.AlarmService);
		var intent = new Intent(Activity, typeof(SyncService));
		var alarmIntent = PendingIntent.GetService(Activity, 0, intent, 0);
		alarmMgr.Cancel(alarmIntent);
		alarmMgr.SetInexactRepeating(AlarmType.ElapsedRealtimeWakeup, SystemClock.ElapsedRealtime() + syncDurationInMinutes, syncDurationInMinutes, alarmIntent);		
				
		}


	}
}
