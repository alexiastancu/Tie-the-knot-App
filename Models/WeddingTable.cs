using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wedding_Planning_App.Models
{
    public class WeddingTable
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [ForeignKey("Wedding")]
        public int WeddingId { get; set; }

        public int TableNumber { get; set; }

        [Ignore]
        public List<GuestSeat> Seats { get; set; }
    }
}
