using Wedding_Planning_App.Models;
using Wedding_Planning_App.Services.Interfaces;
using DbConnection = Wedding_Planning_App.Data.DbConnection;

namespace Wedding_Planning_App.Services
{
    public class WeddingService : IWeddingService
    {
        private readonly DbConnection _dbConnection;

        public WeddingService()
        {
            _dbConnection = MauiProgram.CreateMauiApp().Services.GetRequiredService<DbConnection>();
        }


        public async Task<Wedding> GetWeddingByIdAsync(int id)
        {
            await _dbConnection.SetUpDb();

            var wedding = await _dbConnection._connection.Table<Wedding>()
                                                     .Where(w => w.Id == id)
                                                     .FirstOrDefaultAsync();
            return wedding;
        }

        public async Task<int> AddWeddingAsync(Wedding wedding)
        {
            await _dbConnection.SetUpDb();
            await _dbConnection._connection.InsertAsync(wedding);
            return wedding.Id;
        }

        public async Task<bool> UpdateWeddingAsync(Wedding wedding)
        {
            await _dbConnection.SetUpDb();

            await _dbConnection._connection.UpdateAsync(wedding);
            return true;
        }

        public async Task<bool> DeleteWeddingAsync(Wedding wedding)
        {
            await _dbConnection.SetUpDb();

            await _dbConnection._connection.DeleteAsync(wedding);
            return true;
        }

        public async Task<Wedding> GetWeddingByUserIdAsync(int userId)
        {
            await _dbConnection.SetUpDb();
            return await _dbConnection._connection.Table<Wedding>().Where(w => w.UserId == userId)
                                                            .FirstOrDefaultAsync();
        }

        public async Task<List<Wedding>> GetWeddingsByIdsAsync(List<int> weddingIds)
        {
            await _dbConnection.SetUpDb();
            return await _dbConnection._connection.Table<Wedding>()
                                                  .Where(w => weddingIds.Contains(w.Id))
                                                  .ToListAsync();
        }
    }
}
