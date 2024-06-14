using SQLite;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using Wedding_Planning_App.Models;
using User = Wedding_Planning_App.Models.User;
using DbConnection = Wedding_Planning_App.Data.DbConnection;
using Task = System.Threading.Tasks.Task;
using Wedding_Planning_App.Services.Interfaces;

namespace Wedding_Planning_App.Services
{

    public class UserService : IUserService
    {
        public readonly DbConnection _connection;
        

        public UserService()
        {
            _connection = MauiProgram.CreateMauiApp().Services.GetRequiredService<DbConnection>();
        }

        public async Task<int> CreateAsync(User user, string password = "0")
        {
            _connection.SetUpDb();

            if (password != "0")
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);

            // Insert the user into the database
            var success = await _connection._connection.InsertAsync(user);
            return success;
        }




        public async Task<User> FindByEmailAsync(string email)
        {
            await _connection.SetUpDb();
            User user = await _connection._connection.Table<User>()
                                                     .Where(u => u.Email == email)
                                                     .FirstOrDefaultAsync();
            return user;
        }


        public async Task DeleteAsync(User user)
        {
            _connection.SetUpDb();
            await _connection._connection.DeleteAsync(user);
        }

        public async Task<int> UpdateAsync(User user)
        {
            _connection.SetUpDb();
            var success = await _connection._connection.UpdateAsync(user);
            return success;
        }

        public async Task<List<User>> GetUserList()
        {
            _connection.SetUpDb();
            var userList = await _connection._connection.Table<User>().ToListAsync();
            return userList;
        }

        public async Task<User> GetUserById(int userId)
        {
            _connection.SetUpDb();
            var user = await _connection._connection.Table<User>()
                                                     .Where(u => u.Id == userId)
                                                     .FirstOrDefaultAsync();
            return user;
        }


        public async Task<int> UserHasWeddingAsync(User user)
        {
            _connection.SetUpDb();
            var wedding = await _connection._connection.Table<Wedding>()
                                                       .Where(w => w.UserId == user.Id)
                                                       .FirstOrDefaultAsync();
            if (wedding == null)
            {
                return 0;
            }
            return wedding.Id;
        }

        public async Task<int> AddPasswordToUser(User user, string password)
        {
            await _connection.SetUpDb();
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);

            return await _connection._connection.UpdateAsync(user);

        }
    }
}
