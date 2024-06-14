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
    public class TimelineEvent
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [ForeignKey("Wedding")]
        public int WeddingId { get; set; }
        [Ignore]
        public Wedding Wedding { get; set; }

        public DateTime Time { get; set; }
        public string Description { get; set; }
        public string Section { get; set; }
    }
}
