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
    public class WeddingTableService : IWeddingTableService
    {
        private readonly DbConnection _connection;

        public WeddingTableService(DbConnection connection)
        {
            _connection = connection;
        }

        public async Task<List<WeddingTable>> GetTablesByWeddingIdAsync(int weddingId)
        {
            await _connection.SetUpDb();
            return await _connection._connection.Table<WeddingTable>()
                                      .Where(wt => wt.WeddingId == weddingId)
                                      .ToListAsync();
        }

        public async Task AddWeddingTableAsync(WeddingTable weddingTable)
        {
            await _connection.SetUpDb();
            await _connection._connection.InsertAsync(weddingTable);
        }

        public async Task UpdateWeddingTableAsync(WeddingTable weddingTable)
        {
            await _connection.SetUpDb();
            await _connection._connection.UpdateAsync(weddingTable);
        }

        public async Task<WeddingTable> GetTableByIdAsync(int tableId)
        {
            await _connection.SetUpDb();

            var weddingTable = await _connection._connection.Table<WeddingTable>()
                                          .Where(wt => wt.Id == tableId)
                                          .FirstOrDefaultAsync();

            if (weddingTable != null)
            {
                var seats = await _connection._connection.Table<GuestSeat>()
                                          .Where(gs => gs.TableId == tableId)
                                          .ToListAsync();
                weddingTable.Seats = new List<GuestSeat>(seats);
            }

            return weddingTable;
        }
    }
}
