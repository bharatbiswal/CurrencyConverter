using System;
using System.Collections;
using System.Collections.Generic;

namespace CurrencyConverter
{
	public class CurrencyResponse
	{
		public string date { get; set; }
		public Dictionary<string, float> rates { get; set; }
	}
}
