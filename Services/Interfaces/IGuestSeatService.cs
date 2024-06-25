using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wedding_Planning_App.Models;

namespace Wedding_Planning_App.Services.Interfaces
{
    public interface IGuestSeatService
    {
        Task AddGuestSeatAsync(GuestSeat guestSeat);
        Task<List<GuestSeat>> GetSeatsByTableIdAsync(int tableId);
        Task UpdateGuestSeatAsync(GuestSeat guestSeat);
        Task<GuestSeat> GetGuestSeatByGuestIdAsync(int guestId, int weddingId);
    }
}
