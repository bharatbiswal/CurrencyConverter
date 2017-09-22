using System;
using Android.Support.V7.Widget;
using TextView = Android.Widget.TextView;
using Android.Views;
using System.Collections.Generic;
using ImageView = Android.Widget.ImageView;
using Android.Util;
using Android.Preferences;
using Android.Content;

namespace CurrencyConverter
{
	public class CurrencySelectionAdapter : RecyclerView.Adapter
	{
		private List<Currency> currencies;
		public SparseBooleanArray selectedItemArray;
		public CurrencySelectionAdapter()
		{
		}

		public override int ItemCount
		{
			get
			{
				return currencies != null ? currencies.Count : 0;
			}
		}
		public void SetData(List<Currency> list,SparseBooleanArray arr)
		{
			this.currencies = list;
			this.selectedItemArray = arr;
			NotifyDataSetChanged();
		}
		public SparseBooleanArray GetSelectedItemArray()
		{
			return selectedItemArray;
		}

		public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
		{
			if (holder is CurrencyHolder)
			{
				CurrencyHolder currencyHolder = holder as CurrencyHolder;
				currencyHolder.CurrencyName.Text=currencies[position].Name;
				if (selectedItemArray.Get(position))
				{
					currencyHolder.Check.Visibility = ViewStates.Visible;
				}
				else
				{
					currencyHolder.Check.Visibility = ViewStates.Gone;
				}
			}
		}

		public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
		{
			View view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.selection_ltem, parent, false);
			return new CurrencyHolder(view,this);
		}

		class CurrencyHolder : RecyclerView.ViewHolder
		{
			public TextView CurrencyName { get; private set;}
			public ImageView Check;
			CurrencySelectionAdapter parent;
			public CurrencyHolder(View itemView,CurrencySelectionAdapter parent):base(itemView)
			{
				CurrencyName = itemView.FindViewById<TextView>(Resource.Id.currencySymbol);
				Check = itemView.FindViewById<ImageView>(Resource.Id.check);
				this.parent = parent;
				itemView.Click += (sender, e) =>
				  {
					  if (parent.selectedItemArray.Get(AdapterPosition))
					  {
						  Check.Visibility = ViewStates.Gone;
						  parent.selectedItemArray.Delete(AdapterPosition);
					  }
					  else
					  {
						  Check.Visibility = ViewStates.Visible;
						  parent.selectedItemArray.Put(AdapterPosition, true);
					  }
				  };
			}
		}
			
	}
}
