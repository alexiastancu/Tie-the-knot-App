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
    public class FiancesService : IFiancesService
    {
        private readonly DbConnection _dbConnection;

        public FiancesService()
        {
            _dbConnection = MauiProgram.CreateMauiApp().Services.GetRequiredService<DbConnection>();

        }

        public async Task<int> AddFiancesAsync(Fiances fiances)
        {
            await _dbConnection.SetUpDb();
            return await _dbConnection._connection.InsertAsync(fiances);
        }

        public async Task<Fiances> GetFiancesByUserIdAsync(int userId)
        {
            await _dbConnection.SetUpDb();
            return await _dbConnection._connection.Table<Fiances>()
                .Where(f => f.UserId == userId)
                .FirstOrDefaultAsync();

        }
    }

}
