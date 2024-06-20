using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wedding_Planning_App.Data;
using Wedding_Planning_App.Data.Enums;
using Wedding_Planning_App.Models;
using Wedding_Planning_App.Services.Interfaces;

namespace Wedding_Planning_App.Services
{
    public class WeddingGuestService : IWeddingGuestService
    {
        public readonly DbConnection _connection;
        public WeddingGuestService()
        {
            _connection = MauiProgram.CreateMauiApp().Services.GetRequiredService<DbConnection>();
        }
        public async System.Threading.Tasks.Task AddGuestToWeddingAsync(int weddingId, int guestId)
        {
            _connection.SetUpDb();
            var weddingGuest = new WeddingGuestIntermediate
            {
                WeddingId = weddingId,
                GuestId = guestId,
                InvitationStatus = InvitationStatus.Pending
            };

            await _connection._connection.InsertAsync(weddingGuest);
        }

        public async Task<bool> IsGuestAlreadyAddedToWeddingAsync(int weddingId, int guestId)
        {
            await _connection.SetUpDb();

            var existingGuest = await _connection._connection.Table<WeddingGuestIntermediate>()
                                        .Where(wg => wg.WeddingId == weddingId && wg.GuestId == guestId)
                                        .FirstOrDefaultAsync();

            return existingGuest != null;
        }

        public async Task<List<WeddingGuestIntermediate>> GetGuestListAsync()
        {
            _connection.SetUpDb();
            var list = await _connection._connection.Table<WeddingGuestIntermediate>().ToListAsync();
            return list;
        }

        public async Task<List<WeddingGuestIntermediate>> GetWeddingsByGuestIdAsync(int guestId)
        {
            await _connection.SetUpDb();
            var guestWeddings = await _connection._connection.Table<WeddingGuestIntermediate>()
                                                  .Where(wg => wg.GuestId == guestId)
                                                  .ToListAsync();
            return guestWeddings;
        }

        public async Task RemoveGuestFromWeddingAsync(int weddingId, int guestId)
        {
            await _connection.SetUpDb();

            var weddingGuest = await _connection._connection.Table<WeddingGuestIntermediate>()
                                        .Where(wg => wg.WeddingId == weddingId && wg.GuestId == guestId)
                                        .FirstOrDefaultAsync();

            var gueststable = await _connection._connection.Table<WeddingGuestIntermediate>().ToListAsync();


            var guestSeat = await _connection._connection.Table<GuestSeat>()
                                        .Where(gs => gs.GuestId == guestId)
                                        .FirstOrDefaultAsync();

            if (guestSeat != null)
            {
                guestSeat.GuestId = null;
                guestSeat.IsOccupied = false;
                await _connection._connection.UpdateAsync(guestSeat);
            }

            if (weddingGuest != null)
            {
                await _connection._connection.DeleteAsync(weddingGuest);
            }
            var table = await _connection._connection.Table<WeddingGuestIntermediate>().ToListAsync();
            var guestSeats = await _connection._connection.Table<GuestSeat>().ToListAsync();
        }

        public async Task<List<WeddingGuestIntermediate>> GetGuestsByWeddingIdAsync(int weddingId)
        {
            await _connection.SetUpDb();
            var weddingGuests = await _connection._connection.Table<WeddingGuestIntermediate>()
                                        .Where(wg => wg.WeddingId == weddingId)
                                        .ToListAsync();

            foreach (var wg in weddingGuests)
            {
                wg.Guest = await _connection._connection.Table<Guest>().FirstOrDefaultAsync(g => g.Id == wg.GuestId);
                if (wg.Guest != null)
                {
                    wg.Guest.User = await _connection._connection.Table<User>().FirstOrDefaultAsync(u => u.Id == wg.Guest.UserId);
                }

                wg.Wedding = await _connection._connection.Table<Wedding>().FirstOrDefaultAsync(w => w.Id == wg.WeddingId);
            }

            return weddingGuests;
        }

        public async Task<List<WeddingGuestIntermediate>> GetPendingInvitationsByGuestIdAsync(int guestId)
        {
            await _connection.SetUpDb();
            var pendingInvitations = await _connection._connection.Table<WeddingGuestIntermediate>()
                                        .Where(wg => wg.GuestId == guestId && wg.InvitationStatus == InvitationStatus.Pending)
                                        .ToListAsync();

            foreach (var weddingGuest in pendingInvitations)
            {
                weddingGuest.Guest = await _connection._connection.Table<Guest>().FirstOrDefaultAsync(g => g.Id == weddingGuest.GuestId);
                weddingGuest.Wedding = await _connection._connection.Table<Wedding>().FirstOrDefaultAsync(w => w.Id == weddingGuest.WeddingId);
            }

            return pendingInvitations;
        }

        public async Task UpdateWeddingGuestIntermediateAsync(WeddingGuestIntermediate weddingGuest)
        {
            await _connection.SetUpDb();
            await _connection._connection.UpdateAsync(weddingGuest);
        }

        public async Task<List<WeddingGuestIntermediate>> GetUnassignedGuestsByWeddingIdAsync(int weddingId)
        {
            await _connection.SetUpDb();

            // Get all guests for the wedding
            var weddingGuests = await _connection._connection.Table<WeddingGuestIntermediate>()
                                    .Where(wg => wg.WeddingId == weddingId && wg.InvitationStatus == InvitationStatus.Accepted)
                                    .ToListAsync();

            // Get all guest seats for the wedding
            var guestSeats = await _connection._connection.Table<GuestSeat>()
                                    .Where(gs => gs.GuestId != null)
                                    .ToListAsync();

            // Filter out guests who already have a seat assigned
            var unassignedGuests = weddingGuests
                .Where(wg => !guestSeats.Any(gs => gs.GuestId == wg.GuestId))
                .ToList();

            // Load guest and user details
            foreach (var wg in unassignedGuests)
            {
                wg.Guest = await _connection._connection.Table<Guest>()
                                 .FirstOrDefaultAsync(g => g.Id == wg.GuestId);

                if (wg.Guest != null)
                {
                    wg.Guest.User = await _connection._connection.Table<User>()
                                     .FirstOrDefaultAsync(u => u.Id == wg.Guest.UserId);
                }
            }

            return unassignedGuests;
        }


    }
}
