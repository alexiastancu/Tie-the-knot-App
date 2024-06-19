using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wedding_Planning_App.Models;
using Wedding_Planning_App.Services.Interfaces;
using Wedding_Planning_App.Views.Fiances;

namespace Wedding_Planning_App.ViewModels.Fiances
{
    public partial class SeatingArrangementVM : ObservableObject
    {
        private readonly IWeddingTableService _weddingTableService;
        private readonly IGuestSeatService _guestSeatService;
        private readonly IGuestService _guestService;

        #region Properties
        [ObservableProperty]
        private ObservableCollection<WeddingTable> tables;

        [ObservableProperty]
        private WeddingTable selectedTable;

        [ObservableProperty]
        private GuestSeat selectedSeat;

        [ObservableProperty]
        private string selectedGuestName;

        [ObservableProperty]
        private bool isTableSelected;

        [ObservableProperty]
        private bool isAssignButtonVisible;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(SelectedTable))]
        [NotifyPropertyChangedFor(nameof(SelectedGuestName))]
        private bool shouldRefreshUI;
        #endregion

        public SeatingArrangementVM(IWeddingTableService weddingTableService, IGuestSeatService guestSeatService, IGuestService guestService)
        {
            _weddingTableService = weddingTableService;
            _guestSeatService = guestSeatService;
            _guestService = guestService;
            IsTableSelected = false;
            isAssignButtonVisible = false;
            LoadTables();
        }
        public SeatingArrangementVM()
        {
            
        }


        [RelayCommand]
        public async Task LoadTables()
        {
            bool conversionSucceded = int.TryParse(await SecureStorage.GetAsync("weddingId"), out int weddingId);
            if (!conversionSucceded)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "There was a problem retrieving the wedding, try again later", "OK");
                Debug.WriteLine("Error retrieving weddingId from SecureStorage or converting it to int");
                return;
            }
            var guests = await _guestService.GetGuestsByWeddingIdAsync(weddingId);

            // Calculează numărul de mese necesare
            int numberOfGuests = guests.Count;
            int numberOfTables = (int)Math.Ceiling(numberOfGuests / 8.0);

            // Creează mesele și locurile dacă nu există deja
            var existingTables = await _weddingTableService.GetTablesByWeddingIdAsync(weddingId);
            if (existingTables.Count < numberOfTables)
            {
                for (int i = existingTables.Count + 1; i <= numberOfTables; i++)
                {
                    var weddingTable = new WeddingTable
                    {
                        WeddingId = weddingId,
                        TableNumber = i,
                        Seats = new List<GuestSeat>()
                    };

                    await _weddingTableService.AddWeddingTableAsync(weddingTable);

                    for (int j = 1; j <= 8; j++)
                    {
                        await _guestSeatService.AddGuestSeatAsync(new GuestSeat
                        {
                            TableId = weddingTable.Id,
                            SeatNumber = j,
                            IsOccupied = false
                        });
                        weddingTable.Seats.Add(new GuestSeat
                        {
                            TableId = weddingTable.Id,
                            SeatNumber = j,
                            IsOccupied = false
                        });
                        await _weddingTableService.UpdateWeddingTableAsync(weddingTable);
                    }

                    

                }
            }

            // Încarcă mesele actualizate
            var updatedTables = await _weddingTableService.GetTablesByWeddingIdAsync(weddingId);
            foreach (var table in updatedTables)
            {
                table.Seats = new List<GuestSeat>(await _guestSeatService.GetSeatsByTableIdAsync(table.Id));
            }
            Tables = new ObservableCollection<WeddingTable>(updatedTables);
        }

        partial void OnSelectedTableChanged(WeddingTable value)
        {
            IsTableSelected = value != null;
        }

        [RelayCommand]
        public async System.Threading.Tasks.Task LoadSeats(WeddingTable table)
        {
            var seats = await _guestSeatService.GetSeatsByTableIdAsync(table.Id);
            table.Seats = new List<GuestSeat>(seats);
        }

        [RelayCommand]
        private async void SeatSelected(GuestSeat seat)
        {
            SelectedGuestName = seat.Guest?.User.Name + " " + seat?.Guest?.User.Surname ?? "this seat is not occupied";
            //SelectedGuestName = seat.GuestId?.ToString() ?? "This seat is not occupied";
            SelectedSeat = seat;
            isAssignButtonVisible = true;

            //SelectedGuestName = seat?.Guest.User.Name + " " + seat?.Guest.User.Surname ?? "this seat is not occupied";
            //SelectedGuestName = seat?.GuestId.ToString() ?? "this seat is not occupied";
            //SelectedSeat = seat;
            //var popup = new SeatGuestPopup(seat);
            //var result = await Application.Current.MainPage.ShowPopupAsync(popup);
            //await LoadSeats(SelectedTable);
            //ShouldRefreshUI = !ShouldRefreshUI;
        }

        [RelayCommand]
        private async Task OpenSeatPopup()
        {
            var popup = new SeatGuestPopup(SelectedSeat);
            Models.Guest result = (Models.Guest)await Application.Current.MainPage.ShowPopupAsync(popup);
            await LoadSeats(SelectedTable);
            //SelectedGuestName = result?.Id.ToString() ?? "This seat is not occupied";
            SelectedGuestName = result?.User.Name + " " + result?.User.Surname ?? "this seat is not occupied";

            ShouldRefreshUI = !ShouldRefreshUI;
            isAssignButtonVisible = false;
        }
    }
}

