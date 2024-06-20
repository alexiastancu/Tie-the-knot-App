using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Wedding_Planning_App.Models;

namespace Wedding_Planning_App.Data
{
    //public class DbConnection : IAsyncDisposable
    public class DbConnection
    {
        public SQLiteAsyncConnection _connection;

        public async System.Threading.Tasks.Task SetUpDb()
        {
            if (_connection == null)
            {
                string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Wedding-Planner-db.db3");
                _connection = new SQLiteAsyncConnection(dbPath, SQLiteOpenFlags.Create | SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.SharedCache);
                try
                {
                    await _connection.CreateTableAsync<User>();
                    await _connection.CreateTableAsync<Models.Location>();
                    await _connection.CreateTableAsync<Wedding>();
                    await _connection.CreateTableAsync<Fiances>();
                    await _connection.CreateTableAsync<Guest>();
                    await _connection.CreateTableAsync<WeddingGuestIntermediate>();
                    await _connection.CreateTableAsync<GuestSeat>();
                    await _connection.CreateTableAsync<WeddingTable>();
                    await _connection.CreateTableAsync<Gift>();


                    //await _connection.CreateTableAsync<Vendor>();
                    //await _connection.CreateTableAsync<VendorService>();
                    //await _connection.CreateTableAsync<FinanceItem>();
                    //await _connection.CreateTableAsync<TimelineEvent>();
                    ////await _connection.CreateTableAsync<Models.Task>();
                    //await _connection.CreateTableAsync<MenuItem>();
                    //await _connection.CreateTableAsync<GiftListItem>();
                    //await _connection.CreateTableAsync<GuestGift>();


                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error setting up database: {ex.Message}");
                }

            }
        }


        
    }

}
