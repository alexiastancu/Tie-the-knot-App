using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wedding_Planning_App.Data;
using Wedding_Planning_App.Models;
using Wedding_Planning_App.Services.Interfaces;

namespace Wedding_Planning_App.Services
{
    public class GuestService : IGuestService
    {
        public readonly DbConnection _connection;

        public GuestService()
        {
            _connection = MauiProgram.CreateMauiApp().Services.GetRequiredService<DbConnection>();
        }
        public async Task<int> AddGuestAsync(Guest guest)
        {
            await _connection.SetUpDb();
            return await _connection._connection.InsertAsync(guest);
        }

        public async Task<Guest> GetGuestByUserIdAsync(int userId)
        {
            await _connection.SetUpDb();
            return await _connection._connection.Table<Guest>().Where(g => g.UserId == userId).FirstOrDefaultAsync();
        }

        public async Task<List<Guest>> GetGuestList()
        {
            await _connection.SetUpDb();
            var list = await _connection._connection.Table<Guest>().ToListAsync();
            return list;
        }

        public async System.Threading.Tasks.Task<int> UpdateGuestAsync(Guest guest)
        {
            await _connection.SetUpDb();
            return await _connection._connection.UpdateAsync(guest);
        }
    }
}
