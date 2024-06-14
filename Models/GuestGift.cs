using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace Wedding_Planning_App.Models
{
    public class GuestGift
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [ForeignKey("Guest")]
        public int GuestId { get; set; }
        [Ignore]
        public Guest Guest { get; set; }

        [ForeignKey("GiftListItem")]
        public int GiftListItemId { get; set; }
        [Ignore]
        public GiftListItem GiftListItem { get; set; }

        public bool Purchased { get; set; }
    }
}
