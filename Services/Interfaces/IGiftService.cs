using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wedding_Planning_App.Models;

namespace Wedding_Planning_App.Services.Interfaces
{
    public interface IGiftService
    {
        Task<List<Gift>> GetGiftsByWeddingIdAsync(int weddingId);
        Task AddGiftAsync(Gift gift);
        Task UpdateGiftAsync(Gift gift);
        Task DeleteGiftAsync(int giftId);
    }
}
