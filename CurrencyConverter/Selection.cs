using System;
using SQLite;
namespace CurrencyConverter
{
    [Table("SELECTION")]
	public class Selection
	{
		public bool selected { get; set; }
		public string name { get; set; }
		public Selection(bool selected,string name)
		{
			this.selected = selected;
			this.name = name;
		}
		public Selection()
		{
		}
	}
}
