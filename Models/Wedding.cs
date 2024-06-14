using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wedding_Planning_App.Data.Enums;
using SQLite;

namespace Wedding_Planning_App.Models
{
    public class Wedding
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        [ForeignKey("Fiances")]
        public int FiancesId { get; set; }

        //[Ignore]
        //public User User { get; set; }

        public DateTime WeddingDate { get; set; }

        public decimal Budget { get; set; }

        [ForeignKey("Location")]
        public int LocationId { get; set; }

        public int EstimatedGuestCount { get; set; }

        public WeddingAttire WeddingAttire { get; set; }

        //[Ignore]
        //public ICollection<Guest> Guests { get; set; }
        //[Ignore]
        //public ICollection<Vendor> Vendors { get; set; }
        //[Ignore]
        //public ICollection<VendorService> VendorServices { get; set; }
        //[Ignore]
        //public ICollection<FinanceItem> Finances { get; set; }
        //[Ignore]
        //public ICollection<TimelineEvent> TimelineEvents { get; set; }
        //[Ignore]
        //public ICollection<GuestSeat> GuestSeats { get; set; }
        //[Ignore]
        //public ICollection<Task> Tasks { get; set; }
        //[Ignore]
        //public ICollection<MenuItem> MenuItems { get; set; }
        //[Ignore]
        //public ICollection<GiftListItem> GiftListItems { get; set; }
    }
}
