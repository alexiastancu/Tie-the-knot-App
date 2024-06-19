using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wedding_Planning_App.Models;

namespace Wedding_Planning_App.Services.Interfaces
{
    public interface IWeddingGuestService
    {
        public System.Threading.Tasks.Task AddGuestToWeddingAsync(int weddingId, int guestId);
        public Task<List<WeddingGuestIntermediate>> GetGuestListAsync();
        Task<List<int>> GetWeddingsByGuestIdAsync(int guestId);
        public Task<bool> IsGuestAlreadyAddedToWeddingAsync(int weddingId, int guestId);
        public Task RemoveGuestFromWeddingAsync(int weddingId, int guestId);
    }
}
