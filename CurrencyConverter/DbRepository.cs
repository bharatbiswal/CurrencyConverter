using System;
using SQLite;
using System.IO;
using System.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CurrencyConverter
{
	public class DbRepository
	{
		private readonly SQLiteAsyncConnection sqlConnection;
		public  DbRepository()
		{
			string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "currency.db");
            sqlConnection = new SQLiteAsyncConnection(dbPath);
            sqlConnection.CreateTableAsync<Currency>();
			sqlConnection.CreateTableAsync<Selection>();
		}
		public async Task<bool> InsertCurrency(Currency currency)
		{
			Currency c = await sqlConnection.Table<Currency>().Where(cw => cw.Name == currency.Name).FirstAsync();
			if (c == null)
			{
				await sqlConnection.InsertAsync(currency);
			}
			else
			{
				c.value = currency.value;
				await sqlConnection.UpdateAsync(c);
			}
			return true;
		}
		public async Task<bool>  InsertCurrencies(List<Currency> currenclies)
		{
            await sqlConnection.ExecuteAsync("DELETE FROM Currency");
			await sqlConnection.InsertAllAsync(currenclies);
			return true;

		}

		public async Task<List<Currency>> GetAllCurrencies()
		{
			return await sqlConnection.Table<Currency>().ToListAsync();
		}
		public async Task<Currency> GetCurrencyByname(string name)
		{
			return await sqlConnection.Table<Currency>().Where(cw => cw.Name == name).FirstAsync();
		}
		public async Task<bool> SaveSelection(List<Selection>selections)
		{
			await sqlConnection.ExecuteAsync("DELETE FROM SELECTION");
			await sqlConnection.InsertAllAsync(selections);
			return true;

		}
		public async Task<int> GetSelectionCount()
		{
			return await sqlConnection.Table<Selection>().CountAsync();
		}
		public async Task<List<Selection>> GetAllSelection()
		{
			return await sqlConnection.Table<Selection>().ToListAsync();
				
		}
	}
}
