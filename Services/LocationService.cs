using System.Collections.Generic;
using System.Threading.Tasks;
using Wedding_Planning_App.Models;
using Wedding_Planning_App.Data;
using Location = Wedding_Planning_App.Models.Location;
using Task = System.Threading.Tasks.Task;
using Wedding_Planning_App.Services.Interfaces;

namespace Wedding_Planning_App.Services
{
    public class LocationService : ILocationService
    {
        private readonly DbConnection _dbConnection;

        public LocationService(DbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<int> CreateLocationAsync(Location location)
        {
            await _dbConnection.SetUpDb();
            await _dbConnection._connection.InsertAsync(location);
            return location.LocationID;
        }

        public async Task DeleteLocationAsync(Location location)
        {
            await _dbConnection.SetUpDb();
            await _dbConnection._connection.DeleteAsync(location);
        }

        public async Task<Location> GetLocationById(int locationId)
        {
            await _dbConnection.SetUpDb();
            return await _dbConnection._connection.Table<Location>()
                                                 .Where(l => l.LocationID == locationId)
                                                 .FirstOrDefaultAsync();
        }

        public async Task<List<Location>> GetLocations()
        {
            await _dbConnection.SetUpDb();
            return await _dbConnection._connection.Table<Location>().ToListAsync();
        }

        public async Task<int> UpdateLocationAsync(Location location)
        {
            await _dbConnection.SetUpDb();
            return await _dbConnection._connection.UpdateAsync(location);
        }
    }
}
