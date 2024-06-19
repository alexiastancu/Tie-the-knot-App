using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wedding_Planning_App.Data;
using Wedding_Planning_App.Models;

namespace Wedding_Planning_App.Services.Interfaces
{
    public class GuestSeatService : IGuestSeatService
    {
        private readonly DbConnection _connection;

        public GuestSeatService(DbConnection dbConnection)
        {
            _connection = dbConnection;
        }

        public async Task AddGuestSeatAsync(GuestSeat guestSeat)
        {
            await _connection.SetUpDb();
            await _connection._connection.InsertAsync(guestSeat);

        }

        public async Task<GuestSeat> GetGuestSeatByGuestIdAsync(int guestId)
        {
            await _connection.SetUpDb();
            return await _connection._connection.Table<GuestSeat>()
                                    .Where(gs => gs.GuestId == guestId)
                                    .FirstOrDefaultAsync();
        }

        public async Task<List<GuestSeat>> GetSeatsByTableIdAsync(int tableId)
        {
            await _connection.SetUpDb();

            var guestSeats = await _connection._connection.Table<GuestSeat>()
                                    .Where(gs => gs.TableId == tableId)
                                    .ToListAsync();

            foreach (var guestSeat in guestSeats)
            {
                if (guestSeat.GuestId != 0)
                {
                    guestSeat.Guest = await _connection._connection.Table<Guest>()
                                                .Where(g => g.Id == guestSeat.GuestId)
                                                .FirstOrDefaultAsync();

                    if (guestSeat.Guest != null)
                    {
                        guestSeat.Guest.User = await _connection._connection.Table<User>()
                                                    .Where(u => u.Id == guestSeat.Guest.UserId)
                                                    .FirstOrDefaultAsync();
                    }
                }
            }

            return guestSeats;
        }


        public async Task UpdateGuestSeatAsync(GuestSeat guestSeat)
        {
            await _connection.SetUpDb();
            await _connection._connection.UpdateAsync(guestSeat);
        }
    }
}
