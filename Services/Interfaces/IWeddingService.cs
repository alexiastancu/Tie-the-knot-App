using Wedding_Planning_App.Models;

namespace Wedding_Planning_App.Services.Interfaces
{
    public interface IWeddingService
    {
        Task<Wedding> GetWeddingByIdAsync(int id);
        Task<int> AddWeddingAsync(Wedding wedding);
        Task<bool> UpdateWeddingAsync(Wedding wedding);
        Task<bool> DeleteWeddingAsync(Wedding wedding);
        Task<Wedding> GetWeddingByUserIdAsync(int userId);
        Task<List<Wedding>> GetWeddingsByIdsAsync(List<int> weddingIds);

    }
}
