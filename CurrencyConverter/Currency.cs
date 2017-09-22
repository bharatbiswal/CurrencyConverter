using System;
using SQLite;
namespace CurrencyConverter
{
	[Table("Currency")]
	public class Currency
	{
		public string Name { get; set; }
		public float value { get; set; }
		public Currency(string name, float value)
		{
			this.Name = name;
			this.value = value;
		}
		public Currency()
		{
		}
		public override string ToString()
		{
			return Name;
		}

	}
}
