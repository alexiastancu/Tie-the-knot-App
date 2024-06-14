using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wedding_Planning_App.Models
{
    public class Fiances
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        [Ignore]
        public User User { get; set; }

        public string HusbandName { get; set; }
        public string HusbandSurname { get; set; }
        public string WifeName { get; set; }
        public string WifeSurname { get; set; }

    }
}
