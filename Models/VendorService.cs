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
    public class VendorService
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [ForeignKey("Vendor")]
        public int VendorId { get; set; }
        [Ignore]
        public Vendor Vendor { get; set; }

        [ForeignKey("Wedding")]
        public int WeddingId { get; set; }
        [Ignore]
        public Wedding Wedding { get; set; }

        public string ServiceName { get; set; }
        public decimal Price { get; set; }
    }
}
