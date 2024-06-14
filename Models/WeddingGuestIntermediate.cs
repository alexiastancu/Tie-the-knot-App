using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wedding_Planning_App.Models
{
    public class WeddingGuestIntermediate
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [ForeignKey("Wedding")]
        public int WeddingId { get; set; }

        [ForeignKey("Guest")]
        public int GuestId { get; set; }

        [Ignore]
        public Wedding Wedding { get; set; }

        [Ignore]
        public Guest Guest { get; set; }
    }
}
