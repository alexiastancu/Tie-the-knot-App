using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wedding_Planning_App.Services.Interfaces;

namespace Wedding_Planning_App.ViewModels.Fiances
{
    public partial class SeatingArrangementVM : ObservableRecipient
    {
        //private readonly IGuestService _guestService;

        //[ObservableProperty]
        //private ObservableCollection<Table> tables;

        //[ObservableProperty]
        //private Table selectedTable;

        //public SeatingArrangementVM(IGuestService guestService)
        //{
        //    _guestService = guestService;
        //    LoadTables();
        //}

        //private void LoadTables()
        //{
        //    // Load tables and their seating arrangements
        //    Tables = new ObservableCollection<Table>
        //    {
        //        new Table
        //        {
        //            Name = "Table 1",
        //            Seats = new ObservableCollection<Seat>
        //            {
        //                new Seat { Position = 1, GuestName = "John Doe", IsOccupied = true },
        //                new Seat { Position = 2, GuestName = "Jane Doe", IsOccupied = true },
        //                new Seat { Position = 3, GuestName = "Jack Doe", IsOccupied = true },
        //                new Seat { Position = 4, GuestName = "Jill Doe", IsOccupied = true },
        //                new Seat { Position = 5, GuestName = "Jake Doe", IsOccupied = true },
        //                new Seat { Position = 6, GuestName = "Jenny Doe", IsOccupied = true },
        //                new Seat { Position = 7, GuestName = "Jim Doe", IsOccupied = true },
        //                new Seat { Position = 8, GuestName = "Janet Doe", IsOccupied = true },
        //            }
        //        },
        //        // Add more tables as needed
        //    };
        //}

        //[RelayCommand]
        //private void OnSeatClicked(Seat seat)
        //{
        //    // Handle seat click to assign/view guest
        //    // This can open a popup or navigate to a new page to select/assign a guest
        //}
    }
}
