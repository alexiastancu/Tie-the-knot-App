using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wedding_Planning_App.Models;
using Location = Wedding_Planning_App.Models.Location;
using Task = System.Threading.Tasks.Task;

namespace Wedding_Planning_App.Services.Interfaces
{
    public interface ILocationService
    {
        public Task<List<Location>> GetLocations();
        public Task<Location> GetLocationById(int locationId);
        public Task<int> CreateLocationAsync(Location location);
        public Task<int> UpdateLocationAsync(Location location);
        public Task DeleteLocationAsync(Location location);
    }
}
