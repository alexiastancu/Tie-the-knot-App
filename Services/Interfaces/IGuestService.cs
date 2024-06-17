using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wedding_Planning_App.Models;

namespace Wedding_Planning_App.Services.Interfaces
{
    public interface IGuestService
    {
        Task<int> AddGuestAsync(Guest guest);
        System.Threading.Tasks.Task<Guest> GetGuestByUserIdAsync(int userId);
        Task<List<Guest>> GetGuestList();
        Task<int> UpdateGuestAsync(Guest guest);
        Task<List<Guest>> GetGuestsByWeddingIdAsync(int weddingId);
        Task<List<Guest>> GetUnassignedGuestsByWeddingIdAsync(int weddingId);

    }
}
