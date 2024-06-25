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
    public class GiftService : IGiftService
    {
        private readonly DbConnection _connection;

        public GiftService(DbConnection dbConnection)
        {
            _connection = dbConnection;
        }

        public async Task<List<Gift>> GetGiftsByWeddingIdAsync(int weddingId)
        {
            await _connection.SetUpDb();
            var list = await _connection._connection.Table<Gift>().ToListAsync();
            return await _connection._connection.Table<Gift>().Where(g => g.WeddingId == weddingId).ToListAsync();
        }

        public async Task AddGiftAsync(Gift gift)
        {
            await _connection.SetUpDb();
            await _connection._connection.InsertAsync(gift);
            var giftList = await _connection._connection.Table<Gift>().ToListAsync();
        }

        public async Task UpdateGiftAsync(Gift gift)
        {
            await _connection.SetUpDb();
            await _connection._connection.UpdateAsync(gift);
        }

        public async Task DeleteGiftAsync(int giftId)
        {
            await _connection.SetUpDb();
            await _connection._connection.DeleteAsync<Gift>(giftId);
        }
    }
}
