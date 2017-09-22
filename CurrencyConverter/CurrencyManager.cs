using System;
using System.Collections.Generic;
using Android.Content;
using Android.Preferences;

namespace CurrencyConverter
{
	public sealed class CurrencyManager{
	private static volatile CurrencyManager instance;
	private static object syncRoot = new Object();
	private CurrencyManager() { }
	public static CurrencyManager Instance
		{
			get
			{					
				if (instance == null)
			
				{
					lock (syncRoot)
					{
						if (instance == null)
							instance = new CurrencyManager();
					}
				}
				return instance;
			}
		}
		public bool HasCurrencySelected(Context mContext)
	    {
			ISharedPreferences prefs= PreferenceManager.GetDefaultSharedPreferences(mContext);
			return prefs.GetBoolean("HAS_SELECTION", false);
	    }
		public void PutCurrrencySelection(Context mContext, bool value)
		{
			ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(mContext);
			prefs.Edit().PutBoolean("HAS_SELECTION", value).Commit();

		}

		public void SaveSyncTime(long syncTime,Context context,int checkedId)
		{
			ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(context);
			prefs.Edit().PutLong("SYNC_TIME", syncTime).Commit();
			prefs.Edit().PutInt("SELECT_ID", checkedId).Commit();
		}
		public long GetSyncTime(Context context)
		{
			ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(context);
			return prefs.GetLong("SYNC_TIME", 5);
		}
		public int GetselectedSyncId(Context context)
		{
			ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(context);
			return prefs.GetInt("SELECT_ID", Resource.Id.sync5);
		}

	}

 
}
