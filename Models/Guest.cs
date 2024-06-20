using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using Wedding_Planning_App.Data.Enums;

namespace Wedding_Planning_App.Models
{
    public class Guest
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        [Ignore]
        public User User { get; set; }
        public string DietaryRestrictions { get; set; }
        [Ignore]
        public ICollection<Wedding> Weddings { get; set; }
    }
}
