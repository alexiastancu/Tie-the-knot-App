using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wedding_Planning_App.Data;
using Wedding_Planning_App.Models;
using Wedding_Planning_App.Services.Interfaces;

namespace Wedding_Planning_App.Services
{
    public class GuestService : IGuestService
    {
        public readonly DbConnection _connection;

        public GuestService()
        {
            _connection = MauiProgram.CreateMauiApp().Services.GetRequiredService<DbConnection>();
        }
        public async Task<int> AddGuestAsync(Guest guest)
        {
            await _connection.SetUpDb();
            return await _connection._connection.InsertAsync(guest);
        }

        public async Task<Guest> GetGuestByUserIdAsync(int userId)
        {
            await _connection.SetUpDb();
            return await _connection._connection.Table<Guest>().Where(g => g.UserId == userId).FirstOrDefaultAsync();
        }

        public async Task<List<Guest>> GetGuestList()
        {
            await _connection.SetUpDb();
            var list = await _connection._connection.Table<Guest>().ToListAsync();
            return list;
        }

        public async Task<List<Guest>> GetGuestsByWeddingIdAsync(int weddingId)
        {
            await _connection.SetUpDb();

            var weddingGuestIntermediates = await _connection._connection.Table<WeddingGuestIntermediate>()
                                                           .Where(wgi => wgi.WeddingId == weddingId)
                                                           .ToListAsync();

            var guestIds = weddingGuestIntermediates.Select(wgi => wgi.GuestId).ToList();


            var guests = new List<Guest>();
            foreach (var guestId in guestIds)
            {
                var guest = await _connection._connection.Table<Guest>().FirstOrDefaultAsync(g => g.Id == guestId);
                if (guest != null)
                {
                    var user = await _connection._connection.Table<User>().FirstOrDefaultAsync(u => u.Id == guest.UserId);
                    guest.User = user; 

                    guests.Add(guest);
                }
            }

            return guests;
        }

        public async Task<List<Guest>> GetUnassignedGuestsByWeddingIdAsync(int weddingId)
        {
            await _connection.SetUpDb();

            // Get all guests invited to the wedding
            var allGuests = await GetGuestsByWeddingIdAsync(weddingId);

            var guestSeats = await _connection._connection.Table<GuestSeat>().ToListAsync();
            var weddingTables = await _connection._connection.Table<WeddingTable>().ToListAsync();

            // Join guest seats with wedding tables to get seats for the specified wedding
            var seatsForWedding = from gs in guestSeats
                                  join wt in weddingTables on gs.TableId equals wt.Id
                                  where wt.WeddingId == weddingId
                                  select gs;

            // Get IDs of guests who are assigned to a seat
            var assignedGuestIds = seatsForWedding.Where(gs => gs.GuestId != 0).Select(gs => gs.GuestId).Distinct().ToList();

            // Filter guests who are not assigned to a seat
            var unassignedGuests = allGuests.Where(g => !assignedGuestIds.Contains(g.Id)).ToList();

            // Populate User property for each guest
            foreach (var guest in unassignedGuests)
            {
                guest.User = await _connection._connection.Table<User>().FirstOrDefaultAsync(u => u.Id == guest.UserId);
            }

            return unassignedGuests;
        }





        public async System.Threading.Tasks.Task<int> UpdateGuestAsync(Guest guest)
        {
            await _connection.SetUpDb();
            return await _connection._connection.UpdateAsync(guest);
        }
    }
}
