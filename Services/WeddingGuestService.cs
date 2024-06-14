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
    public class WeddingGuestService : IWeddingGuestService
    {
        public readonly DbConnection _connection;
        public WeddingGuestService()
        {
            _connection = MauiProgram.CreateMauiApp().Services.GetRequiredService<DbConnection>();
        }
        public async System.Threading.Tasks.Task AddGuestToWeddingAsync(int weddingId, int guestId)
        {
            _connection.SetUpDb();
            var weddingGuest = new WeddingGuestIntermediate
            {
                WeddingId = weddingId,
                GuestId = guestId                
            };

            await _connection._connection.InsertAsync(weddingGuest);
        }

        public async Task<bool> IsGuestAlreadyAddedToWeddingAsync(int weddingId, int guestId)
        {
            await _connection.SetUpDb();

            var existingGuest = await _connection._connection.Table<WeddingGuestIntermediate>()
                                        .Where(wg => wg.WeddingId == weddingId && wg.GuestId == guestId)
                                        .FirstOrDefaultAsync();

            return existingGuest != null;
        }

        public async Task<List<WeddingGuestIntermediate>> GetGuestListAsync()
        {
            _connection.SetUpDb();
            var list = await _connection._connection.Table<WeddingGuestIntermediate>().ToListAsync();
            return list;
        }

        public async Task<List<int>> GetWeddingsByGuestIdAsync(int guestId)
        {
            await _connection.SetUpDb();
            var guestWeddings = await _connection._connection.Table<WeddingGuestIntermediate>()
                                                  .Where(wg => wg.GuestId == guestId)
                                                  .ToListAsync();
            var guestWeddingIds = guestWeddings.Select(wg => wg.WeddingId).ToList();
            return guestWeddingIds;
        }
    }
}
