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
    public class Vendor
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        [Ignore]
        public User User { get; set; }

        public string CompanyName { get; set; }
        public string ServiceDescription { get; set; }
        [Ignore]
        public ICollection<Wedding> Weddings { get; set; }
        [Ignore]
        public ICollection<VendorService> VendorServices { get; set; }
    }
}
