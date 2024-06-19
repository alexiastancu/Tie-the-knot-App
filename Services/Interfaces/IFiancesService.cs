using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wedding_Planning_App.Models;

namespace Wedding_Planning_App.Services.Interfaces
{
    public interface IFiancesService
    {
        Task<int> AddFiancesAsync(Fiances fiances);
        Task<Fiances> GetFiancesByUserIdAsync(int userId);

        Task<Fiances> GetFiancesByIdAsync(int id);
        Task<int> UpdateFiancesAsync(Fiances fiances);
    }
}
