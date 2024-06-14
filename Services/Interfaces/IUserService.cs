using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wedding_Planning_App.Models;
using Task = System.Threading.Tasks.Task;

namespace Wedding_Planning_App.Services.Interfaces
{
    public interface IUserService
    {
        public Task<User> FindByEmailAsync(string email);
        public Task<int> CreateAsync(User user, string password = "0");
        public Task DeleteAsync(User user);
        public Task<int> UpdateAsync(User user);
        public Task<List<User>> GetUserList();
        Task<int> UserHasWeddingAsync(User user);
        public Task<User> GetUserById(int userId);

        public Task<int> AddPasswordToUser(User user, string password);

    }
}
