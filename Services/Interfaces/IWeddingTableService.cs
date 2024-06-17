using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wedding_Planning_App.Models;

namespace Wedding_Planning_App.Services.Interfaces
{
    public interface IWeddingTableService
    {
        Task<List<WeddingTable>> GetTablesByWeddingIdAsync(int weddingId);
        Task AddWeddingTableAsync(WeddingTable weddingTable);
        Task UpdateWeddingTableAsync(WeddingTable weddingTable);
        Task<WeddingTable> GetTableByIdAsync(int tableId);
    }
}
